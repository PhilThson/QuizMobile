using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Quiz.Mobile.Services;
using Quiz.Mobile.Views;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Helpers;
using Xamarin.Essentials;
using System.Net.Http;
using Quiz.Mobile.ViewModels.Abstract;

namespace Quiz.Mobile
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            DependencyService.Register<IHttpClientService, HttpClientService>();
            //Druga koncepcja mediatora, ale z wykorzystaniem wstrzykiwania
            //zależności zamiast singletona
            //DependencyService.RegisterSingleton<IMediator, Mediator>();

            //nadpisanie motywu zgodnie z preferencjami
            TheTheme.SetTheme();

            // wyczyszczenie SecureStorage
            SecureStorage.RemoveAll();

            MainPage = new AppShell();
        }

        protected override void OnStart ()
        {
            OnResume();
        }

        protected override void OnSleep ()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume ()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
    }
}

