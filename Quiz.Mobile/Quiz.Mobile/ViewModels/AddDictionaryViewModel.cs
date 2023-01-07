using System;
using System.Threading.Tasks;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.ViewModels.Abstract;
using Quiz.Mobile.Interfaces;
using Xamarin.Forms;
using Quiz.Mobile.Helpers.Exceptions;
using Quiz.Mobile.Shared.ViewModels;
using System.Net.Http;
using Quiz.Mobile.Helpers;
using Xamarin.CommunityToolkit.Extensions;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemType), nameof(ItemType))]
    public class AddDictionaryViewModel : SingleItemViewModel<CreateDictionaryDto>
    {
        #region Pola prywatne
        private readonly IHttpClientService _client;
        private readonly IMediator _mediator;
        #endregion

        #region Konstruktor
        public AddDictionaryViewModel()
        {
            base.Title = "Dodawanie danych słownikowych";

            _client = DependencyService.Get<IHttpClientService>(
                DependencyFetchTarget.GlobalInstance);
            _mediator = Mediator.Instance;

            this.PropertyChanged +=
                (_, __) => SaveAndCloseCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Właściwości

        private string _ItemType;
        public string ItemType
        {
            get => _ItemType;
            set
            {
                SetProperty(ref _ItemType, value);
                if (value != null)
                    base.Title = _ItemType switch
                    {
                        //obejście dla CS0150: A constant value is expected
                        //bo normalnie nie można robić switch'a po zmiennych
                        var val when val == QuizApiSettings.Areas =>
                            "Dodawanie obszaru zestawu pytań",
                        var val when val == QuizApiSettings.Difficulties =>
                            "Dodawanie skali trudności",
                        _ => base.Title
                    };
            }
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }

        #endregion

        #region Metody

        protected override bool CanSave(object arg) =>
            !string.IsNullOrEmpty(_Name) &&
            !string.IsNullOrEmpty(_Description) &&
            (_Name?.Length < 512) &&
            (_Description?.Length <= 1024) &&
            IsNotBusy;

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;
                Item = new CreateDictionaryDto
                {
                    Name = Name,
                    Description = Description
                };
                await _client.AddItem<CreateDictionaryDto>(Item, ItemType);
                await Application.Current.MainPage.DisplayToastAsync(
                    "Poprawnie zapisano dane.");
                await Task.Delay(1000);
                _mediator.RaiseRequestDictionaryListRefresh(ItemType);
                await base.NavigateBack();
            }
            catch (HttpRequestException e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Dodawanie",
                    $"Nie udało się dodać rekordu. Odpowiedź serwera: {e.Message}",
                    "OK");
            }
            catch (DataNotFoundException e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Dodawanie", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}

