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
using Quiz.Mobile.CommunityToolkit;

namespace Quiz.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemType), nameof(ItemType))]
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
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
            Item = new CreateDictionaryDto();

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
                {
                    var action = ItemId == default ? "Dodawanie" : "Edycja";
                    base.Title = _ItemType switch
                    {
                        //obejście dla CS0150: A constant value is expected
                        //bo normalnie nie można robić switch'a po zmiennych
                        var val when val == QuizApiSettings.Areas =>
                            $"{action} obszaru zestawu pytań",
                        var val when val == QuizApiSettings.Difficulties =>
                            $"{action} skali trudności",
                        _ => base.Title
                    };
                }
            }
        }

        public byte _ItemId;
        public byte ItemId
        {
            get => _ItemId;
            set
            {
                if(value != _ItemId)
                {
                    _ItemId = value;
                    LoadItem(_ItemId).SafeFireAndForget(
                        ex => Console.WriteLine(ex.Message));
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => Item.Name;
            set
            {
                if (value != Item.Name)
                {
                    Item.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => Item.Description;
            set
            {
                if (value != Item.Description)
                {
                    Item.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ExtendedName
        {
            get => Item.ExtendedName;
            set
            {
                if (value != Item.ExtendedName)
                {
                    Item.ExtendedName = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSaveProp => CanSave(null);

        #endregion

        #region Metody

        protected override bool CanSave(object arg) =>
            !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Description) &&
            (Name?.Length < 512) &&
            (Description?.Length <= 1024) &&
            (string.IsNullOrEmpty(ExtendedName) ? true :
            (ExtendedName.Length <= 1024)) &&
            IsNotBusy;

        protected override async Task SaveAndClose()
        {
            try
            {
                IsBusy = true;

                if (ItemId == default(byte))
                    await _client.AddItem(Item, ItemType);
                else
                {
                    switch(ItemType)
                    {
                        case var val when val == QuizApiSettings.Areas:
                            await _client.UpdateItem((AreaViewModel)Item);
                            break;
                        case var val when val == QuizApiSettings.Difficulties:
                            await _client.UpdateItem((DifficultyViewModel)Item);
                            break;
                        default:
                            throw new ConversionException(
                                $"Nie znaleziono zadeklarowanego typu " +
                                $"słownikowego ({ItemType})");
                    };
                }

                await Application.Current.MainPage.DisplayToastAsync(
                    "Poprawnie zapisano dane.");
                _mediator.RaiseRequestDictionaryListRefresh(ItemType);
                await Task.Delay(500);
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
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Błąd", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadItem(byte itemId)
        {
            try
            {
                if (string.IsNullOrEmpty(ItemType))
                    throw new DataValidationException("Brak typu danej słownikowej");

                switch (ItemType)
                {
                    case var val when val == QuizApiSettings.Areas:
                        var areaVM = await _client.GetItemById<AreaViewModel>(itemId);
                        Item = (CreateDictionaryDto)areaVM;
                        break;
                    case var val when val == QuizApiSettings.Difficulties:
                        var difficultyVM = await _client.GetItemById<DifficultyViewModel>(itemId);
                        Item = (CreateDictionaryDto)difficultyVM;
                        break;
                    default:
                        throw new ConversionException(
                            $"Nie znaleziono zadeklarowanego typu " +
                            $"słownikowego ({ItemType})");
                };
                foreach (var prop in Item.GetType().GetProperties())
                    OnPropertyChanged(prop.Name);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Edycja",
                    $"Nie udało się załadować rekordu. '{e.Message}'",
                    "OK");
                return;
            }
        }

        #endregion
    }
}

