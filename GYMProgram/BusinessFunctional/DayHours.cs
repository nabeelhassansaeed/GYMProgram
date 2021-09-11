using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.BusinessFunctional
{
    public class DayHours
    {
        List<Hour> Hours = new List<Hour>();

        public DayHours()
        {
            Hours.Add(new Hour { HourValue = "", HourText = "إختيار" });
        }
        public List<Hour> GetHoursList() {

            for (int i = 1; i <= 24; i++)
            {
            
            
                    Hour hour = new Hour();
                    hour.HourValue =Convert.ToString( i);
                    hour.HourText = Convert.ToString(i) ;
                    Hours.Add(hour);
            }
            return Hours;
        }
    }
   public class Hour
    {
        public string HourValue { get; set; }
        public string HourText { get; set; }
    }
    
}
