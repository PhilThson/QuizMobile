using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.CommunityToolkit.Commands;
using Xamarin.Forms;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.Views.Student;
using Xamarin.CommunityToolkit.Extensions;

namespace Quiz.Mobile.ViewModels
{
    public class StudentsViewModel : ItemsCollectionViewModel<StudentViewModel>
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

        private StudentViewModel _SelectedStudent;
        public StudentViewModel SelectedStudent
        {
            get => _SelectedStudent;
            set => SetProperty(ref _SelectedStudent, value);
        }

        public AsyncCommand<object> FavoriteCommand { get; }

        #endregion

        #region Metody

        protected async override Task Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                List.Clear();
                var students = await _client.GetAllItems<StudentViewModel>();
                List.AddRange(students);
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
            await _client.RemoveItemById<StudentViewModel>(student.Id);
            await Refresh();
            await Task.Delay(1000);
            DependencyService.Get<IToast>()?.MakeToast("Usunięto!");
        }

        protected override async Task Load()
        {
            var students = await _client.GetAllItems<StudentViewModel>();
            List = new ObservableRangeCollection<StudentViewModel>(students);
        }

        protected async Task Favorite(object obj)
        {
            if (obj == null)
                return;
            var student = obj as StudentViewModel;
            await Application.Current.MainPage.DisplayAlert(
                "Ulubiony", student.FirstName, "OK");
        }

        #endregion
    }
}

