using EntityFrameworkCore.Triggers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class WeekDay
    {
        [Key]
        public System.Guid Guid { get; set; }

        //[Required(ErrorMessage = "حقل إجباري"), MaxLength(150)]
        public int DayNumber { get; set; }
        [MaxLength(20)]
        public string DayName { get; set; }
        [MaxLength(20)]
        public string DayLatinName { get; set; }

        [MaxLength(800)]
        public string Notes { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime WorkStartHour { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime WorkEndHour { get; set; }

        [MaxLength(20)]
        public string Minutes { get; set; }

        [ForeignKey("WeekDaysHead")]
        public Nullable<System.Guid> HeadGuid { get; set; }

        public int QTYCustomers { get; set; }
        public int Number { get; set; }
       
        public virtual WeekDaysHead WeekDaysHead { get; set; }

        // لكي يعمل لا بد من يحميل الحزمة  Install-Package EntityFrameworkCore.Triggers -Version 1.2.3
        //static WeekDay()
        //{
        //    Triggers<WeekDay>.Inserting += entry => entry.Entity.Inserted = (entry.Entity.WorkStartHour.ToShortTimeString() +
        //    entry.Entity.WorkEndHour.ToShortTimeString());
        //    //Triggers<WeekDay>.Updating += entry => entry.Entity.Updated = DateTime.UtcNow;
        //}

    }
}
