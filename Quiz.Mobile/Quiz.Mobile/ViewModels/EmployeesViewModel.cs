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
        private readonly IHttpClientService _client;

        public EmployeesViewModel()
        {
            base.Title = "Wszyscy pracownicy";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
        }

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
                DependencyService.Get<IToast>()?.MakeToast(
                    $"Nie udało się pobrać pracowników. Odpowiedź serwera: {e.Message}");
            }
        }

        private EmployeeViewModel _SelectedEmployee;
        public EmployeeViewModel SelectedEmployee
        {
            get => _SelectedEmployee;
            set => SetProperty(ref _SelectedEmployee, value);
        }

        protected override async Task Selected(EmployeeViewModel employee)
        {
            //przesłanie argumentu dzięki EventToCommand 
            //oraz ItemSelectedEventArgsConverter
            if (employee == null)
                return;

            var route = $"{nameof(EmployeeDetailsPage)}?EmployeeId={employee.Id}";
            await Shell.Current.GoToAsync(route);

            SelectedEmployee = null;
        }

        protected override async Task Remove(EmployeeViewModel employee)
        {
            //await _employeeService.RemoveEmployee(employee.Id);
            //await Refresh();
            await Task.Delay(1000);
        }

        protected override async Task Load()
        {
            var employees = await _client.GetAllItems<EmployeeViewModel>();
            List = new ObservableRangeCollection<EmployeeViewModel>(employees);
        }
    }
}

