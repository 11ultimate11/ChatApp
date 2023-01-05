using ChatApp.Models;
using ChatApp.Services;
using ChatApp.Services.Global;
using ChatApp.Services.Interfaces;
using ChatApp.Utility;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChatApp.ViewModels
{
    public class SettingsVM : BaseViewModel
    {
        public PersonModel Person { get; private set; } = AppSettings.Person;
        private readonly IApiProcessor api;
        public SettingsVM()
        {
            api = DependencyService.Get<IApiProcessor>();
            EditCommand = new AsyncCommand(EditAsync);
            _checkbox = Preferences.Get("autologin", false);
        }

        public AsyncCommand EditCommand { get; set; }
        private bool _checkbox;
        public bool CheckBoxState
        {
            get => _checkbox;
            set
            {
                _checkbox= value;
                Preferences.Set("autologin", _checkbox);
                if(_checkbox)
                {
                    Preferences.Set("username", Person.Credentials.Username);
                    Preferences.Set("password", Person.Credentials.Password);
                }
                else
                {
                    Preferences.Set("username", string.Empty);
                    Preferences.Set("password", string.Empty);
                }
            }
        }
        private int _chatsCount;
        public int ChatsCount
        {
            get => _chatsCount;
            set
            {
                _chatsCount = value;
                OnPropertyChanged(nameof(ChatsCount));
            }
        }
        private int _msjsCount;
        public int MsjsCount
        {
            get => _msjsCount;
            set
            {
                _msjsCount = value;
                OnPropertyChanged(nameof(MsjsCount));
            }
        }
        private async Task EditAsync()
        {
            var pick = await MediaPicker.PickPhotoAsync();
            if(pick != null)
            {
                var media = AppSettings.GetMediaModelFromPath(pick.FullPath);
                if(media != null)
                {
                    Person.Media.Key = media.Key;
                    Person.Media.Data = media.Data;
                    Person.UpdateMediaUI();
                    _ = await api.PutAsync(Person, "Account/UpdatePerson");
                }
            }
        }
        private async Task GetCounts()
        {
            var result = await api.GetStringAsync("Chat/GetCounts");
            if(string.IsNullOrEmpty(result)) { return; }
            var obj = JSONObject.Parse(result);
            ChatsCount = int.Parse(obj["chats"].Value);
            MsjsCount = int.Parse(obj["msjs"].Value);
        }
        public void OnAppearing()
        {
            Task.Run(GetCounts);
        }
    }
}
