using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Quiz.Mobile.Models;
using Quiz.Mobile.Services;
using Quiz.Mobile.CommunityToolkit;

namespace Quiz.Mobile.ViewModels.Abstract
{
    //klasa bazowego ViewModelu z biblioteki CommunityToolkit
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        string? title = string.Empty;

        public string? Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        string? subtitle = string.Empty;

        public string? Subtitle
        {
            get => subtitle;
            set => SetProperty(ref subtitle, value);
        }

        string? icon = string.Empty;

        public string? Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }

        bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        bool isNotBusy = true;

        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }

        bool canLoadMore = true;

        public bool CanLoadMore
        {
            get => canLoadMore;
            set => SetProperty(ref canLoadMore, value);
        }


        string? header = string.Empty;

        public string? Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        string? footer = string.Empty;

        public string? Footer
        {
            get => footer;
            set => SetProperty(ref footer, value);
        }

        public virtual void Dispose() { }
    }
}

