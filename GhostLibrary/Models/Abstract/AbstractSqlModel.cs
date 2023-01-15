using System.ComponentModel.DataAnnotations;

namespace GhostLibrary.Models.Abstract;

public abstract class AbstractSqlModel 
{
    [Key]
    public int ID { get; set; }
    public DateTime CreatedDate { get; set; }
}
