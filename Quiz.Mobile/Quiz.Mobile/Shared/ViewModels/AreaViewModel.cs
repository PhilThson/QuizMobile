using System;
using System.ComponentModel;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class AreaViewModel
	{
        public byte Id { get; set; }
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [DisplayName("Opis")]
        public string? Description { get; set; }
        [DisplayName("Nazwa rozszerzona")]
        public string? ExtendedName { get; set; }
    }
}

