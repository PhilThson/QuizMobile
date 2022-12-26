using System;
using System.Collections.Generic;
using Quiz.Mobile.ViewModels;
using Quiz.Mobile.Views;
using Quiz.Mobile.Views.Employee;
using Quiz.Mobile.Views.Student;
using Xamarin.Forms;

namespace Quiz.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(EmployeeDetailsPage), typeof(EmployeeDetailsPage));
            Routing.RegisterRoute(nameof(AddEmployeePage), typeof(AddEmployeePage));
            Routing.RegisterRoute(nameof(StudentDetailsPage), typeof(StudentDetailsPage));
            Routing.RegisterRoute(nameof(AddStudentPage), typeof(AddStudentPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

