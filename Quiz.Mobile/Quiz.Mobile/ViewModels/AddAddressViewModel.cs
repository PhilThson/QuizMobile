using System;
using System.Net.Http;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.Helpers.Exceptions;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Models;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Views.Address;
using Quiz.Mobile.Views.Employee;
using Xamarin.Forms;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(AddressId), nameof(AddressId))]
    [QueryProperty(nameof(IsForEmployee), nameof(IsForEmployee))]
    public class AddAddressViewModel : SingleItemViewModel<AddressDto>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public AddAddressViewModel()
        {
            _mediator = Mediator.Instance;
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);

            Item = new AddressDto();
            base.Title = "Dodawanie adresu";
            this.PropertyChanged += (_, __) =>
                SaveAndCloseCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Właściwości

        public string Country
        {
            get => Item.Country;
            set
            {
                if (value != Item.Country)
                {
                    Item.Country = value;
                    OnPropertyChanged();
                }
            }
        }
        public string City
        {
            get => Item.City;
            set
            {
                if (value != Item.City)
                {
                    Item.City = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Street
        {
            get => Item.Street;
            set
            {
                if (value != Item.Street)
                {
                    Item.Street = value;
                    OnPropertyChanged();
                }
            }
        }
        public string HouseNumber
        {
            get => Item.HouseNumber;
            set
            {
                if (value != Item.HouseNumber)
                {
                    Item.HouseNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FlatNumber
        {
            get => Item.FlatNumber;
            set
            {
                if (value != Item.FlatNumber)
                {
                    Item.FlatNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PostalCode
        {
            get => Item.PostalCode;
            set
            {
                if (value != Item.PostalCode)
                {
                    Item.PostalCode = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _IsPostalCodeValid = false;
        public bool IsPostalCodeValid
        {
            get => _IsPostalCodeValid;
            set => SetProperty(ref _IsPostalCodeValid, value);
        }

        public bool CanSaveProp => CanSave(null);

        #endregion

        #region Właściwości nawigacyjne

        private int _AddressId;
        public int AddressId
        {
            get => _AddressId;
            set
            {
                if (value != _AddressId)
                {
                    _AddressId = value;
                    base.Title = "Edycja adresu";
                    LoadAddress().SafeFireAndForget(ex =>
                        Console.WriteLine(ex.Message));
                }
            }
        }

        private byte _IsForEmployee;
        private bool isPostalCodeValid;

        public byte IsForEmployee
        {
            get => _IsForEmployee;
            set
            {
                if (value != _IsForEmployee)
                {
                    _IsForEmployee = value;
                    base.Title = "Dodawanie adresu pracownika";
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Metody

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;
                object createdAddressId = null;
                if (Item.Id == default)
                    createdAddressId = await _client.AddItem(Item);
                else
                    await _client.UpdateItem(Item);

                DependencyService.Get<IToast>()?.MakeToast("Zapisano adres!");
                await Task.Delay(2000);

                if (IsForEmployee == 1)
                {
                    await Shell.Current.GoToAsync($"..?CreatedAddressId={createdAddressId}");
                    return;
                }

                _mediator.RaiseRequestAddressesRefresh();
                await Shell.Current.GoToAsync($"//{nameof(AddressesPage)}");
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayAlert(Title,
                    $"Niepowodzenie. Odpowiedź serwera: {e.Message}", "OK");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Title, e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadAddress()
        {
            try
            {
                IsBusy = true;
                Item = await _client.GetItemById<AddressDto>(_AddressId);
                foreach (var prop in Item.GetType().GetProperties())
                    OnPropertyChanged(prop.Name);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Edycja",
                    $"Nie udało się załadować adresu. '{e.Message}'",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override bool CanSave(object arg) =>
            !string.IsNullOrEmpty(Item.Country) &&
            !string.IsNullOrEmpty(Item.City) &&
            !string.IsNullOrEmpty(Item.HouseNumber) &&
            _IsPostalCodeValid &&
            (Item.Country?.Length <= 64) &&
            (Item.City?.Length <= 128) &&
            (Item.HouseNumber?.Length <= 10) &&
            ((string.IsNullOrEmpty(Item.Street)) ? true :
                (Item.Street?.Length <= 128)) &&
            ((string.IsNullOrEmpty(Item.FlatNumber)) ? true :
                (Item.FlatNumber?.Length <= 10)) &&
            IsNotBusy
            ;

        #endregion
    }
}

