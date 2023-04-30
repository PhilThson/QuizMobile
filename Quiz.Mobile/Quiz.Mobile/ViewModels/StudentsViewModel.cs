using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.CommunityToolkit.Commands;
using Xamarin.Forms;
using System.Threading.Tasks;
using Quiz.Mobile.Views.Student;
using Xamarin.CommunityToolkit.Extensions;
using Command = Quiz.Mobile.CommunityToolkit.Commands.Command;
using System.Linq;
using System.Windows.Input;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using Xamarin.Essentials;

namespace Quiz.Mobile.ViewModels
{
    public class StudentsViewModel : ItemsCollectionViewModel<StudentViewModel>,
        IHasCollectionViewModel
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public StudentsViewModel()
        {
            base.Title = "Wszyscy uczniowie";
            FavoriteCommand = new AsyncCommand<object>(Favorite);
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            _mediator = Mediator.Instance;
            _mediator.RequestStudentsRefresh +=
                () => Refresh().SafeFireAndForget(ex =>
                        Console.WriteLine(ex.Message));
        }
        #endregion

        #region Właściwości i komendy
        public IHasCollectionView View { get; set; }

        private StudentViewModel _SelectedStudent;
        public StudentViewModel SelectedStudent
        {
            get => _SelectedStudent;
            set => SetProperty(ref _SelectedStudent, value);
        }

        public AsyncCommand<object> FavoriteCommand { get; }

        private string _SearchText;
        public string SearchText
        {
            get => _SearchText;
            set => SetProperty(ref _SearchText, value);
        }

        private ICommand _MoveToStartCommand;
        public ICommand MoveToStartCommand =>
            _MoveToStartCommand ??=
                new Command(() => View.CollectionView.ScrollTo(0));

        private IAsyncCommand _LoadMoreCommand;
        public IAsyncCommand LoadMoreCommand =>
            _LoadMoreCommand ??=
                new AsyncCommand(LoadMore);

        #endregion

        #region Metody

        protected async override Task Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(500);
                List.Clear();
                AllList = await _client.GetAllItems<StudentViewModel>();
                List.AddRange(AllList.Take(10));
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync("Odświeżono");
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Nie udało się pobrać uczniów. Odpowiedź serwera: {e.Message}",
                    5000);
            }
        }

        protected override async Task Selected(StudentViewModel student)
        {
            if (student == null)
                return;

            var route = $"{nameof(StudentDetailsPage)}?StudentId={student.Id}";
            await Shell.Current.GoToAsync(route);
        }

        protected override async Task Add()
        {
            var route = nameof(AddStudentPage);
            await AppShell.Current.GoToAsync(route);
        }

        protected override async Task Remove(StudentViewModel student)
        {
            var accept = await Application.Current.MainPage.DisplayAlert(
                "Usuwanie", "Czy na pewno usunąć ucznia?", "TAK", "Anuluj");
            if (!accept)
                return;

            await _client.RemoveItemById<StudentViewModel>(student.Id);
            await Refresh();
            await Task.Delay(1000);
            DependencyService.Get<IToast>()?.MakeToast("Usunięto!");
        }

        protected override async Task Load()
        {
            AllList = await _client.GetAllItems<StudentViewModel>();
            List = new CommunityToolkit.ObservableRangeCollection<StudentViewModel>(AllList.Take(10));
        }

        protected async Task Favorite(object obj)
        {
            if (obj == null)
                return;
            var student = obj as StudentViewModel;
            await Application.Current.MainPage.DisplayAlert(
                "Ulubiony", student.FirstName, "OK");
        }

        protected override void Filter(string value)
        {
            List.Clear();
            if (string.IsNullOrEmpty(value))
            {
                List.AddRange(AllList);
                return;
            }

            List.AddRange(AllList
                .Where(s =>
                ($"{s.FirstName} {s.LastName}"
                    .Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                ($"{s.LastName} {s.FirstName}"
                    .Contains(value, StringComparison.InvariantCultureIgnoreCase))
                ));
        }

        private async Task LoadMore()
        {
            IsLoading = true;
            await Task.Delay(500);
            if (List.Count >= AllList.Count())
            {
                IsLoading = false;
                return;
            }

            List.AddRange(AllList.Skip(List.Count).Take(10));
            IsLoading = false;
        }

        public bool IsLoading { get; set; } = false;

        #endregion
    }
}

