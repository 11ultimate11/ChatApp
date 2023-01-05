
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppServer.Models;

public class ChatMessageModel 
{
    public int ID { get; set; }
    public DateTime CreatedDate { get; set; }
    public int PersonID { get; set; }
    public int ReferenceID { get; set; }
    public string? Content { get; set; }
}
