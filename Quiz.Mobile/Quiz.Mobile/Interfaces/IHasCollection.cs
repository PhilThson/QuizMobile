using System;
using Xamarin.Forms;

namespace Quiz.Mobile.Interfaces
{
	public interface IHasCollectionViewModel
	{
        IHasCollectionView View { get; set; }
    }

	public interface IHasCollectionView
	{
        CollectionView CollectionView { get; }
    }
}

