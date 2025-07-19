using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_N6_ver3
{
    class connectDb
    {
        public static string connectString()
        {
            return @"Data Source = LYDINHSON\SQLEXPRESS; Initial Catalog = BTL_N6_QLKS; Integrated Security = True";
        }
    }
}
