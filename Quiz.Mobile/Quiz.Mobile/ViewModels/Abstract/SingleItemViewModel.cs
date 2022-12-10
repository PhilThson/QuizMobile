using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using Xamarin.Forms;

namespace Quiz.Mobile.ViewModels.Abstract
{
	public abstract class SingleItemViewModel<T> : BaseViewModel
	{
        #region Pola, właściwości, komendy
        public T Item { get; set; }
        public IAsyncCommand SaveAndCloseCommand;
        public IAsyncCommand NavigateBackCommand;
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
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}

