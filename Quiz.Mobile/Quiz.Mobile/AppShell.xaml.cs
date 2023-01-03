using System;
using System.Collections.Generic;
using Quiz.Mobile.ViewModels;
using Quiz.Mobile.Views;
using Quiz.Mobile.Views.Dictionary;
using Quiz.Mobile.Views.Employee;
using Quiz.Mobile.Views.Student;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Quiz.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EmployeeDetailsPage), typeof(EmployeeDetailsPage));
            Routing.RegisterRoute(nameof(AddEmployeePage), typeof(AddEmployeePage));
            Routing.RegisterRoute(nameof(StudentDetailsPage), typeof(StudentDetailsPage));
            Routing.RegisterRoute(nameof(AddStudentPage), typeof(AddStudentPage));
            Routing.RegisterRoute(nameof(AddDictionaryPage), typeof(AddDictionaryPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async void OnVersionClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Wersja", VersionTracking.CurrentVersion, "OK");
        }
    }
}

