using System.Data.Entity;

namespace HotelBooking_Web.Models
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext() : base("name=QLKS") { }

        public DbSet<Customer> Customers { get; set; }
    }
}
