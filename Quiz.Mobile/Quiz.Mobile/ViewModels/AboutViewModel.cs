using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Quiz.Mobile.ViewModels.Abstract;

namespace Quiz.Mobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public string AboutImage => "school_about.png";

        public AboutViewModel()
        {
            base.Title = "O programie";
        }
    }
}
