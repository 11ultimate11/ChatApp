using GhostLibrary.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace GhostLibrary.Models;

public class AntragModel : AbstractSqlModel
{
    [Required(ErrorMessage = "Please enter {0}.")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Please enter {0}.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Please enter {0}.")]
    public string? Telefon { get; set; }
    [Required(ErrorMessage = "Please enter {0}.")]
    public string?  Content { get; set; }
    [Required(ErrorMessage = "Please enter {0}.")]
    public int  Platz { get; set; }
}
