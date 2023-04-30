using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using System.Diagnostics;
using Quiz.Mobile.Interfaces;
using System.Collections.Generic;
using System.Windows.Input;

namespace Quiz.Mobile.ViewModels.Abstract
{
    public abstract class ItemsCollectionViewModel<T> : BaseViewModel
    {
        #region Pola i komendy
        private IEnumerable<T> _AllList;
        private ObservableRangeCollection<T> _List;
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand AddCommand { get; }
        public IAsyncCommand<T> RemoveCommand { get; }
        public IAsyncCommand<T> SelectedCommand { get; }
        public ICommand FilterCommand { get; }
        #endregion

        #region Konstruktor
        public ItemsCollectionViewModel()
        {
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<T>(Remove);
            SelectedCommand = new AsyncCommand<T>(Selected);
            FilterCommand = new Command<string>(Filter);
        }
        #endregion

        #region Publiczne pola i właściwości
        //Lista przechowująca przefiltrowaną kolekcję
        public ObservableRangeCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load().SafeFireAndForget(ex =>
                    {
                        _List = new ObservableRangeCollection<T>();
                        Console.WriteLine(ex.Message);
                    });
                return _List;
            }
            //SetProperty sprawdzi czy wartość się nie zmieniła, dokona walidacji,
            //przypisze wartość do wybranej właściwości oraz rozgłosi zmianę
            set => SetProperty(ref _List, value);
        }

        //Lista przechowująca całą kolekcję
        public IEnumerable<T> AllList
        {
            get => _AllList;
            set
            {
                if(value != _AllList)
                {
                    _AllList = value;
                    //List = new ObservableRangeCollection<T>(_AllList);
                }
            }
        }
        #endregion

        #region Deklaracje wymaganych metod
        protected abstract Task Refresh();
        protected abstract Task Add();
        protected abstract Task Remove(T obj);
        protected abstract Task Load();
        protected abstract Task Selected(T obj);
        #endregion

        #region Deklaracje opcjonalnych metod
        protected virtual void Filter(string filter) { }
        #endregion
    }
}

