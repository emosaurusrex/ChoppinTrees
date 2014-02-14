using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data
{
    public static class Date
    {
        public static DateTime FromJava(double timeStamp)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            date = date.AddSeconds(Math.Round(timeStamp / 1000)).ToLocalTime();
            return date;
        }
    }
}
