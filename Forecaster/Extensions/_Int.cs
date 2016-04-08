using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daisybrand.forecaster.Extensions
{
    public static class _Int
    {
        public static string AddLeadingZeros(this int i)
        {
            var s = i.ToString();
            var total = 6 - s.Length;
            if (total > 0)
                for (int c = 0; c < total; c++)
                    s = "0" + s;
            return s;
        }

        public static decimal ToDecimal(this int i)
        {
            return Convert.ToDecimal(i);
        }
    }
}
