using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Views.Employee;
using Quiz.Mobile.Shared.ViewModels;
using System.Net.Http;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using System.Linq;
using System.Windows.Input;

namespace Quiz.Mobile.ViewModels
{
    public class EmployeesViewModel : ItemsCollectionViewModel<EmployeeViewModel>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private IAsyncCommand<object> _SelectedEmployeeCommand;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public EmployeesViewModel()
        {
            base.Title = "Wszyscy pracownicy";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            _mediator = Mediator.Instance;
            _mediator.RequestEmployeesRefresh += OnRequestEmployeesRefresh;
        }
        #endregion

        #region Komendy
        //Osobna komenda dla kolekcji typu ListView ze względu na przyjmowany typ
        //object - wynika to z podwójnego wywołania komendy, pierwszy raz z parametrem
        //EmployeeViewModel, a drugi z eventem ItemSelectedChanged (bug Xamarina)
        public IAsyncCommand<object> SelectedEmployeeCommand =>
            _SelectedEmployeeCommand ??= new AsyncCommand<object>(EmployeeSelected);

        #endregion

        #region Właściwości
        private EmployeeViewModel _SelectedEmployee;
        public EmployeeViewModel SelectedEmployee
        {
            get => _SelectedEmployee;
            set => SetProperty(ref _SelectedEmployee, value);
        }

        //Znacznie prostsza wersja filtrowania
        private string _FilterText;
        public string FilterText
        {
            get => _FilterText;
            set
            {
                if(value != _FilterText)
                {
                    _FilterText = value;
                    Filter(_FilterText);
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Metody
        protected override async Task Add()
        {
            var route = nameof(AddEmployeePage);
            await AppShell.Current.GoToAsync(route);
        }

        protected async override Task Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                List.Clear();
                AllList = await _client.GetAllItems<EmployeeViewModel>();
                List.AddRange(AllList);
                IsBusy = false;
                DependencyService.Get<IToast>()?.MakeToast("Odświeżono");
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Nie udało się pobrać pracowników. Odpowiedź serwera: {e.Message}",
                    5000);
            }
        }

        //Niewykorzystywane w przypadku ListView -
        //dla pracowników jest komenda SelectedEmployeeCommand
        //oraz obsługa w metodzie EmployeeSelected
        protected override async Task Selected(EmployeeViewModel employee)
        {
            if (employee == null)
                return;

            var route = $"//{nameof(EmployeeDetailsPage)}?EmployeeId={employee.Id}";
            await Shell.Current.GoToAsync(route);

            SelectedEmployee = null;
        }

        //Usuwanie w szczegółach pracownika
        protected override async Task Remove(EmployeeViewModel employee)
        {
            throw new NotImplementedException();
        }

        protected override async Task Load()
        {
            AllList = await _client.GetAllItems<EmployeeViewModel>();
            List = new ObservableRangeCollection<EmployeeViewModel>(AllList);
        }

        private async Task EmployeeSelected(object obj)
        {
            var employee = obj as EmployeeViewModel;
            if (employee == null)
                return;
            var route = $"{nameof(EmployeeDetailsPage)}?EmployeeId={employee.Id}";
            await Shell.Current.GoToAsync(route);

            SelectedEmployee = null;
        }

        protected override void Filter(string filter)
        {
            List.Clear();
            if (string.IsNullOrEmpty(filter))
            {
                List.AddRange(AllList);
                return;
            }

            List.AddRange(AllList
                .Where(e =>
                    (e.LastName?
                        .Contains(filter,
                            StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (e.FirstName?
                        .Contains(filter,
                            StringComparison.InvariantCultureIgnoreCase) ?? false)
                    ));
        }

        public void OnRequestEmployeesRefresh() =>
            this.Refresh().SafeFireAndForget(e => Console.WriteLine(e.Message));

        public override void Dispose()
        {
            _mediator.RequestEmployeesRefresh -= OnRequestEmployeesRefresh;
            base.Dispose();
        }
        #endregion
    }
}