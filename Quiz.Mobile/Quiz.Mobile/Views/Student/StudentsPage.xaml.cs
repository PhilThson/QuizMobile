using System;
using System.Collections.Generic;
using Quiz.Mobile.Interfaces;
using Xamarin.Forms;

namespace Quiz.Mobile.Views.Student
{
    public partial class StudentsPage : ContentPage, IHasCollectionView
    {
        public StudentsPage()
        {
            InitializeComponent();
        }

        CollectionView IHasCollectionView.CollectionView => collectionView;

        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is IHasCollectionViewModel hasCollectionViewModel)
            {
                hasCollectionViewModel.View = this;
            }
            base.OnBindingContextChanged();
        }
    }
}

