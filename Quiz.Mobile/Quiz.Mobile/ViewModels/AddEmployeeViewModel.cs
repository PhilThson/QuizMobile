using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Quiz.Mobile.ViewModels
{
    public class AddEmployeeViewModel : SingleItemViewModel<EmployeeViewModel>
    {
        private readonly IEmployeeService _employeeService;

        public AddEmployeeViewModel()
        {
            base.Title = "Dodawanie pracownika";
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
                    //Powinno wystarczyć wywołanie metody bez paramterów,
                    //bo wewnątrz jest sprawdzany tzw. CallMember
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
        #endregion

        protected override async Task SaveAndClose()
        {
            await base.NavigateBack();
        }
    }
}

