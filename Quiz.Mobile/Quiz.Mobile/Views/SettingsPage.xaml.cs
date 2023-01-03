﻿using System;
using System.Collections.Generic;
using Quiz.Mobile.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quiz.Mobile.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            switch (Settings.Theme)
            {
                case 0:
                    RadioButtonSystem.IsChecked = true;
                    break;
                case 1:
                    RadioButtonLight.IsChecked = true;
                    break;
                case 2:
                    RadioButtonDark.IsChecked = true;
                    break;
            }
        }

        bool loaded;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            loaded = true;
        }

        void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!loaded)
                return;

            if (!e.Value)
                return;

            var val = (sender as RadioButton)?.Value as string;
            if (string.IsNullOrWhiteSpace(val))
                return;

            //ustawienie motywu w zależności od zaznaczonego przycisku
            switch (val)
            {
                case "Systemowy":
                    Settings.Theme = 0;
                    break;
                case "Jasny":
                    Settings.Theme = 1;
                    break;
                case "Ciemny":
                    Settings.Theme = 2;
                    break;
            }

            TheTheme.SetTheme();
        }
    }
}

