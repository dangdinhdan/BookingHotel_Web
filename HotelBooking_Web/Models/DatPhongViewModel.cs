using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBooking_Web.Models
{
    public class DatPhongViewModel
    {
        public int PhongID { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int SoNguoi { get; set; }

        public int SoDem => (CheckOut - CheckIn).Days;
    }

    
}