using ChatApp.Models.Abstract;
using ChatApp.Models.Interfaces;
using ChatApp.Services.Global;
using ChatApp.Services.Processor;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChatApp.Models
{
    public class ChatModel : AbstractSqlModel, IChatModel
    {
        public int OwnerID { get; set; }
        public int TargetID { get; set; }
        public int Count { get; set; }
        public ChatType ChatType { get; set; }
        private string _lastmsj;
        public string LastMessage
        {
            get => _lastmsj;
            set
            {
                _lastmsj = value;
                OnPropertyChanged(nameof(LastMessage));
            }
        }
        public List<ChatKeyModel> Keys { get; set; } = new List<ChatKeyModel>();
        [JsonIgnore]
        public ObservableRangeCollection<ChatMessageModel> ChatMessages { get; set; } = new ObservableRangeCollection<ChatMessageModel>();

        private PersonModel _targeT;
        [JsonIgnore]
        public PersonModel Target
        {
            get => _targeT ?? GetTargetModel();
            private set => _targeT = value;
        }
        private PersonModel _owner;
        [JsonIgnore]
        public PersonModel Owner
        {
            get => _owner ?? GetOwnerModel();
            private set => _owner = value;
        }
        private PersonModel GetOwnerModel()
        {
            Task.Run(async () =>
            {
                var id = OwnerID == AppSettings.Person.ID ? OwnerID : TargetID;
                var model = await DependencyService.Get<IUsersProcessor>(DependencyFetchTarget.GlobalInstance).GetPerson(id);
                if (model != null)
                {
                    Owner = model;
                    OnPropertyChanged(nameof(Owner));
                }
            });
            return null;
        }
        private PersonModel GetTargetModel()
        {
            Task.Run(async () =>
            {
                var id = OwnerID != AppSettings.Person.ID ? OwnerID : TargetID;
                var model = await DependencyService.Get<IUsersProcessor>(DependencyFetchTarget.GlobalInstance).GetPerson(id);
                if (model != null)
                {
                    Target = model;
                    OnPropertyChanged(nameof(Target));
                }
            });
            return null;
        }
    }
}
