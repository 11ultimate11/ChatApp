using GhostLibrary.Models.Abstract;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using static GhostLibrary.Helpers.Enums;

namespace GhostLibrary.Models;

public class ChatModel : AbstractSqlModel
{
    [ForeignKey(nameof(OwnerModel))]
    public int OwnerID { get; set; }
    [ForeignKey(nameof(TargetModel))]
    public int TargetID { get; set; }
    public int MsjCount { get; set; }
    public string? LastMessage { get; set; }
    [JsonIgnore]
    public virtual PersonModel? OwnerModel { get; set; }
    [JsonIgnore]
    public virtual PersonModel? TargetModel { get; set; }
    public ChatType? ChatType { get; set; }
    //[NotMapped]
    //public List<ChatKeyModel>? Keys { get; set; } = new List<ChatKeyModel>();
}
