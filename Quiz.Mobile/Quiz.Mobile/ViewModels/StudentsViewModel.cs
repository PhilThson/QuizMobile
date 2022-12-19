using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.CommunityToolkit.Commands;
using Xamarin.Forms;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;


namespace Quiz.Mobile.ViewModels
{
    public class StudentsViewModel : ItemsCollectionViewModel<StudentViewModel>
    {
        private readonly IStudentService _studentService;

        public StudentsViewModel()
        {
            base.Title = "Wszyscy uczniowie";
            SelectedCommand = new AsyncCommand<object>(Selected);
            FavoriteCommand = new AsyncCommand<StudentViewModel>(Favorite);
            _studentService = DependencyService.Get<IStudentService>(DependencyFetchTarget.GlobalInstance);
        }

        protected override async Task Add()
        {
            //var route = nameof(AddEmployeePage);
            //await AppShell.Current.GoToAsync(route);
        }

        private async Task Remove(StudentViewModel student)
        {
            //teraz można odwołać się do metody z zarejestrowanego serwisu
            await _studentService.RemoveStudent(student.Id);
            await Refresh();
        }

        protected async override Task Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                List.Clear();
                var students = await _studentService.GetAllStudents();
                List.AddRange(students);
                IsBusy = false;
                DependencyService.Get<IToast>()?.MakeToast("Odświeżono");
            }
            catch (Exception e)
            {
                IsBusy = false;
                DependencyService.Get<IToast>()?.MakeToast(
                    $"Nie udało się pobrać uczniów. Odpowiedź serwera: {e.Message}");
            }
        }

        private StudentViewModel _SelectedStudent;
        public StudentViewModel SelectedStudent
        {
            get => _SelectedStudent;
            set => SetProperty(ref _SelectedStudent, value);
        }

        public AsyncCommand<object> SelectedCommand { get; }

        async Task Selected(object args)
        {
            //przesłanie argumentu dzięki EventToCommand 
            //oraz ItemSelectedEventArgsConverter
            var student = args as StudentViewModel;
            if (student == null)
                return;

            //var route = $"{nameof(StudentDetailsPage)}?StudentId={student.Id}";
            //await Shell.Current.GoToAsync(route);

            SelectedStudent = null;
        }

        protected override Task Remove(object id)
        {
            throw new NotImplementedException();
        }

        protected override async Task Load()
        {
            var students = await _studentService.GetAllStudents();
            List = new ObservableRangeCollection<StudentViewModel>(students);
        }

        public AsyncCommand<StudentViewModel> FavoriteCommand { get; }

        async Task Favorite(StudentViewModel student)
        {
            if (student == null)
                return;

            await Application.Current.MainPage.DisplayAlert(
                "Favorite", student.FirstName, "OK");
            //await AppShell.Current.GoToAsync(nameof(AddStudentPage));
        }
    }
}

