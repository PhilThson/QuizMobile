using System;
using System.ComponentModel;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class DifficultyViewModel
	{
        public byte Id { get; set; }
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string? Description { get; set; }
    }
}

