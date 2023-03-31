using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ParkingPay.Data;
using ParkingPay.Models;

namespace ParkingPay.Controllers
{
    public class EnteredCarModelsController : Controller
    {
        private readonly ParkingPayContext _context;

        public EnteredCarModelsController(ParkingPayContext context)
        {
            _context = context;
        }

        // GET: EnteredCarModels/SearchCar
        public IActionResult SearchCar()
        {
            return View();
        }

        // POST: EnteredCarModels/SearchCar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchCar(string Number)
        {

            if (ModelState.IsValid)
            {
                
                var car = await _context.EnteredCarModel.Where(i => i.Number == Number).FirstOrDefaultAsync(m => m.IsPaid == false);
                if (car == null)
                {

                    return RedirectToAction(nameof(ErrorCar));
                }
                if (car.IsPaid != true)
                {
                    return RedirectToAction(nameof(Details), new
                    {
                        id = car.Id
                    });
                }
                else return RedirectToAction(nameof(ErrorCar));
            }
            return RedirectToAction(nameof(ErrorCar));

        }

        // GET: EnteredCarModels/HomeIndex
        public async Task<IActionResult> HomeIndex()
        {
            return _context.EnteredCarModel != null ?
                        View(await _context.EnteredCarModel.ToListAsync()) :
                        Problem("Entity set 'ParkingPayContext.EnteredCarModel'  is null.");
        }

        // GET: EnteredCarModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EnteredCarModel == null)
            {
                return NotFound();
            }

            var enteredCarModel = await _context.EnteredCarModel
                .FirstOrDefaultAsync(m => m.Id == id);
            enteredCarModel.ParkingCost = enteredCarModel.GetCost();
            enteredCarModel.ParkingTimeinMinutes = enteredCarModel.GetParkingTimeInMinute();


            if (enteredCarModel == null)
            {
                return NotFound();
            }

            return View(enteredCarModel);
        }

        // POST: EnteredCarModels/Details/5
        [HttpPost]
        public async Task<IActionResult> Details(int id, int? Paid)
        {
            if (Paid == null || _context.EnteredCarModel == null)
            {
                return NotFound();
            }

            var car = await _context.EnteredCarModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            if (car.GetCost() == Paid)

            {
                car.ParkingCost = car.GetCost();
                car.ParkingTimeinMinutes = car.GetParkingTimeInMinute();
                car.IsPaid = true;
                car.Paid = Paid;
                car.CheckOutTime = DateTime.Now;

                _context.Update(car);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(PaymentSuccess));
            }
            else if (car.GetCost() < Paid)
            {
                return RedirectToAction(nameof(PaymentOverError));
            }
            else return RedirectToAction(nameof(PaymentBelowError));

            
        }

        // GET: EnteredCarModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EnteredCarModels/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number, TimeOfEntry")] EnteredCarModel enteredCarModel)
        {
            if (ModelState.IsValid)
            {
                var car = await _context.EnteredCarModel.Where(x => x.Number == enteredCarModel.Number)
                    .FirstOrDefaultAsync(m => m.IsPaid == false);

                if (car == null)
                {

                    var car2 = new EnteredCarModel
                    {
                        Id = enteredCarModel.Id,
                        Number = enteredCarModel.Number,
                        TimeOfEntry = DateTime.Now,

                    };
                    _context.Add(car2);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(HomeIndex));
                }
                else return RedirectToAction(nameof(ErrorAlreadyParked));

            }
            return View(enteredCarModel);
        }

        //GET: EnteredCarModels/PaymentSucces
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        //GET: EnteredCarModels/ErrorAlreadyParked
        public IActionResult ErrorAlreadyParked()
        {
            return View();
        }

        //GET: EnteredCarModels/PaymentError
        public IActionResult PaymentOverError()
        {
            return View();

        }
        //GET: EnteredCarModels/PaymentError
        public IActionResult PaymentBelowError()
        {
            return View();
        }
        //GET: EnteredCarModels/PaymentError
        public IActionResult ErrorCar()
        {
            return View();
        }
        
        //GET: EnteredCarModels/report
        public async Task<IActionResult> ReportAsync()
        {
            using (var p = new ExcelPackage())
            {

                //A workbook must have at least on cell, so lets add one... 
                var ws = p.Workbook.Worksheets.Add("MySheet");
                
                ws.Cells[1, 1, 1, 8].LoadFromArrays(new object[][] { new[] { "ID", "Номер авто", "Дата и время въезда", "Сумма за парковку", "Время стоянки в минутах", "Внесенная сумма", "Дата и время выезда с парковки", "Оплачено" } });
                DateTime dateTime = DateTime.Today;

                var row = 2;
                
                foreach (var item in await _context.EnteredCarModel.Where(t => t.TimeOfEntry.Date == dateTime || t.IsPaid ==false).ToListAsync()) 
                {
                    ws.Cells[row, 1].Value = item.Id;
                    ws.Cells[row, 2].Value = item.Number;
                    ws.Cells[row, 3].Value = item.TimeOfEntry.ToString();
                    ws.Cells[row, 4].Value = item.ParkingCost;
                    ws.Cells[row, 5].Value = item.ParkingTimeinMinutes;
                    ws.Cells[row, 6].Value = item.Paid;
                    ws.Cells[row, 7].Value = item.CheckOutTime.ToString();
                    ws.Cells[row, 8].Value = item.IsPaid;
                    row++;
                    

                    
                }

                ws.Cells[1, 1, row, 8].AutoFitColumns();

                ws.Cells[1, 1, 1, 8].Style.Font.Bold = true;

                
                ws.Cells[1, 1, 1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                //Save the new workbook. We haven't specified the filename so use the Save as method.
                var excelByteArray = await p.GetAsByteArrayAsync();

                return File(excelByteArray, "application/xlsx", $"report_{dateTime.ToString("d")}.xlsx");
            }
            
        }
    }
    }



       