using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.ViewModels
{
    
    public class ListWeekDays
    {
        public List<WeekDays> GetListWeekDays()
        {
            List<WeekDays> weekDays = new List<WeekDays>();
            weekDays.Add(new WeekDays
            {
                DayNumber = 0,
                DayName = "الاحد",
                DayLatinName = "Sunday"
            });
            weekDays.Add(new WeekDays
            {
                DayNumber = 1,
                DayName = "الاثنين",
                DayLatinName = "Monday"
            });
            weekDays.Add(new WeekDays
            {
                DayNumber = 2,
                DayName = "الثلاثاء",
                DayLatinName = "Tuesday"
            });
            weekDays.Add(new WeekDays
            {
                DayNumber = 3,
                DayName = "الاربعاء",
                DayLatinName = "Wednesday"
            });
            weekDays.Add(new WeekDays
            {
                DayNumber = 4,
                DayName = "الخميس",
                DayLatinName = "Thursday"
            });
            weekDays.Add(new WeekDays
            {
                DayNumber = 5,
                DayName = "الجمعة",
                DayLatinName = "Friday"
            });
            weekDays.Add(new WeekDays
            {
                DayNumber = 6,
                DayName = "السبت",
                DayLatinName = "Saturday"
            });
            return weekDays;
        }
        public WeekDays FindWeekDays(int DayNumber)
        {
            var list = GetListWeekDays();
            WeekDays weekDay = list.Find(s=>s.DayNumber==DayNumber);
            return weekDay;
        }
    }
    public class WeekDays
    {
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public string DayLatinName { get; set; }
    }
}
