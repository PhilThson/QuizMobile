using System;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.ViewModels.Abstract;
using Xamarin.Forms;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using System.Diagnostics;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Views.Dictionary;
using Quiz.Mobile.Views.Student;
using Xamarin.CommunityToolkit.Extensions;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), nameof(StudentId))]
    public class StudentDetailsViewModel : SingleItemViewModel<StudentViewModel>
    {
        #region Prywatne pola
        private readonly IHttpClientService _client;
        #endregion

        #region Właściwości
        private int _StudentId;
        public int StudentId
        {
            get => _StudentId;
            set
            {
                if (value != _StudentId)
                {
                    _StudentId = value;
                    LoadStudent().SafeFireAndForget(ex => Console.WriteLine(ex));
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Konstruktor
        public StudentDetailsViewModel()
        {
            Item = new StudentViewModel();
            Title = "Szczegóły ucznia";
            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
        }
        #endregion

        #region Metody
        //Przekierowanie do edycji
        protected override async Task SaveAndClose()
        {
            var route = $"{nameof(AddStudentPage)}?StudentId={StudentId}";
            await AppShell.Current.GoToAsync(route);
        }

        private async Task LoadStudent()
        {
            try
            {
                IsBusy = true;
                Item = await _client.GetItemById<StudentViewModel>(_StudentId);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayToastAsync(
                        "Nie udało się pobrać ucznia. " +
                    $"Odpowiedź serwera: {e.Message}", 5000);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override bool CanSave(object arg) => true;
        #endregion
    }
}

