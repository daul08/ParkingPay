using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingPay.Models;

namespace ParkingPay.Data
{
    public class ParkingPayContext : DbContext
    {
        public ParkingPayContext (DbContextOptions<ParkingPayContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingPay.Models.EnteredCarModel> EnteredCarModel { get; set; } = default!;
    }
}
