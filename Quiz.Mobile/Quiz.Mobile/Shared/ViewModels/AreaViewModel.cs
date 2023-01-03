using System;
using System.ComponentModel;
using Quiz.Mobile.Models;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class AreaViewModel : DictionaryItem
	{
        [DisplayName("Nazwa rozszerzona")]
        public string? ExtendedName { get; set; }
    }
}

