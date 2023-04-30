using System;
using System.ComponentModel;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Models;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class AreaViewModel : DictionaryItem
	{
        [DisplayName("Nazwa rozszerzona")]
        public string? ExtendedName { get; set; }

        public static explicit operator AreaViewModel(
            CreateDictionaryDto createDictionary)
        {
            var areaVM = new AreaViewModel();
            areaVM.CopyPropertiesExtension(createDictionary);
            return areaVM;
        }
    }
}

