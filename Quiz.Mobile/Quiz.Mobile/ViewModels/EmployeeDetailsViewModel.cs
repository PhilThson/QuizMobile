﻿using System;
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
using Quiz.Mobile.Views.Student;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(EmployeeId), nameof(EmployeeId))]
    public class EmployeeDetailsViewModel : SingleItemViewModel<EmployeeViewModel>
    {
        #region Prywatne pola
        private readonly IHttpClientService _client;
        private IAsyncCommand<int> _RemoveCommand;
        #endregion

        #region Konstruktor
        public EmployeeDetailsViewModel()
        {
            Item = new EmployeeViewModel();
            Title = "Szczegóły pracownika";
            _client = DependencyService.Get<IHttpClientService>();
        }
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

        private bool _HasAddress;
        public bool HasAddress
        {
            get => _HasAddress;
            set => SetProperty(ref _HasAddress, value);
        }
        #endregion

        #region Komendy
        public IAsyncCommand<int> RemoveCommand =>
            _RemoveCommand ??= new AsyncCommand<int>(RemoveEmployee);
        #endregion

        #region Metody
        //Przekierowanie do edycji
        protected override async Task SaveAndClose()
        {
            var route = $"{nameof(AddEmployeePage)}?EmployeeId={EmployeeId}";
            await AppShell.Current.GoToAsync(route);
        }

        private async Task LoadEmployee()
        {
            try
            {
                IsBusy = true;
                int.TryParse(_EmployeeId, out var id);
                Item = await _client.GetItemById<EmployeeViewModel>(id);
                if (Item.Addresses?.Count > 0)
                    HasAddress = true;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Błąd",
                    "Nie udało się pobrać pracownika. " +
                    $"Odpowiedź serwera: {e.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override bool CanSave(object arg) => true;

        private async Task RemoveEmployee(int employeeId)
        {
            try
            {
                var accept = await Application.Current.MainPage.DisplayAlert(
                    "Usuwanie", "Czy na pewno usunąć pracownika?", "TAK", "Anuluj");
                if (!accept)
                    return;
                IsBusy = true;
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

