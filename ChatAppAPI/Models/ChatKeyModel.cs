
using ChatAppAPI.Models.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChatAppAPI.Models;

public class ChatKeyModel : AbstractSqlModel
{
    [ForeignKey(nameof(Person))]
    public int PersonID { get; set; }
    [ForeignKey(nameof(Chat))]
    public int ReferenceID { get; set; }
    public int LastSeenID { get; set; }
    [JsonIgnore]
    public virtual PersonModel? Person { get; set; }
    [JsonIgnore]
    public virtual ChatModel? Chat { get; set; }
}
