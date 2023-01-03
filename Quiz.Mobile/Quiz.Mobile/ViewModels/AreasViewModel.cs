using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Quiz.Mobile.Interfaces;
using Xamarin.Forms;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.Views.Dictionary;
using Quiz.Mobile.Helpers;

namespace Quiz.Mobile.ViewModels
{
    public class AreasViewModel : ItemsCollectionViewModel<AreaViewModel>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        #endregion

        #region Konstruktor
        public AreasViewModel()
        {
            base.Title = "Obszary zestawu pytań";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
        }
        #endregion

        #region Metody

        protected override async Task Add()
        {
            var route = $"{nameof(AddDictionaryPage)}" +
                $"?ItemType={QuizApiSettings.Areas}";
            await AppShell.Current.GoToAsync(route);
        }

        protected override async Task Load()
        {
            var areas = await _client.GetAllItems<AreaViewModel>();
            List = new ObservableRangeCollection<AreaViewModel>(areas);
        }

        protected override async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                List.Clear();
                var areas = await _client.GetAllItems<AreaViewModel>();
                List.AddRange(areas);
                IsBusy = false;
                DependencyService.Get<IToast>()?.MakeToast("Odświeżono");
            }
            catch (Exception e)
            {
                IsBusy = false;
                DependencyService.Get<IToast>()?.MakeToast(
                    "Nie udało się pobrać obszarów zestawu pytań. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        protected override async Task Remove(AreaViewModel obj)
        {
            try
            {
                IsBusy = true;
                await _client.RemoveItemById<AreaViewModel>(obj.Id);
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Usunięto", "", "OK");
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Błąd",
                    $"Nie udało się usunąć rekordu. '{e.Message}'", "OK");
            }
        }

        protected override Task Selected(AreaViewModel obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

