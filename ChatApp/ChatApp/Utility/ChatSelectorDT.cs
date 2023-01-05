using ChatApp.Models;
using ChatApp.Services.Global;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChatApp.Utility
{
    class ChatSelectorDT : DataTemplateSelector
    {
        public DataTemplate EigeneDT { get; set; }
        public DataTemplate FremdDT { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ChatMessageModel model)
            {
                return model.PersonID == AppSettings.Person.ID ? EigeneDT : FremdDT;
            }
            return null;
        }
    }
}
