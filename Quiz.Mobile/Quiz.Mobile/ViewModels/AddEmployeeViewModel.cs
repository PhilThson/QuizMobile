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
            base.Title = "Szczegóły pracownika";
            _employeeService = DependencyService.Get<IEmployeeService>();
        }

        #region Właściwości pracownika
        public string Imie
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
        public string Nazwisko
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
        #endregion

        protected override async Task SaveAndClose()
        {
            throw new NotImplementedException();
        }
    }
}

