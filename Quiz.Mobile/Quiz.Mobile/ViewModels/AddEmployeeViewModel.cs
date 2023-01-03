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

namespace Quiz.Mobile.ViewModels
{
    public class AddEmployeeViewModel : SingleItemViewModel<CreateEmployeeDto>
    {
        private readonly IHttpClientService _client;

        public AddEmployeeViewModel()
        {
            base.Title = "Dodawanie pracownika";
            Item = new CreateEmployeeDto()
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-20),
                DateOfEmployment = DateTime.Now.Date
            };
            _client = DependencyService.Get<IHttpClientService>();

            this.PropertyChanged +=
                (_, __) => SaveAndCloseCommand.RaiseCanExecuteChanged();
        }

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
        public byte? JobId
        {
            get => Item.JobId;
            set
            {
                if (value != Item.JobId)
                {
                    Item.JobId = value;
                    OnPropertyChanged();
                }
            }
        }
        public byte? PositionId
        {
            get => Item.PositionId;
            set
            {
                if (value != Item.PositionId)
                {
                    Item.PositionId = value;
                    OnPropertyChanged();
                }
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

        #region Metody

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;
                await _client.AddItem<CreateEmployeeDto>(Item);
                IsBusy = false;
                DependencyService.Get<IToast>()?.MakeToast("Poprawnie dodano pracownika!");
                await Task.Delay(2000);
                await base.NavigateBack();
            }
            catch (HttpRequestException e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Dodawanie pracownika",
                    $"Nie udało się dodać pracownika. Odpowiedź serwera: {e.Message}", "OK");
            }
            catch (DataNotFoundException e)
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Dodawanie", e.Message, "OK");
            }
        }

        #endregion

        #region Pobieranie danych słownikowych

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

        protected override bool CanSave(object arg) =>
            !string.IsNullOrEmpty(FirstName) &&
            !string.IsNullOrEmpty(LastName) &&
            _IsPersonalNumberValid &&
            _IsEmailValid &&
            _IsSalaryValid &&
            (DateOfEmployment != null);

        #endregion
    }
}

