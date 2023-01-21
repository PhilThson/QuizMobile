using System;
using System.ComponentModel;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Models;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Shared.ViewModels
{
    public class DifficultyViewModel : DictionaryItem
    {
        public static explicit operator DifficultyViewModel(
            CreateDictionaryDto createDictionary)
        {
            var difficultyVM = new DifficultyViewModel();
            difficultyVM.CopyPropertiesExtension(createDictionary);
            return difficultyVM;
        }
    }
}

