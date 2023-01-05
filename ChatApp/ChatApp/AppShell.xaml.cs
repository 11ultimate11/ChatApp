using ChatApp.ViewModels;
using ChatApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ChatApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainView), typeof(MainView));
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));
            Routing.RegisterRoute(nameof(Login), typeof(Login));
            Routing.RegisterRoute(nameof(Register), typeof(Register));
            Routing.RegisterRoute(nameof(ChatView), typeof(ChatView));
        }

    }
}
