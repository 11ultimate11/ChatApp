using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Models.Abstract
{
    public abstract class AbstractSqlModel : ObservableObject
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
