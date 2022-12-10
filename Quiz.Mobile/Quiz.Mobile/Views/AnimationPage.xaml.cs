using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quiz.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quiz.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimationPage : ContentPage
    {
        ImageCacheViewModel vm;

        readonly Animation rotation;

        public AnimationPage()
        {
            InitializeComponent();

            rotation = new Animation(v => LabelLoad.Rotation = v,
                0, 360, Easing.Linear);

            vm = (ImageCacheViewModel)BindingContext;

            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(vm.IsBusy))
            {
                if (vm.IsBusy)
                {
                    //animate
                    //każdy ContentPage implementuje IAnimatable!
                    rotation.Commit(this, "rotate", 16, 1000, Easing.Linear,
                        (v, c) => LabelLoad.Rotation = 0, //finished
                        () => true); //repeat
                }
                else
                {
                    //stop

                    this.AbortAnimation("rotate");
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //można to też zrobić generycznie:
            //przekazując View do ViewModel'u
            //za pomocą komendy ButtonClickedCommand
            //i animując tenże widok (View)
            var a1 = LabelLoad.FadeTo(0, 1000, Easing.Linear);
            var a2 = LabelLoad.ScaleTo(2.0, 1000, Easing.BounceIn);

            await Task.WhenAll(a1, a2);

            var a3 = LabelLoad.FadeTo(1, 1000, Easing.Linear);
            var a4 = LabelLoad.ScaleTo(1.0, 1000, Easing.BounceOut);

            await Task.WhenAll(a3, a4);
        }
    }
}

