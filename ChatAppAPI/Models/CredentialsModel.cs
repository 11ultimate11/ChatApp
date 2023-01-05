using ChatAppAPI.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChatAppAPI.Models
{
    public class CredentialsModel : AbstractSqlModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        [NotMapped]
        public int PersonID { get; set; }
    }
}
