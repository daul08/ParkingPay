using System.ComponentModel.DataAnnotations;

namespace ParkingPay.Models
{
    public class EnteredCarModel
    {
        public int Id { get; set; }
        [Display(Name = "Номер автомобиля")]
        public string Number { get; set; }
        [Display(Name = "Дата и время въезда на парковку")]
        public DateTime TimeOfEntry { get; set; }
        [Display(Name = "Дата и время выезда с парковки")]
        public DateTime CheckOutTime { get; set; }
        [Display(Name = "Время стоянки")]
        public int? ParkingTimeinMinutes { get; set; }
        [Display(Name = "Оплата за парковку в тенге, составляет:")]
        public int? ParkingCost { get; set; }
        [Display(Name = "Оплачено")]
        public bool IsPaid { get; set; }
        [Display(Name = "Внесенная сумма")]
        public int? Paid { get; set; }
        int ParkingPrice = 50;

        public int GetCost()
        {
            if (GetParkingTimeInMinute() % 30 > 0)
            {
                var parkingCost = (GetParkingTimeInMinute() / 30) * ParkingPrice;
                return parkingCost + ParkingPrice;
            }
            else
            {
                var parkingCost = (GetParkingTimeInMinute() / 30) * ParkingPrice;
                return parkingCost;
            }
        }

        public int GetParkingTimeInMinute()
        {
            DateTime paymentTime = DateTime.Now;
            var parkingTimeInMinutes = ((paymentTime - TimeOfEntry).TotalMinutes);
            return (int)parkingTimeInMinutes;
        }

    }

    
}
