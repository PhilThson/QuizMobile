using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.CommunityToolkit;
using System.Diagnostics;
using System.Net.Http;
using Xamarin.CommunityToolkit.Extensions;
using Quiz.Mobile.Helpers.Exceptions;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Views.Employee;
using System.Collections.Generic;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.Models;
using Quiz.Mobile.Views.Address;
using System.Linq;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(EmployeeId), nameof(EmployeeId))]
    [QueryProperty(nameof(CreatedAddressId), nameof(CreatedAddressId))]
    public class AddEmployeeViewModel : SingleItemViewModel<CreateEmployeeDto>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public AddEmployeeViewModel()
        {
            base.Title = "Dodawanie pracownika";
            Item = new CreateEmployeeDto()
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-20),
                DateOfEmployment = DateTime.Now.Date,
                EmploymentEndDate = DateTime.Now.Date.AddDays(1)
            };

            _client = DependencyService.Get<IHttpClientService>();
            _mediator = Mediator.Instance;

            this.PropertyChanged +=
                (_, __) => SaveAndCloseCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Właściwości pracownika
        public string FirstName
        {
            get => Item.FirstName;
            set
            {
                if (value != Item.FirstName)
                {
                    Item.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LastName
        {
            get => Item.LastName;
            set
            {
                if (value != Item.LastName)
                {
                    Item.LastName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PersonalNumber
        {
            get => Item.PersonalNumber;
            set
            {
                if (value != Item.PersonalNumber)
                {
                    Item.PersonalNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _IsPersonalNumberValid = false;
        public bool IsPersonalNumberValid
        {
            get => _IsPersonalNumberValid;
            set => SetProperty(ref _IsPersonalNumberValid, value);
        }

        public DateTime? DateOfBirth
        {
            get => Item.DateOfBirth;
            set
            {
                if (value != Item.DateOfBirth)
                {
                    Item.DateOfBirth = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Salary
        {
            get => Item.Salary;
            set
            {
                if (value != Item.Salary)
                {
                    Item.Salary = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _IsSalaryValid = false;
        public bool IsSalaryValid
        {
            get => _IsSalaryValid;
            set => SetProperty(ref _IsSalaryValid, value);
        }

        public int? DaysOfLeave
        {
            get => Item.DaysOfLeave;
            set
            {
                if (value != Item.DaysOfLeave)
                {
                    Item.DaysOfLeave = value;
                    OnPropertyChanged();
                }
            }
        }

        public double? HourlyRate
        {
            get => Item.HourlyRate;
            set
            {
                if (value != Item.HourlyRate)
                {
                    Item.HourlyRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public double? Overtime
        {
            get => Item.Overtime;
            set
            {
                if (value != Item.Overtime)
                {
                    Item.Overtime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Email
        {
            get => Item.Email;
            set
            {
                if (value != Item.Email)
                {
                    Item.Email = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _IsEmailValid = false;
        public bool IsEmailValid
        {
            get => _IsEmailValid;
            set => SetProperty(ref _IsEmailValid, value);
        }

        public string? PhoneNumber
        {
            get => Item.PhoneNumber;
            set
            {
                if (value != Item.PhoneNumber)
                {
                    Item.PhoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _IsPhoneNumberValid = false;
        public bool IsPhoneNumberValid
        {
            get => _IsPhoneNumberValid;
            set => SetProperty(ref _IsPhoneNumberValid, value);
        }

        private byte? _SelectedJobIndex;
        public byte? SelectedJobIndex
        {
            get => _SelectedJobIndex;
            set
            {
                SetProperty(ref _SelectedJobIndex, value);
                Item.JobId = Jobs[_SelectedJobIndex.Value].Id;
            }
        }
        private byte? _SelectedPositionIndex;
        public byte? SelectedPositionIndex
        {
            get => _SelectedPositionIndex;
            set
            {
                SetProperty(ref _SelectedPositionIndex, value);
                Item.PositionId = Positions[_SelectedPositionIndex.Value].Id;
            }
        }
        public DateTime? DateOfEmployment
        {
            get => Item.DateOfEmployment;
            set
            {
                if (value != Item.DateOfEmployment)
                {
                    Item.DateOfEmployment = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? EmploymentEndDate
        {
            get => Item.EmploymentEndDate;
            set
            {
                if (value != Item.EmploymentEndDate)
                {
                    Item.EmploymentEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableRangeCollection<JobDto> _Jobs;
        public ObservableRangeCollection<JobDto> Jobs
        {
            get
            {
                if (_Jobs == null)
                    LoadJobs().SafeFireAndForget(ex =>
                    {
                        Debug.WriteLine(ex.Message);
                    });
                return _Jobs;
            }
            set => SetProperty(ref _Jobs, value);
        }

        private ObservableRangeCollection<PositionDto> _Postions;
        public ObservableRangeCollection<PositionDto> Positions
        {
            get
            {
                if (_Postions == null)
                    LoadPositions().SafeFireAndForget(ex => Debug.WriteLine(ex));
                return _Postions;
            }
            set => SetProperty(ref _Postions, value);
        }

        #endregion

        #region Właściwości adresu

        private ObservableRangeCollection<AddressDto> _Addresses;
        public ObservableRangeCollection<AddressDto> Addresses =>
            _Addresses ??= new ObservableRangeCollection<AddressDto>();

        #endregion

        #region Właściwości nawigacyjne

        private int _EmployeeId;
        public int EmployeeId
        {
            get => _EmployeeId;
            set
            {
                if (value != _EmployeeId)
                {
                    _EmployeeId = value;
                    Title = "Edycja pracownika";
                    LoadEmployee().SafeFireAndForget(
                        ex => Console.WriteLine(ex.Message));
                    //bo binduje sie na widoku
                    OnPropertyChanged();
                }
            }
        }

        private int _CreatedAddressId;
        public int CreatedAddressId
        {
            get => _CreatedAddressId;
            set
            {
                if(value != _CreatedAddressId)
                {
                    _CreatedAddressId = value;
                    LoadCreatedAddress().SafeFireAndForget(ex =>
                        Console.WriteLine(ex.Message));
                }
            }
        }

        public bool CanSaveProp => CanSave(null);

        #endregion

        #region Komendy
        private IAsyncCommand _AddAddressCommand;
        public IAsyncCommand AddAddressCommand =>
            _AddAddressCommand ??= new AsyncCommand(AddAddress);

        private async Task AddAddress()
        {
            var route = $"{nameof(AddAddressPage)}?IsForEmployee=1";
            await AppShell.Current.GoToAsync(route);
        }
        #endregion

        #region Metody

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;
                if (Item.Id == default)
                {
                    Item.AddressesIds = new List<int>(Addresses.Select(a => a.Id));
                    await _client.AddItem(Item);
                }
                else
                    await _client.UpdateItem(Item);

                DependencyService.Get<IToast>()?.MakeToast("Zapisano pracownika!");
                await Task.Delay(2000);
                _mediator.RaiseRequestEmployeesRefresh();
                await Shell.Current.GoToAsync($"//{nameof(EmployeesPage)}");
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayAlert(Title,
                    $"Niepowodzenie. Odpowiedź serwera: {e.Message}", "OK");
            }
            catch (DataNotFoundException e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Title, e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override bool CanSave(object arg) =>
            !string.IsNullOrEmpty(FirstName) &&
            !string.IsNullOrEmpty(LastName) &&
            _IsPersonalNumberValid &&
            _IsEmailValid &&
            _IsSalaryValid &&
            ((string.IsNullOrEmpty(PhoneNumber)) ? true :
                _IsPhoneNumberValid) &&
            (DateOfEmployment.HasValue) &&
            (DateOfBirth.HasValue) &&
            (DateOfBirth < DateTime.Now.Date) &&
            ((!DaysOfLeave.HasValue) ? true :
                (DaysOfLeave <= 500)) &&
            ((!EmploymentEndDate.HasValue) ? true :
                (EmploymentEndDate >= DateOfEmployment.Value.Date)) &&
            IsNotBusy
            ;

        #endregion

        #region Pobieranie danych

        private async Task LoadJobs()
        {
            try
            {
                var jobs = await _client.GetAllItems<JobDto>();
                Jobs = new ObservableRangeCollection<JobDto>(jobs);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?.MakeToast(
                    "Nie udało się pobrać etatów. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }
        private async Task LoadPositions()
        {
            try
            {
                var positions = await _client.GetAllItems<PositionDto>();
                Positions = new ObservableRangeCollection<PositionDto>(positions);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?.MakeToast(
                    "Nie udało się pobrać stanowisk. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        private async Task LoadEmployee()
        {
            try
            {
                IsBusy = true;
                var employeeFromDb =
                    await _client.GetItemById<EmployeeViewModel>(_EmployeeId);
                Item = (CreateEmployeeDto)employeeFromDb;
                Addresses.AddRange(employeeFromDb.Addresses);
                foreach (var prop in Item.GetType().GetProperties())
                    OnPropertyChanged(prop.Name);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Edycja",
                    $"Nie udało się załadować pracownika. '{e.Message}'",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadCreatedAddress()
        {
            try
            {
                IsBusy = true;
                var addressFromDb =
                    await _client.GetItemById<AddressDto>(_CreatedAddressId);
                Addresses.Add(addressFromDb);
            }
            catch(Exception e)
            {
                DependencyService.Get<IToast>()?.MakeToast(
                    $"Nie udało się pobrać utworzonego adresu. '{e.Message}'");
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion
    }
}

