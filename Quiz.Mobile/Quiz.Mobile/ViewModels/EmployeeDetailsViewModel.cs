using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.CommunityToolkit;
using System.Diagnostics;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(EmployeeId), nameof(EmployeeId))]
    public class EmployeeDetailsViewModel : SingleItemViewModel<EmployeeViewModel>
    {
        #region Prywatne pola
        private readonly IEmployeeService _employeeService;
        #endregion

        #region Właściwości
        private string _EmployeeId;
        public string EmployeeId
        {
            get => _EmployeeId;
            set
            {
                if(value != _EmployeeId)
                {
                    _EmployeeId = value;
                    LoadEmployee().SafeFireAndForget(ex => Debug.WriteLine(ex));
                }
            }
        }
        #endregion

        #region Konstruktor
        public EmployeeDetailsViewModel()
		{
            Item = new EmployeeViewModel();
            Title = "Szczegóły pracownika";
            _employeeService = DependencyService.Get<IEmployeeService>();
		}
        #endregion

        #region Metody
        protected override async Task SaveAndClose()
        {
            await base.NavigateBack();
        }

        private async Task LoadEmployee()
        {
            try
            {
                int.TryParse(_EmployeeId, out var id);
                Item = await _employeeService.GetEmployeeById(id);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?
                    .MakeToast("Nie udało się pobrać pracownika. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        protected override bool CanSave(object arg) => true;
        #endregion
    }
}

