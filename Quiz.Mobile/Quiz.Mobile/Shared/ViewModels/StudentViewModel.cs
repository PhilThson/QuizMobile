using System;
using Quiz.Mobile.Models;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class StudentViewModel : Person
	{
        public string DisabilityCert { get; set; }
        public string PlaceOfBirth { get; set; }
        public BranchDto? Branch { get; set; }
    }
}