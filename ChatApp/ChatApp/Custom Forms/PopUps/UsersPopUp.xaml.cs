using ChatApp.Models;
using ChatApp.Services.Global;
using ChatApp.Services.Interfaces;
using MvvmHelpers;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp.Custom_Forms.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPopUp : PopupPage
    {
        private ObservableRangeCollection<PersonModel> personModels = new ObservableRangeCollection<PersonModel>();
        private bool _noMore;
        public UsersPopUp()
        {
            InitializeComponent();
            coll.ItemsSource = personModels;
            BindingContext = this;
            SelectCommand = new Command(Select);
            Task.Run(GetModels);
        }
        public Command SelectCommand { get; private set; }
        /// <summary>
        /// Normalerweise würde ich die Option wählen, Modelle asynchron aus der Datenbank zu laden, wenn sie benötigt werden und nicht alle gleichzeitig, wie ein Paginierungssystem.
        /// Aber wieder ist es nur ein Demo-Projekt
        /// </summary>
        /// <returns></returns>
        private async Task GetModels()
        {
            if (IsBusy || _noMore) return;
            else
            {
                IsBusy = true;
                var api = DependencyService.Get<IApiProcessor>();
                var result = await api.GetStringAsync("Account/GetAllAccount");
                if (!string.IsNullOrEmpty(result))
                {
                    var models = JsonConvert.DeserializeObject<List<PersonModel>>(result);
                    if (models != null && models.Any())
                    {
                        personModels.AddRange(models);
                    }
                }
            }
        }
        private async void Select(object obj)
        {
            if (obj is PersonModel person)
            {
                InternalDelegates.CallOnUserPick(person);
                System.Console.WriteLine($"Selected {person.Nachname}");
                await PopupNavigation.Instance.PopAllAsync();
            }
        }
        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
            catch (System.Exception) { }
        }
    }
}