using Quiz.Mobile.CommunityToolkit.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace Quiz.Mobile.ViewModels
{
    public class ImageCacheViewModel : BaseViewModel
    {
        private Command refreshCommand;

        public ImageSource Image { get; set; } =
            (Device.RuntimePlatform == Device.Android) ?
            ImageSource.FromFile("idea.png") :
            ImageSource.FromFile("Images/idea.png");
            //new UriImageSource
            //{
            //    Uri = new Uri("https:///00032.jpg"),
            //    CachingEnabled = true,
            //    CacheValidity = TimeSpan.FromMinutes(1)
            //};


        public ImageCacheViewModel()
        {
            //do wyświetlania animacji przez 5 sekund (bo np. czekamy na odpowiedź http)
            RefreshLongCommand = new AsyncCommand(async () =>
            {
                IsBusy = true;
                await Task.Delay(5000);
                IsBusy = false;
            });
        }


        public ICommand RefreshCommand => refreshCommand ??= new Command(Refresh);

        public AsyncCommand RefreshLongCommand { get; }

        private void Refresh()
        {
            //Image = new UriImageSource
            //{
            //    Uri = new Uri("https:///00032.jpg"),
            //    CachingEnabled = true,
            //    CacheValidity = TimeSpan.FromMinutes(1)
            //};
            Image =
            (Device.RuntimePlatform == Device.Android) ?
            ImageSource.FromFile("win.png") :
            ImageSource.FromFile("Images/win.png");

            OnPropertyChanged(nameof(Image));
        }
    }
}

