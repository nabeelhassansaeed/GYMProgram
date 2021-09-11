using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class UnavailableDate
    {
        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Guid { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }
        [MaxLength(800)]
        public string Notes { get; set; }

        [ForeignKey("Section")]
        public Nullable<System.Guid> SectionGuid { get; set; }

        public virtual Section Section { get; set; }
    }
}
