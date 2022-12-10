using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Quiz.Mobile.Services;
using Quiz.Mobile.Views;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Helpers;
using Xamarin.Essentials;

namespace Quiz.Mobile
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton<IQuizApiSettings>(new QuizApiSettings());
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IEmployeeService, EmployeeService>();

            //nadpisanie motywu zgodnie z preferencjami
            TheTheme.SetTheme();

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

