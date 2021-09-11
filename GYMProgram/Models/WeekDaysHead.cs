using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class WeekDaysHead
    {
        public WeekDaysHead()
        {
            this.weekDays = new HashSet<WeekDay>();
        }

        [Key]
        public System.Guid Guid { get; set; }

        [ForeignKey("Section")]
        public Guid SectionGuid { get; set; }

        [Required(ErrorMessage = "حقل إجباري"), MaxLength(800)]      
        public string Notes { get; set; }

        public int Number { get; set; }
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]       
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public DateTime ToDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Section Section { get; set; }

        public virtual ICollection<WeekDay> weekDays { get; set; }
    }
}
