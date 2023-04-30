using System;
using System.Linq;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Views.Address;
using Quiz.Mobile.Views.Dictionary;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Quiz.Mobile.ViewModels
{
	public class AddressesViewModel : ItemsCollectionViewModel<AddressDto>
	{
		#region Pola prywatne
		private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public AddressesViewModel()
        {
            _mediator = Mediator.Instance;
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);

            base.Title = "Wszystkie adresy";
            _mediator.RequestAddressesRefresh += OnRequestAddressesRefresh;
        }
        #endregion

        #region Właściwości
        private AddressDto _SelectedAddress;
        public AddressDto SelectedAddress
        {
            get => _SelectedAddress;
            set => SetProperty(ref _SelectedAddress, value);
        }

        private string _FilterText;
        public string FilterText
        {
            get => _FilterText;
            set
            {
                SetProperty(ref _FilterText, value);
                Filter(_FilterText);
            }
        }
        #endregion

        #region Metody

        protected override async Task Add()
        {
            var route = $"{nameof(AddAddressPage)}";
            await AppShell.Current.GoToAsync(route);
        }

        protected override async Task Load()
        {
            AllList = await _client.GetAllItems<AddressDto>();
            List = new ObservableRangeCollection<AddressDto>(AllList);
        }

        protected override async Task Refresh()
        {
            IsBusy = true;
            try
            {
                await Task.Delay(1000);
                List.Clear();
                AllList = await _client.GetAllItems<AddressDto>();
                List.AddRange(AllList);
                await Application.Current.MainPage.DisplayToastAsync("Odświeżono");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Nie udało się pobrać adresów. Odpowiedź serwera: {e.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task Remove(AddressDto address)
        {
            var accept = await Application.Current.MainPage.DisplayAlert(
                "Usuwanie", "Czy na pewno usunąć wybrany adres?", "TAK", "Anuluj");
            if (!accept)
                return;

            await _client.RemoveItemById<AddressDto>(address.Id);
            await Refresh();
            await Task.Delay(1000);
            DependencyService.Get<IToast>()?.MakeToast("Usunięto!");
        }

        protected override async Task Selected(AddressDto address)
        {
            if (address == null)
                return;

            var route = $"{nameof(AddressDetailsPage)}?AddressId={address.Id}";
            await AppShell.Current.GoToAsync(route);
        }

        protected override void Filter(string filter)
        {
            List.Clear();
            if(string.IsNullOrEmpty(filter))
            {
                List.AddRange(AllList);
                return;
            }

            List.AddRange(AllList
                .Where(a =>
                ($"{a.Country} {a.City} {a.Street}"
                    ?.Contains(filter,
                    StringComparison.InvariantCultureIgnoreCase) ?? false)
                ));
        }

        public override void Dispose()
        {
            _mediator.RequestAddressesRefresh -= OnRequestAddressesRefresh;
            base.Dispose();
        }

        #endregion

        #region Metody prywatne
        private void OnRequestAddressesRefresh()
        {
            Refresh().SafeFireAndForget(ex => Console.WriteLine(ex.Message));
        }
        #endregion
    }
}

