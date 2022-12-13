using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit;
using Xamarin.Forms;
using System.Windows.Input;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Views.Employee;
using Quiz.Mobile.Shared.ViewModels;
using System.Collections.Generic;

namespace Quiz.Mobile.ViewModels
{
    public class EmployeesViewModel : ItemsCollectionViewModel<EmployeeViewModel>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesViewModel()
        {
            base.Title = "Wszyscy pracownicy";

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
            IsBusy = true;

            await Task.Delay(1000);

            List.Clear();

            var employees = await _employeeService.GetAllEmployees();

            List.AddRange(employees);

            IsBusy = false;

            DependencyService.Get<IToast>()?.MakeToast("Refreshed");
        }

        private EmployeeViewModel selectedEmployee;
        //odwzorowanie zdarzenia ItemSelected z CodeBehindu
        public EmployeeViewModel SelectedEmployee
        {
            get => selectedEmployee;
            set => SetProperty(ref selectedEmployee, value);
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
            await AppShell.Current.GoToAsync(route);

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

