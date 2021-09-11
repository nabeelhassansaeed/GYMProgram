using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.Models
{
    public class GYM
    {
        public GYM()
        {
            this.Customers = new HashSet<Customer>();
            this.Sections = new HashSet<Section>();
        }

        [Key]//, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  

        public System.Guid Guid { get; set; }
        [Required(ErrorMessage = "حقل إجباري"), MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string LatinName { get; set; }
        [MaxLength(250)]
        public string Location { get; set; }
        [DataType(DataType.MultilineText)]
        [MaxLength(800)]
        public string Notes { get; set; }
        [MaxLength(250)]
        public string Logo { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}
