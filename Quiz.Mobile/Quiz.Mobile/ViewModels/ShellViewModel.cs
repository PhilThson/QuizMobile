using System;
using Xamarin.Essentials;
using Quiz.Mobile.ViewModels.Abstract;

namespace Quiz.Mobile.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public string HeaderImage => "school.png";
        public string AppTitle => "Placówka Oświatowa Mobile";
        public string Version => $"Wersja {VersionTracking.CurrentVersion}";
    }
}

