using System;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.ViewModels.Abstract;
using Xamarin.Forms;
using Quiz.Mobile.Shared.ViewModels;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using System.Diagnostics;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(StudentId), nameof(StudentId))]
    public class StudentDetailsViewModel : SingleItemViewModel<StudentViewModel>
    {
        #region Prywatne pola
        private readonly IHttpClientService _client;
        #endregion

        #region Właściwości
        private string _StudentId;
        public string StudentId
        {
            get => _StudentId;
            set
            {
                if (value != _StudentId)
                {
                    _StudentId = value;
                    LoadStudent().SafeFireAndForget(ex => Console.WriteLine(ex));
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
        protected override async Task SaveAndClose()
        {
            await base.NavigateBack();
        }

        private async Task LoadStudent()
        {
            try
            {
                int.TryParse(_StudentId, out var id);
                Item = await _client.GetItemById<StudentViewModel>(id);
            }
            catch (Exception e)
            {
                DependencyService.Get<IToast>()?
                    .MakeToast("Nie udało się pobrać ucznia. " +
                    $"Odpowiedź serwera: {e.Message}");
            }
        }

        protected override bool CanSave(object arg) => true;
        #endregion
    }
}

