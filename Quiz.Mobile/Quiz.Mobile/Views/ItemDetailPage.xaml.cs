using System.ComponentModel;
using Xamarin.Forms;
using Quiz.Mobile.ViewModels;

namespace Quiz.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
