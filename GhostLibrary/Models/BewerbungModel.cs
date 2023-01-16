using GhostLibrary.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLibrary.Models
{
    public class BewerbungModel : AbstractSqlModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string?  Telefon { get; set; }
        [Required]
        public string? Content { get; set; }
        [ForeignKey(nameof(Job))]
        public int JobID { get; set; }
        public JobModel? Job { get; set; }
        [NotMapped]
        public List<BewerbungModel>? Bewerbungen { get; set; }
    }
}
