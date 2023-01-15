using GhostLibrary.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GhostLibrary.Models
{
    public class CredentialsModel : AbstractSqlModel
    {
        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Username is not valid.")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter {0}.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Password is not valid.")]
        public string? Password { get; set; }
        [NotMapped]
        public int PersonID { get; set; }
    }
}
