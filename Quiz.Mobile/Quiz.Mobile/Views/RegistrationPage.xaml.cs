using System;
using System.Collections.Generic;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quiz.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage, IHasLabelView
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        public Label Label => LabelLoad;


        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is IHasLabelViewModel hasLabelViewModel)
            {
                hasLabelViewModel.View = this;
            }
            base.OnBindingContextChanged();
        }
    }
}