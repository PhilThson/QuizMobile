﻿using System;
using System.Threading.Tasks;
using Quiz.Mobile.CommunityToolkit;
using Quiz.Mobile.CommunityToolkit.Commands;
using Quiz.Mobile.CommunityToolkit.Interfaces;
using System.Diagnostics;
using Xamarin.Forms;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.ViewModels.Abstract
{
    public abstract class ItemsCollectionViewModel<T> : BaseViewModel
    {
        #region Pola i komendy
        private ObservableRangeCollection<T> _List;
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand AddCommand { get; }
        public IAsyncCommand<T> RemoveCommand { get; }
        public IAsyncCommand<T> SelectedCommand { get; }
        #endregion

        #region Konstruktor
        public ItemsCollectionViewModel()
        {
            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<T>(Remove);
            SelectedCommand = new AsyncCommand<T>(Selected);
        }
        #endregion

        #region Publiczne pola i właściwości
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
        #endregion

        #region Deklaracje wymaganych metod
        protected abstract Task Refresh();
        protected abstract Task Add();
        protected abstract Task Remove(T obj);
        protected abstract Task Load();
        protected abstract Task Selected(T obj);
        #endregion
    }
}

