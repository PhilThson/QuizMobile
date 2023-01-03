using Quiz.Mobile.Views;
using Quiz.Mobile.Views.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Quiz.Mobile.ViewModels.Abstract;

namespace Quiz.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}

