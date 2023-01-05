using ChatApp.Services;
using ChatApp.Services.Interfaces;
using ChatApp.Services.Processor;
using ChatApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.CommunityToolkit.Extensions;
using ChatApp.Custom_Forms.PopUps;
using Rg.Plugins.Popup.Enums;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

namespace ChatApp
{
    public partial class App : Application
    {
        public static App Instance { get; private set; }
        public App()
        {
            Instance = this;
            InitializeComponent();
            DependencyService.Register<IApiProcessor , ApiProcessor>();
            DependencyService.Register<ILoginProcessor , LoginProcessor>();
            DependencyService.Register<IUsersProcessor , UsersProcessor>();
            DependencyService.Register<IServerProcessor , ServerProcessor>();
            MainPage = new AppShell();
            GoToLogIn();
            Task.Run(() => DependencyService.Get<IServerProcessor>(DependencyFetchTarget.GlobalInstance).InitHubConnection());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public async void ShowUsersPopUp()
        {
            var popup = new UsersPopUp()
            {
                BackgroundColor = Color.Transparent,
                CloseWhenBackgroundIsClicked = true,
                Animation = new ScaleAnimation()
                {
                    PositionIn = MoveAnimationOptions.Top,
                    PositionOut = MoveAnimationOptions.Top
                }
            };
            await PopupNavigation.Instance.PushAsync(popup);
        }
        public async void ShowWaitPopUp()
        {
            var popup = new WaitPopUp()
            {
                BackgroundInputTransparent = true,
                CloseWhenBackgroundIsClicked = false,
                Animation = new ScaleAnimation()
                {
                    PositionIn = MoveAnimationOptions.Center,
                    PositionOut = MoveAnimationOptions.Center
                }
            };
            await PopupNavigation.Instance.PushAsync(popup);
        }
        async void GoToLogIn()
        {
            await Shell.Current.GoToAsync(nameof(Login));
        }
        public async void DisplayToast(string message)
        {
            ToastOptions toastOptions = new ToastOptions()
            {
                BackgroundColor= Color.White,
                MessageOptions = new MessageOptions()
                {
                    Message = message,
                    Foreground = Color.DodgerBlue
                }
            };
            await App.Current.MainPage.DisplayToastAsync(toastOptions);
        }
    }
}
