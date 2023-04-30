using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Quiz.Mobile.Interfaces;
using Xamarin.Forms;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.Views.Dictionary;
using Quiz.Mobile.Helpers;
using Xamarin.CommunityToolkit.Extensions;
using Quiz.Mobile.Views.Student;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.ViewModels
{
    public class AreasViewModel : ItemsCollectionViewModel<AreaViewModel>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public AreasViewModel()
        {
            base.Title = "Obszary zestawu pytań";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            _mediator = Mediator.Instance;

            _mediator.RequestDictionaryListRefresh += (dictionaryType) =>
                OnRequestDictionaryListRefresh(dictionaryType);
        }
        #endregion

        #region Właściwości
        private AreaViewModel _SelectedArea;
        public AreaViewModel SelectedArea
        {
            get => _SelectedArea;
            set => SetProperty(ref _SelectedArea, value);
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
                await Application.Current.MainPage.DisplayToastAsync("Odświeżono");
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Obszary",
                    "Nie udało się pobrać obszarów zestawu pytań. " +
                    $"Odpowiedź serwera: {e.Message}", "OK");
            }
        }

        protected override async Task Remove(AreaViewModel obj)
        {
            try
            {
                var accept = await Application.Current.MainPage.DisplayAlert(
                    "Usuwanie", "Czy na pewno usunąć obszar zestawu pytań?",
                    "TAK", "Anuluj");
                if (!accept)
                    return;
                IsBusy = true;
                await _client.RemoveItemById<AreaViewModel>(obj.Id);
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync("Usunięto");
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Błąd",
                    $"Nie udało się usunąć rekordu. '{e.Message}'", "OK");
            }
        }

        protected override async Task Selected(AreaViewModel area)
        {
            if (area == null)
                return;

            var route = $"{nameof(AddDictionaryPage)}" +
                $"?ItemType={QuizApiSettings.Areas}&ItemId={area.Id}";

            await AppShell.Current.GoToAsync(route);
        }

        private void OnRequestDictionaryListRefresh(string dictionaryType)
        {
            if (dictionaryType == QuizApiSettings.Areas)
                Refresh().SafeFireAndForget(e => Console.WriteLine(e.Message));
        }

        #endregion
    }
}

