using GhostLibrary.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GhostLibrary.Helpers.Enums;

namespace GhostLibrary.Models;

public class JobModel : AbstractSqlModel
{
    public JobFunction Position { get; set; }
    public JobType Hours { get; set; }
    [Required(ErrorMessage = "Please enter {0}")]
    [StringLength(150, MinimumLength = 5, ErrorMessage = "{0} is not valid.")]
    public string? Vergütung { get; set; }
    [Required(ErrorMessage = "Please enter {0}")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "{0} is not valid.")]
    public string? Content { get; set; }
}
