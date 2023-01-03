using System;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using Quiz.Mobile.CommunityToolkit.Commands;
using System.Diagnostics;
using System.Net.Http;
using Quiz.Mobile.Views.Employee;
using Xamarin.CommunityToolkit.Extensions;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(EmployeeId), nameof(EmployeeId))]
    public class EmployeeDetailsViewModel : SingleItemViewModel<EmployeeViewModel>
    {
        #region Prywatne pola
        private readonly IHttpClientService _client;
        private IAsyncCommand<int> _RemoveCommand;
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
            _client = DependencyService.Get<IHttpClientService>();
		}
        #endregion

        #region Komendy
        public IAsyncCommand<int> RemoveCommand =>
            _RemoveCommand ??= new AsyncCommand<int>(RemoveEmployee);
        #endregion

        #region Metody
        protected override async Task SaveAndClose()
        {
            throw new NotImplementedException();
        }

        private async Task LoadEmployee()
        {
            try
            {
                int.TryParse(_EmployeeId, out var id);
                Item = await _client.GetItemById<EmployeeViewModel>(id);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?
                    .MakeToast("Nie udało się pobrać pracownika. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        protected override bool CanSave(object arg) => true;

        private async Task RemoveEmployee(int employeeId)
        {
            IsBusy = true;
            try
            {
                await _client.RemoveItemById<EmployeeViewModel>(employeeId);
                await Application.Current.MainPage.DisplayToastAsync("Usunięto pracownika");
                Mediator.Instance.RaiseRequestEmployeesRefresh();
                await Task.Delay(400);
                await base.NavigateBack();
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayAlert("Niepowodzenie",
                    $"Nie udało się usunąć pracownika. '{e.Message}'", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}

