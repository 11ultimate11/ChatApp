using ChatAppAPI.Models.Abstract;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppAPI.Models;

public class ChatMessageModel : AbstractSqlModel
{
    [ForeignKey(nameof(Person))]
    public int PersonID { get; set; }
    [ForeignKey(nameof(Chat))]
    public int ReferenceID { get; set; }
    public string? Content { get; set; }
    [JsonIgnore]
    public virtual PersonModel? Person { get; set; }
    [JsonIgnore]
    public virtual ChatModel? Chat { get; set; }
}
