using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Helpers.Exceptions;
using Xamarin.Forms;
using Quiz.Mobile.Shared.DTOs;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Xamarin.CommunityToolkit.Extensions;
using System.Net.Http;
using System.Linq;
using Quiz.Mobile.Helpers;
using Xamarin.Essentials;
using Quiz.Mobile.Views;

namespace Quiz.Mobile.ViewModels
{
	public class RegistrationViewModel : SingleItemViewModel<CreateUserDto>,
        IHasLabelViewModel
    {
		#region Pola prywatne
		private readonly IHttpClientService _client;
        private readonly Animation rotation;
        #endregion

        #region Konstruktor
        public RegistrationViewModel()
		{
            Item = new CreateUserDto();
			_client = DependencyService.Get<IHttpClientService>(
				DependencyFetchTarget.GlobalInstance);
            this.PropertyChanged += (_, __) =>
                SaveAndCloseCommand.RaiseCanExecuteChanged();
            this.PropertyChanged += OnIsBusyChanged;
            rotation = new Animation(v =>
                View.Label.Rotation = v, 0, 360, Easing.Linear);
        }
        #endregion

        #region Właściwości
        public IHasLabelView View { get; set; }

        public string FirstName
        {
            get => Item.FirstName;
            set
            {
                if (value != Item.FirstName)
                {
                    Item.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => Item.LastName;
            set
            {
                if (value != Item.LastName)
                {
                    Item.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => Item.Email;
            set
            {
                if (value != Item.Email)
                {
                    Item.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _IsEmailValid = false;
        public bool IsEmailValid
        {
            get => _IsEmailValid;
            set => SetProperty(ref _IsEmailValid, value);
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }

        private string _RepeatedPassword;
        public string RepeatedPassword
        {
            get => _RepeatedPassword;
            set => SetProperty(ref _RepeatedPassword, value);
        }

        private RoleDto _SelectedRole;
        public RoleDto SelectedRole
        {
            get => _SelectedRole;
            set => SetProperty(ref _SelectedRole, value);
        }

        private ObservableRangeCollection<RoleDto> _Roles;
        public ObservableRangeCollection<RoleDto> Roles
        {
            get
            {
                if (_Roles == null)
                    LoadRoles().SafeFireAndForget(ex => Console.WriteLine(ex));
                return _Roles;
            }
            set => SetProperty(ref _Roles, value);
        }

        //wykorzystanie właściwości do sprawdzania poprawności formularza
        //bo do propertisa można się bindować na widoku
        public bool CanSaveProp =>
            !string.IsNullOrEmpty(FirstName) &&
            FirstName?.Length < 50 &&
            !string.IsNullOrEmpty(LastName) &&
            LastName?.Length < 50 &&
            !string.IsNullOrEmpty(Password) &&
            Password?.Length < 50 &&
            SelectedRole != null &&
            _IsEmailValid &&
            _Password.Equals(_RepeatedPassword) &&
            IsNotBusy;

        #endregion

        #region Metody

        private async Task LoadRoles()
        {
            try
            {
                var roles = await _client.GetAllItems<RoleDto>();
                Roles = new ObservableRangeCollection<RoleDto>(roles);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?.MakeToast(
                    "Nie udało się pobrać ról użytkownika. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(2000);
                Item.PasswordHash = SecurePasswordHasher.Hash(_Password);
                Item.RoleId = _SelectedRole.Id;
                await _client.AddItem(Item);
                await Application.Current.MainPage.DisplayToastAsync(
                    "Poprawnie dodano użytkownika!");
                await Task.Delay(1000);
                await base.NavigateBack();
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Nie udało się dodać użytkownika. Odpowiedź serwera: {e.Message}");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Nie udało się dodać użytkownika. '{e.Message}'");
            }
            finally
            {
                IsBusy = false;
                ClearProperties();
            }
        }

        protected override bool CanSave(object arg) => CanSaveProp;

        private void ClearProperties()
        {
            foreach (var prop in this.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string)))
                    prop.SetValue(this, string.Empty);
            SelectedRole = null;
        }

        private void OnIsBusyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.IsBusy))
            {
                if (this.IsBusy)
                    rotation.Commit(View, "rotate", 16, 1000, Easing.Linear,
                        (v, c) => View.Label.Rotation = 0,
                        () => true);
                else
                    View.AbortAnimation("rotate");
            }
        }

        public override void Dispose()
        {
            this.PropertyChanged -= OnIsBusyChanged;
            base.Dispose();
        }

        #endregion
    }
}

