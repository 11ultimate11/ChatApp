using ChatApp.Models;
using ChatApp.Services.Global;
using ChatApp.Services.Interfaces;
using ChatApp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace ChatApp.ViewModels
{
    /// <summary>
    /// This class is the viewmodel of the Register Page.
    /// For the sake of the project and little amount of time  I have skipped the validation of the model. This means that there is no sql-injection detection mechanism implemented.
    /// </summary>
    public class RegisterVM : BaseViewModel
    {
        private readonly ILoginProcessor login;
        /// <summary>
        /// Setting dependency iniatialze commands
        /// </summary>
        public RegisterVM()
        {
            login = DependencyService.Get<ILoginProcessor>();
            GoBackCommand = new Command(GoBackAsync);
            RegisterCommand = new AsyncCommand(RegisterAsync);
            SelectAvatarCommand = new Command(SelectAvatarAsync);
            foreach (var item in Enum.GetValues(typeof(Genders)))
            {
                Genders.Add(((Genders)item).ToString());
            }
        }
        #region Commands
        public Command GoBackCommand { get; private set; }
        public Command SelectAvatarCommand { get; private set; }
        public AsyncCommand RegisterCommand { get; private set; }
        #endregion
        #region Fields
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        private string _icon = "user";
        public new string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
                OnPropertyChanged(nameof(TextIconColor));
                OnPropertyChanged(nameof(IconSelect));
            }
        }
        public bool IconSelect => _icon != null && _icon != "user";
        public Color TextIconColor => _icon != null && _icon != "user" ? Color.Green: Color.Red;
        public DateTime Geburtsdatum { get; set; } = DateTime.Now.AddYears(-18);
        public List<string> Genders { get; set; } = new List<string>();
        public int GenderID { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Try to Register a new person async .
        /// </summary>
        /// <returns></returns>
        private async Task RegisterAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            App.Instance.ShowWaitPopUp();
            PersonModel model = new PersonModel()
            {
                Credentials = new CredentialsModel()
                {
                    Username = Username,
                    Password = Password
                },
                Nachname = Nachname,
                Vorname = Vorname,
                CreatedDate= Geburtsdatum,
                Gender = (Genders)GenderID,
                Media = (!string.IsNullOrEmpty(Icon) && Icon != "user") ? AppSettings.GetMediaModelFromPath(Icon) : null,
            };
            var result = await login.RegisterAsync(model);
            if (result)
            {
                App.Current.MainPage = new AppShell();
            }
            else
            {
                App.Instance.DisplayToast("Failed to register please try again\nPossibly username already taken");
            }

            await PopupNavigation.Instance.PopAllAsync();
            IsBusy= false;
        }
        /// <summary>
        /// Pick a new avatar
        /// </summary>
        private async void SelectAvatarAsync()
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result !=null)
            {
                Icon = result.FullPath;
            }
        }
        /// <summary>
        /// Return to Login Page
        /// </summary>
        private async void GoBackAsync()
        {
            await Shell.Current.GoToAsync(nameof(Login));
        }
        #endregion
    }
}
