using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class Customer
    {

        public Customer()
        {
            this.DailyBookings = new HashSet<DailyBooking>();
            
        }

        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public System.Guid Guid { get; set; }
            [Required(ErrorMessage = "حقل إجباري"), MaxLength(150)]
            public string Name { get; set; }
            [MaxLength(150)]
            public string LatinName { get; set; }
            [MaxLength(800)]
            public string IDNumber { get; set; }
            public string Mobile { get; set; }
            public bool Status { get; set; }
            public string Notes { get; set; }
            [DataType(DataType.Date)]
            //[DefaultValue(typeof(DateTime), DateTime.Now.ToString("yyyy-MM-dd"))]
            public System.DateTime ExpirDate { get; set; }

            public System.Guid GYMGuid { get; set; }

        [ForeignKey("GYMGuid")]
        public virtual GYM GYM { get; set; }
        public virtual ICollection<DailyBooking> DailyBookings { get; set; }
       
    }
}

