using GhostLibrary.Models.Abstract;

namespace GhostLibrary.Models;

public class AntragModel : AbstractSqlModel
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Telefon { get; set; }
    public string?  Content { get; set; }
    public int  Platz { get; set; }
}
