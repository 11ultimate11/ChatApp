using ChatApp.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Models
{
    public class ChatMessageModel : AbstractSqlModel
    {
        public int PersonID { get; set; }
        public int ReferenceID { get; set; }
        public string Content { get; set; }
        public string Date => CreatedDate.ToString("ddd dd.mm.yy");
    }
}
