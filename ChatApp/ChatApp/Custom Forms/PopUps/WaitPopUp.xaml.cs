using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp.Custom_Forms.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WaitPopUp : PopupPage
	{
		public WaitPopUp ()
		{
			InitializeComponent ();
		}
	}
}