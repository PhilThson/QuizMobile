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

namespace Quiz.Mobile.ViewModels
{
    public class EmployeesViewModel : ItemsCollectionViewModel<EmployeeViewModel>,
        IDisposable
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
                var employees = await _client.GetAllItems<EmployeeViewModel>();
                List.AddRange(employees);
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

        //niewykorzystywane w przypadku ListView
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
            var employees = await _client.GetAllItems<EmployeeViewModel>();
            List = new ObservableRangeCollection<EmployeeViewModel>(employees);
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

        public void OnRequestEmployeesRefresh() =>
            this.Refresh().SafeFireAndForget(e => Console.WriteLine(e.Message));

        public void Dispose()
        {
            _mediator.RequestEmployeesRefresh -= OnRequestEmployeesRefresh;
        }
        #endregion
    }
}

