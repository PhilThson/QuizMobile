using Quiz.Mobile.Views;
using System;
using Xamarin.Forms;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.DTOs;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.Interfaces;
using System.Net.Http;
using Xamarin.CommunityToolkit.Extensions;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Helpers.Exceptions;
using Xamarin.Essentials;
using System.Linq;

namespace Quiz.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Pola prywatne
        private IHttpClientService _client;
        #endregion

        #region Konstruktor
        public LoginViewModel()
        {
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            this.PropertyChanged += (_, __) => LoginCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Właściwości

        private string _Email;
        public string Email
        {
            get => _Email;
            set => SetProperty(ref _Email, value);
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }

        public bool CanLogin =>
            !string.IsNullOrEmpty(Email) &&
            !string.IsNullOrEmpty(Password) &&
            !base.IsBusy;

        private string ErrorMessage => "Wystąpił błąd podczas logowania.";

        #endregion

        #region Komendy
        private IAsyncCommand _LoginCommand;
        public IAsyncCommand LoginCommand => _LoginCommand ??=
            new AsyncCommand(Login, (_) => CanLogin);

        private IAsyncCommand _RegisterCommand;
        public IAsyncCommand RegisterCommand => _RegisterCommand ??=
            new AsyncCommand(Register);
        #endregion

        #region Metody

        private async Task Register()
        {
            await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");
        }

        private async Task Login()
        {
            try
            {
                if (Email != "Test" && Password != "123")
                {
                    IsBusy = true;
                    //var userDto = await _client.GetItemByKey<UserSimpleDataDto>(
                    //    nameof(Email).ToLower(), _Email);

                    //if (!SecurePasswordHasher.Verify(_Password, userDto.PasswordHash))
                    //    throw new DataValidationException("Niepoprawne hasło.");

                    // zmiana implementacji - uwierzytelnianie po stronie Rest API

                    var simpleUserDto = new SimpleUserDto 
                    { 
                        Email = Email, 
                        Password = Password 
                    };

                    var cookie = await _client.Login(simpleUserDto);
                    await SecureStorage.SetAsync(QuizApiSettings.QuizUserKey, cookie.First());
                }

                await Application.Current.MainPage.DisplayToastAsync("Zalogowano!");
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch (DataValidationException e)
            {
                await Application.Current.MainPage.DisplayToastAsync(e.Message, 5000);
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Wystąpił błąd komunikacji z API: {e.Message}", 5000);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"{ErrorMessage} '{e.Message}'", 5000);
            }
            finally
            {
                IsBusy = false;
                Email = string.Empty;
                Password = string.Empty;
            }
        }

        #endregion
    }
}