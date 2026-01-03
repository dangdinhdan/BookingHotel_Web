using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace HotelBooking_Web.Models
{
    // Từ khóa "partial" rất quan trọng để nó hiểu đây là một phần của lớp DataClasses1DataContext
    public partial class DataClasses1DataContext
    {
        // Hàm khởi tạo không tham số (Constructor 0 arguments)
        public DataClasses1DataContext() :
            base(ConfigurationManager.ConnectionStrings["QLKSConnectionString2"].ConnectionString)
        {
            OnCreated();
        }
    }
}