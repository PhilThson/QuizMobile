using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using System.Diagnostics;

namespace Quiz.Mobile.ViewModels.Abstract
{
	public abstract class ItemsCollectionViewModel<T> : BaseViewModel
	{
        #region Pola i komendy
        private ObservableRangeCollection<T> _List;
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand AddCommand { get; }
        public IAsyncCommand<object> RemoveCommand { get; }
        #endregion

        #region Konstruktor
        public ItemsCollectionViewModel()
		{
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<object>(Remove);
        }
        #endregion

        #region Publiczne pola i właściwości
        public ObservableRangeCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load().SafeFireAndForget(ex => Debug.WriteLine(ex.Message));
                return _List;
            }
            //SetProperty sprawdzi czy wartość się nie zmieniła, dokona walidacji,
            //przypisze wartość do wybranej właściwości oraz rozgłosi zmianę
            set => SetProperty(ref _List, value);
        }
        #endregion

        #region Deklaracje wymaganych metod
        protected abstract Task Refresh();
        protected abstract Task Add();
        protected abstract Task Remove(object id);
        protected abstract Task Load();
        #endregion
    }
}

