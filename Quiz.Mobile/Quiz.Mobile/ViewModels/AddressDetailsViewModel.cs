using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Views.Address;
using Quiz.Mobile.Views.Student;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(AddressId), nameof(AddressId))]
    public class AddressDetailsViewModel : SingleItemViewModel<AddressDto>
	{
        #region Pola prywatne
        private readonly IHttpClientService _client;
        #endregion

        #region Konstruktor
        public AddressDetailsViewModel()
		{
            Item = new AddressDto();
            Title = "Szczegóły adresu";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
        }
        #endregion

        #region Właściwości
        private int _AddressId;
        public int AddressId
        {
            get => _AddressId;
            set
            {
                SetProperty(ref _AddressId, value);
                LoadAddress().SafeFireAndForget(ex =>
                    Console.WriteLine(ex.Message));
            }
        }
        #endregion

        #region Metody

        protected override async Task SaveAndClose()
        {
            var route = $"{nameof(AddAddressPage)}?AddressId={AddressId}";
            await AppShell.Current.GoToAsync(route);
        }

        private async Task LoadAddress()
        {
            try
            {
                IsBusy = true;
                Item = await _client.GetItemById<AddressDto>(_AddressId);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    "Nie udało się pobrać adresu. " +
                    $"Odpowiedź serwera: {e.Message}", 5000);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override bool CanSave(object arg) => true;

        #endregion
    }
}

