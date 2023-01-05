using ChatApp.Models;
using ChatApp.Models.Interfaces;
using ChatApp.Services.Global;
using ChatApp.Services.Interfaces;
using ChatApp.Services.Processor;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChatApp.ViewModels
{
    public class ChatVM : BaseViewModel
    {
        private readonly IApiProcessor api;
        private readonly IChatModel chatModel;
        private readonly IServerProcessor server;
        private readonly CollectionView _collection;
        public ObservableRangeCollection<ChatMessageModel> Models { get; private set; } = new ObservableRangeCollection<ChatMessageModel>();
        private string _route => $"Chat/GetMessages/{chatModel.ID}/{Models.Count}";
        const string _routePost = "Chat/AddChatMsj";
        public ChatVM(CollectionView _collection)
        {
            this._collection= _collection;
            api = DependencyService.Get<IApiProcessor>();
            server = DependencyService.Get<IServerProcessor>(DependencyFetchTarget.GlobalInstance);
            chatModel = AppSettings.ChatModel;
            LoadMoreCommand = new AsyncCommand(LoadMoreAsync);
            SendCommand = new AsyncCommand(SendAsync);
            Models.AddRange(chatModel.ChatMessages);
            InternalDelegates.OnBroadcast+=InternalDelegates_OnBroadcast;
        }

        #region Fields
        private bool _noMore;
        private bool _isSending;
        private string _input;
        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged(nameof(Input));
            }
        }
        #endregion
            #region Commands
        public AsyncCommand LoadMoreCommand { get; private set; }
        public AsyncCommand SendCommand { get; private set; }
        #endregion
        #region Methods

        private void InternalDelegates_OnBroadcast(ChatMessageModel obj)
        {
            if(obj.ReferenceID == chatModel.ID)
            {
                Models.Insert(0, obj);
                Device.BeginInvokeOnMainThread(() =>
                {
                    _collection.ScrollTo(0);
                });
            }
        }
        private async Task LoadMoreAsync()
        {
            if (_noMore || IsBusy) return;
            IsBusy = true;
            var result = await api.GetStringAsync(_route);
            if (!string.IsNullOrEmpty(result))
            {
                var models = JsonConvert.DeserializeObject<List<ChatMessageModel>>(result);
                if (models != null && models.Any())
                {
                    Models.AddRange(models);
                    _noMore = !models.Any();
                }
            }
            else
            {
                _noMore = true;
            }
            IsBusy= false;
        }
        private async Task SendAsync()
        {
            if (_isSending) return;
            _isSending = true;
            ChatMessageModel model = new ChatMessageModel()
            {
                CreatedDate = DateTime.Now,
                Content = Input,
                PersonID = AppSettings.Person.ID,
                ReferenceID = AppSettings.ChatModel.ID
            };
            var result = await api.PostAsync(model, _routePost);
            if (result)
            {
                chatModel.ChatMessages.Add(model);
                Models.Insert(0, model);
                chatModel.Count++;
                chatModel.LastMessage = Input;
                Device.BeginInvokeOnMainThread(() =>
                {
                    _collection.ScrollTo(0);
                });
                await server.Broadcast(model, $"Person{chatModel.Target.ID}");
            }
            else
            {
                App.Instance.DisplayToast("Failed to upload chat message model");
            }
            Input = string.Empty;
            _isSending = false;
        }
        public async void OnAppearing()
        {
            await Task.Delay(150);
            await LoadMoreCommand.ExecuteAsync();
        }
        public void OnDisappearing()
        {
            InternalDelegates.OnBroadcast-=InternalDelegates_OnBroadcast;
        }
        #endregion

    }
}
