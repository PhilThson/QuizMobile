using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Quiz.Mobile.Interfaces;
using Xamarin.Forms;
using Quiz.Mobile.CommunityToolkit;
using System.Net.Http;
using Quiz.Mobile.Views.Dictionary;
using Quiz.Mobile.Helpers;
using Xamarin.CommunityToolkit.Extensions;

namespace Quiz.Mobile.ViewModels
{
	public class DifficultiesViewModel : ItemsCollectionViewModel<DifficultyViewModel>
	{
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public DifficultiesViewModel()
		{
			base.Title = "Skale trudności";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            _mediator = Mediator.Instance;
            _mediator.RequestDictionaryListRefresh += OnRequestDictionaryListRefresh;
        }
        #endregion

        #region Właściwości

        private DifficultyViewModel _SelectedDifficulty;
        public DifficultyViewModel SelectedDifficulty
        {
            get => _SelectedDifficulty;
            set => SetProperty(ref _SelectedDifficulty, value);
        }

        #endregion

        #region Metody

        protected override async Task Add()
        {
            var route = $"{nameof(AddDictionaryPage)}" +
                $"?ItemType={QuizApiSettings.Difficulties}";
            await AppShell.Current.GoToAsync(route);
        }

        protected override async Task Load()
        {
            var difficulties = await _client.GetAllItems<DifficultyViewModel>();
            List = new ObservableRangeCollection<DifficultyViewModel>(difficulties);
        }

        protected override async Task Refresh()
        {
            IsBusy = true;
            try
            {
                await Task.Delay(1000);
                List.Clear();
                var difficulties = await _client.GetAllItems<DifficultyViewModel>();
                List.AddRange(difficulties);
                await Application.Current.MainPage.DisplayToastAsync("Odświeżono");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Nie udało się pobrać skal trudności. Odpowiedź serwera: {e.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task Remove(DifficultyViewModel obj)
        {
            try
            {
                var accept = await Application.Current.MainPage.DisplayAlert(
                    "Usuwanie", "Czy na pewno usunąć skalę trudności?",
                    "TAK", "Anuluj");
                if (!accept)
                    return;
                IsBusy = true;
                await _client.RemoveItemById<DifficultyViewModel>(obj.Id);
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

        protected override async Task Selected(DifficultyViewModel difficulty)
        {
            if (difficulty == null)
                return;

            var route = $"{nameof(AddDictionaryPage)}" +
                $"?ItemType={QuizApiSettings.Difficulties}&ItemId={difficulty.Id}";

            await AppShell.Current.GoToAsync(route);
        }


        private void OnRequestDictionaryListRefresh(string dictionaryType)
        {
            if (dictionaryType == QuizApiSettings.Difficulties)
                Refresh().SafeFireAndForget(e => Console.WriteLine(e.Message));
        }

        public override void Dispose()
        {
            _mediator.RequestDictionaryListRefresh -= OnRequestDictionaryListRefresh;
            base.Dispose();
        }

        #endregion
    }
}

