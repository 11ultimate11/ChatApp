using ChatApp.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Models
{
    public class ChatKeyModel : AbstractSqlModel
    {
        public int PersonID { get; set; }
        public int ReferenceID { get; set; }
        public int LastSeenID { get; set; }
    }
}
