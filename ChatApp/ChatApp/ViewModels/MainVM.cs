using ChatApp.Models;
using ChatApp.Models.Interfaces;
using ChatApp.Services.Global;
using ChatApp.Services.Interfaces;
using ChatApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ChatApp.ViewModels
{
    public class MainVM
    {
        /// <summary>
        /// Get Dependency
        /// </summary>
        private readonly IApiProcessor api;
        public ObservableRangeCollection<IChatModel> Models { get; private set; } = new ObservableRangeCollection<IChatModel>();
        public MainVM()
        {
            api = DependencyService.Get<IApiProcessor>(DependencyFetchTarget.GlobalInstance);
            InternalDelegates.OnUserPick+=InternalDelegates_OnUserPick;
            InternalDelegates.OnBroadcast+=InternalDelegates_OnBroadcast;
            LoadChatsCommand = new AsyncCommand(LoadChatsModelAsync);
            LoadUsersCommand = new Command(LoadUsers);
            SelectCommand = new Command(Select);
        }


        #region Fields
        private bool _isBusy;
        private bool _noMore;
        #endregion
        #region Commands
        public AsyncCommand LoadChatsCommand { get; private set; }
        public Command LoadUsersCommand { get; private set; }   
        public Command SelectCommand { get; private set; }   
        #endregion

        #region Methods
        /// <summary>
        /// Load async chatmodels from api
        /// </summary>
        /// <returns></returns>
        private async Task LoadChatsModelAsync()
        {
            if (_isBusy || _noMore) return;
            _isBusy = true;
            var json = await api?.GetStringAsync($"Chat/Getchats/{Models.Count}");
            if (!string.IsNullOrEmpty(json))
            {
                List<ChatModel> models = JsonConvert.DeserializeObject<List<ChatModel>>(json);
                if(models != null && models.Any())
                {
                    Models.AddRange(models);
                }
                _noMore = !models.Any();
            }
            else
            {
                _noMore = true;
            }
            _isBusy = false;
        }
        private async void Select(object obj)
        {
            if(obj is ChatModel model)
            {
                AppSettings.ChatModel = model;
                await Shell.Current.GoToAsync(nameof(ChatView));
            }
        }
        private void LoadUsers()
        {
            App.Instance.ShowUsersPopUp();
        }
        #endregion
        #region Delegates
        private async void InternalDelegates_OnUserPick(PersonModel obj)
        {
            var find = Models.ToList().Find(x => (x.OwnerID == obj.ID && x.TargetID == AppSettings.Person.ID) || (x.TargetID == obj.ID && x.OwnerID == AppSettings.Person.ID));
            if(find != null)
            {
                AppSettings.ChatModel= find;
                await Shell.Current.GoToAsync(nameof(ChatView));
            }
            else
            {
                ChatModel chatModel = new ChatModel()
                {
                    OwnerID = AppSettings.Person.ID,
                    TargetID = obj.ID,
                    ChatType = ChatType.single,
                    CreatedDate = DateTime.Now,
                };
                var result = await api.PostAsync(chatModel, "Chat");
                if (result)
                {
                    Models.Add(chatModel);
                    AppSettings.ChatModel= chatModel;
                    await Shell.Current.GoToAsync(nameof(ChatView));
                }
            }
        }

        private void InternalDelegates_OnBroadcast(ChatMessageModel obj)
        {
            var find = Models.ToList().Find(x=> x.ID == obj.ReferenceID);
            if(find != null)
            {
                find.Count++;
                find.LastMessage = obj.Content;
                find.ChatMessages.Add(obj);
            }
            else
            {
                ChatModel chatModel = new ChatModel()
                {
                    ID = obj.ReferenceID,
                    LastMessage = obj.Content,
                    OwnerID = obj.PersonID,
                    TargetID = AppSettings.Person.ID,
                    CreatedDate = DateTime.Now,
                    ChatType = ChatType.single,
                };
                chatModel.ChatMessages.Add(obj);
                Models.Insert(0,chatModel);
            }
        }
        #endregion
    }
}
