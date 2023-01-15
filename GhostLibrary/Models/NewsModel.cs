using GhostLibrary.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GhostLibrary.Models
{
    public class NewsModel : AbstractSqlModel
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        [ForeignKey(nameof(Media))]
        public int MediaID { get; set; }
        [Required]
        public MediaModel? Media { get; set; }
    }
}
