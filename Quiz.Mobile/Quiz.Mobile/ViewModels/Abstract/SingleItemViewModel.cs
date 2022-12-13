using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using Xamarin.Forms;

namespace Quiz.Mobile.ViewModels.Abstract
{
	public abstract class SingleItemViewModel<T> : BaseViewModel
        where T : class
	{
        #region Pola, właściwości, komendy
        private T _Item;
        public T Item
        {
            get => _Item;
            set => SetProperty(ref _Item, value);
        }

        public AsyncCommand SaveAndCloseCommand { get; set; }
        public AsyncCommand NavigateBackCommand { get; set; }
        #endregion

        #region Konstruktor
        public SingleItemViewModel()
        {
            SaveAndCloseCommand = new AsyncCommand(SaveAndClose);
            NavigateBackCommand = new AsyncCommand(NavigateBack);
        }
        #endregion

        #region Deklaracje metod
        protected abstract Task SaveAndClose();
        #endregion

        #region Metody
        protected async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}

