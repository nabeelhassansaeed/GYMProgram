using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class Section
    {
        public Section()
        {
            this.bookings = new HashSet<Bookings>();
            this.weekDaysHead = new HashSet<WeekDaysHead>();
            this.unavailableDate = new HashSet<UnavailableDate>();

        }

        [Key]
        public System.Guid Guid { get; set; }
        [Required(ErrorMessage = "حقل إجباري"), MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string LatinName { get; set; }
        [MaxLength(800)]
        public string Notes { get; set; }

        //[Required(ErrorMessage = "حقل إجباري"), MaxLength(150)]

        [ForeignKey("GYM")]
        public Nullable<System.Guid> GYMGuid { get; set; }

        public virtual GYM GYM { get; set; }

        public virtual ICollection<Bookings> bookings { get; set; }
        public virtual ICollection<WeekDaysHead> weekDaysHead { get; set; }
        public virtual ICollection<UnavailableDate> unavailableDate { get; set; }


    }
}
