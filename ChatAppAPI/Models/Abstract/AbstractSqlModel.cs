using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatAppAPI.Models.Abstract;

public abstract class AbstractSqlModel
{
    [Key]
    public int ID { get; set; }
    public DateTime CreatedDate { get; set; }
}
