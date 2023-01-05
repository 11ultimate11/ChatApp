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
	public partial class SecondaryNavigationBar : ContentView
	{
		public SecondaryNavigationBar ()
		{
			InitializeComponent ();
			if(AppSettings.ChatModel != null && AppSettings.ChatModel.Target != null)
			{
                user.Text = AppSettings.ChatModel.Target.Name;
                img.Source = AppSettings.ChatModel.Target.Avatar;
            }
		}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			await Shell.Current.GoToAsync("..");
        }
    }
}