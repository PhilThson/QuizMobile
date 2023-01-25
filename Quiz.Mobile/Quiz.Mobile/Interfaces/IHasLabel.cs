using System;
using Xamarin.Forms;
namespace Quiz.Mobile.Interfaces
{
	public interface IHasLabelViewModel
	{
        IHasLabelView View { get; set; }
    }

	public interface IHasLabelView : IAnimatable
	{
		Label Label { get; }
	}
}

