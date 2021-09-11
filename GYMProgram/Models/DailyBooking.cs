using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class DailyBooking
    {
        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Guid { get; set; }

        public Nullable<System.Guid> CUGuid { get; set; }

        public Nullable<System.Guid> BookingGuid { get; set; }

        [DataType(DataType.DateTime)]
        public System.DateTime Date { get; set; }
        
        public bool Status { get; set; }
        public int Number { get; set; }

        public System.DateTime StartDate { get; set; }
        
        public System.DateTime EndDate { get; set; }

        [ForeignKey("CUGuid")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("BookingGuid")]
        public virtual Bookings booking { get; set; }
    }
}
