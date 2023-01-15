using GhostLibrary.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostLibrary.Models;

public class ZettelModel : AbstractSqlModel
{
    [Required(ErrorMessage = "Please enter {0}.")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} is not valid.")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Please enter {0}.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "{0} is not valid.")]
    public string? Content { get; set; }
}
