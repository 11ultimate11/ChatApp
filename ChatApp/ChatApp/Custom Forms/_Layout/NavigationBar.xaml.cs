using ChatApp.Services.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp.Custom_Forms._Layout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationBar : ContentView
    {
        public bool AllowBackNavigation { get; set; }
        public Command BackCommand { get; private set; }    
        public NavigationBar()
        {
            InitializeComponent();
            BindingContext = this;
            if(AppSettings.Person != null)
            {
                user.Text = AppSettings.Person.GetName();
                img.Source = AppSettings.Person.Avatar;
            }
            back.IsVisible = AllowBackNavigation;
            BackCommand = new Command(GoBack);
        }
        private async void GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}