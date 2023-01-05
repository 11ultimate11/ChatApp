using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatView : ContentPage
	{
		public ChatView ()
		{
			InitializeComponent ();
            BindingContext = new ChatVM(coll);
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
			((ChatVM)BindingContext).OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((ChatVM)BindingContext).OnDisappearing();
        }
    }
}