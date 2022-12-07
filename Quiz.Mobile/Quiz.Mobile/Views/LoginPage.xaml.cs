using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quiz.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        //jeśli już użytkownik jest już zalogowany
        //to odrazu odeślij go do strony CoffeeEquipmentPage
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            /*var loggedIn = true;
            if(loggedIn)
            {
                await Shell.Current.GoToAsync($"//{nameof(CoffeeEquipmentPage)}");
            }*/
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // dzięki relative path, wciśnięcie klawisza wstecz
            // powoduje wyjście z aplikacji
            //await Shell.Current.GoToAsync($"//{nameof(CoffeeEquipmentPage)}");
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //można określić że panel logowania, zawsze będzie poprzedzającym panel rejestracji
            //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}/{nameof(RegistrationPage)}");
            //albo poprostu określić ścieżkę
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }
    }
}
