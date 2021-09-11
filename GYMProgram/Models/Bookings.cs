using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class Bookings
    {
        public Bookings()
        {
            this.dailyBooking = new HashSet<DailyBooking>();            
        }
        [Key]
        public System.Guid Guid { get; set; }
        public int Number { get; set; }
        public Nullable<System.Guid> SectionGuid { get; set; }
        
        public int Status { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
        //[DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }
        //[DataType(DataType.Date)]
        public System.DateTime EndDate { get; set; }
        public int QTYCustomers { get; set; }

        [ForeignKey("SectionGuid")]
        public virtual Section Section { get; set; }

        public virtual ICollection<DailyBooking> dailyBooking { get; set; }

    }
}
