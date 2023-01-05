using ChatApp.Services.Interfaces;
using ChatApp.Services.Processor;
using ChatApp.Views;
using MvvmHelpers;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChatApp.ViewModels
{
    /// <summary>
    /// This is the viewmodel of the Login Page
    /// </summary>
    public class LoginVM : BaseViewModel
    {
        private readonly ILoginProcessor processor;
        /// <summary>
        /// Setting dependency and commands
        /// For the sake of the project and little amount of time  I have skipped the validation of the model. This means that there is no sql-injection detection mechanism implemented.
        /// </summary>
        public LoginVM()
        {
            processor = DependencyService.Get<ILoginProcessor>();
            LogInCommand = new AsyncCommand(LogInAsync);
            RegisterCommand = new Command(GoToRegisterAsync);
            Task.Run(AutoLogin);
        }
        #region Commands
        public AsyncCommand LogInCommand { get; private set; }
        public Command RegisterCommand { get; private set; }
        #endregion
        #region Fields
        public string Username { get; set; }
        public string Password { get; set; }
        private bool _isLoging;
        public bool CheckBoxState { get; set; }
        public bool IsLoging
        {
            get => _isLoging;
            set
            {
                _isLoging= value;
                OnPropertyChanged(nameof(IsLoging));
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Try to login in async
        /// </summary>
        /// <returns></returns>
        private async Task LogInAsync()
        {
            if (IsLoging) return;
            IsLoging = true;
            App.Instance.ShowWaitPopUp();
            var result = await processor.Login(Username, Password);
            if (result)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage = new AppShell();
                });
            }
            else
            {
                App.Instance.DisplayToast("Failed to log in");
            }
            IsLoging = false;
            Preferences.Set("autologin", CheckBoxState);
            if (CheckBoxState)
            {
                Preferences.Set("username", Username);
                Preferences.Set("password", Password);
            }
            await PopupNavigation.Instance.PopAllAsync();
        }
        private async Task AutoLogin()
        {
            if (Preferences.Get("autologin", false))
            {
                App.Instance.ShowWaitPopUp();
                var server = DependencyService.Get<IServerProcessor>(DependencyFetchTarget.GlobalInstance);
                //while(server.HubConnectionState != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                //{
                //    await Task.Delay(TimeSpan.FromMilliseconds(20));
                //}
                var result = await processor.Login(Preferences.Get("username", string.Empty), Preferences.Get("password", string.Empty));
                if (result)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        App.Current.MainPage = new AppShell();
                    });
                }
                else
                {
                    App.Instance.DisplayToast("Autologin fehlgeschlagen");
                }
                await PopupNavigation.Instance.PopAllAsync();
            }
        }
        /// <summary>
        /// Go back to Register Page
        /// </summary>
        private async void GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(Register));
        }
        #endregion
    }
}
