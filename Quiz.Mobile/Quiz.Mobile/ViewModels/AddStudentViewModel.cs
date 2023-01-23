using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Interfaces;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Quiz.Mobile.CommunityToolkit;
using System.Net.Http;
using Quiz.Mobile.Helpers.Exceptions;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Views.Student;
using Xamarin.CommunityToolkit.Extensions;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), nameof(StudentId))]
    public class AddStudentViewModel : SingleItemViewModel<CreateStudentDto>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public AddStudentViewModel()
		{
            Title = "Dodawanie ucznia";
            Item = new CreateStudentDto()
            {
                DateOfBirth = DateTime.Now.Date.AddYears(-7)
            };
			_client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            _mediator = Mediator.Instance;
            this.PropertyChanged +=
                (_, __) => SaveAndCloseCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Właściwości ucznia
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

        public string DisabilityCert
        {
            get => Item.DisabilityCert;
            set
            {
                if (value != Item.DisabilityCert)
                {
                    Item.DisabilityCert = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PlaceOfBirth
        {
            get => Item.PlaceOfBirth;
            set
            {
                if (value != Item.PlaceOfBirth)
                {
                    Item.PlaceOfBirth = value;
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

        private ObservableRangeCollection<BranchDto> _Branches;
        public ObservableRangeCollection<BranchDto> Branches
        {
            get
            {
                if (_Branches == null)
                    LoadBranches().SafeFireAndForget(ex =>
                        Console.WriteLine(ex.Message));
                return _Branches;
            }
            set => SetProperty(ref _Branches, value);
        }

        public byte? BranchId
        {
            get => Item.BranchId;
            set
            {
                if (value != Item.BranchId)
                {
                    Item.BranchId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _StudentId;
        public int StudentId
        {
            get => _StudentId;
            set
            {
                if(value != _StudentId)
                {
                    _StudentId = value;
                    Title = "Edycja ucznia";
                    LoadStudent(_StudentId).SafeFireAndForget(
                        ex => Console.WriteLine(ex.Message));
                }
            }
        }

        public bool IsValidForm => CanSave(null);
        #endregion

        #region Metody

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;
                if (Item.Id == default)
                    await _client.AddItem(Item);
                else
                    await _client.UpdateItem(Item);

                DependencyService.Get<IToast>()?.MakeToast("Poprawnie zapisano ucznia!");
                _mediator.RaiseRequestStudentsRefresh();
                await Task.Delay(2000);
                await Shell.Current.GoToAsync($"//{nameof(StudentsPage)}");
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                    $"Niepowodzenie. Odpowiedź serwera: {e.Message}", 5000);
            }
            catch (DataNotFoundException e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Wystąpił błąd", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override bool CanSave(object arg) =>
            !string.IsNullOrEmpty(FirstName) &&
            !string.IsNullOrEmpty(LastName) &&
            !string.IsNullOrEmpty(PlaceOfBirth) &&
            !string.IsNullOrEmpty(DisabilityCert) &&
            DisabilityCert.Length <= 15 &&
            _IsPersonalNumberValid &&
            (DateOfBirth.HasValue) &&
            (DateOfBirth < DateTime.Now.Date) &&
            BranchId.HasValue &&
            IsNotBusy;

        #endregion

        #region Pobranie danych

        private async Task LoadBranches()
        {
            try
            {
                var branches = await _client.GetAllItems<BranchDto>();
                Branches = new ObservableRangeCollection<BranchDto>(branches);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?.MakeToast(
                    "Nie udało się pobrać oddziałów. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        private async Task LoadStudent(int id)
        {
            try
            {
                IsBusy = true;
                var studentFromDb = await _client.GetItemById<StudentViewModel>(id);
                Item = (CreateStudentDto)studentFromDb;
                foreach (var prop in Item.GetType().GetProperties())
                    OnPropertyChanged(prop.Name);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Edycja",
                    $"Nie udało się załadować ucznia. '{e.Message}'",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}

