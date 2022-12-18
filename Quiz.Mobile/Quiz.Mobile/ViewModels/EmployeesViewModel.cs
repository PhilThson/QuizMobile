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

namespace Quiz.Mobile.ViewModels
{
    public class EmployeesViewModel : ItemsCollectionViewModel<EmployeeViewModel>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesViewModel()
        {
            base.Title = "Wszyscy pracownicy";
            //List = new ObservableRangeCollection<EmployeeViewModel>();
            SelectedCommand = new AsyncCommand<object>(Selected);
            _employeeService = DependencyService.Get<IEmployeeService>(DependencyFetchTarget.GlobalInstance);
        }

        protected override async Task Add()
        {
            var route = nameof(AddEmployeePage);
            await AppShell.Current.GoToAsync(route);
        }

        private async Task Remove(EmployeeViewModel employee)
        {
            //teraz można odwołać się do metody z zarejestrowanego serwisu
            await _employeeService.RemoveEmployee(employee.Id);
            await Refresh();
        }

        protected async override Task Refresh()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                List.Clear();
                var employees = await _employeeService.GetAllEmployees();
                List.AddRange(employees);
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync("Odświeżono");
            }
            catch (Exception e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayToastAsync("Nie udało się " +
                    $" pobrać pracowników. Odpowiedź serwera: '{e.Message}'", 5000);
            }
        }

        private EmployeeViewModel _SelectedEmployee;
        public EmployeeViewModel SelectedEmployee
        {
            get => _SelectedEmployee;
            set => SetProperty(ref _SelectedEmployee, value);
        }

        public AsyncCommand<object> SelectedCommand { get; }

        async Task Selected(object args)
        {
            //przesłanie argumentu dzięki EventToCommand 
            //oraz ItemSelectedEventArgsConverter
            var employee = args as EmployeeViewModel;
            if (employee == null)
                return;

            var route = $"{nameof(EmployeeDetailsPage)}?EmployeeId={employee.Id}";
            await Shell.Current.GoToAsync(route);

            SelectedEmployee = null;
        }

        protected override Task Remove(object id)
        {
            throw new NotImplementedException();
        }

        protected override async Task Load()
        {
            var employees = await _employeeService.GetAllEmployees();
            List = new ObservableRangeCollection<EmployeeViewModel>(employees);
        }
    }
}

