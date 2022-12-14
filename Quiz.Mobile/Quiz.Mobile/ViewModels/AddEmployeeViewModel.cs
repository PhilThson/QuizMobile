using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.CommunityToolkit;
using System.Diagnostics;

namespace Quiz.Mobile.ViewModels
{
    public class AddEmployeeViewModel : SingleItemViewModel<EmployeeViewModel>
    {
        private readonly IEmployeeService _employeeService;

        public AddEmployeeViewModel()
        {
            base.Title = "Dodawanie pracownika";
            Item = new EmployeeViewModel()
            {
                DateOfEmployment = DateTime.Now
            };
            _employeeService = DependencyService.Get<IEmployeeService>();
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
        public JobDto Job
        {
            get => Item.Job;
            set
            {
                if (value != Item.Job)
                {
                    Item.Job = value;
                    OnPropertyChanged();
                }
            }
        }
        public PositionDto Position
        {
            get => Item.Position;
            set
            {
                if (value != Item.Position)
                {
                    Item.Position = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime DateOfEmployment
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
                    LoadJobs().SafeFireAndForget(ex => Debug.WriteLine(ex.Message));
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
                    LoadPositions().SafeFireAndForget(ex => Debug.WriteLine(ex.Message));
                return _Postions;
            }
            set => SetProperty(ref _Postions, value);
        }
        #endregion

        protected override async Task SaveAndClose()
        {
            await base.NavigateBack();
        }

        #region Pobieranie danych słownikowych

        private async Task LoadJobs()
        {
            try
            {
                var jobs = await _employeeService.GetAllJobs();
                Jobs = new ObservableRangeCollection<JobDto>(jobs);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?
                    .MakeToast("Nie udało się pobrać etatów. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }
        private async Task LoadPositions()
        {
            try
            {
                var positions = await _employeeService.GetAllPositions();
                Positions = new ObservableRangeCollection<PositionDto>(positions);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?
                    .MakeToast("Nie udało się pobrać stanowisk. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        #endregion
    }
}

