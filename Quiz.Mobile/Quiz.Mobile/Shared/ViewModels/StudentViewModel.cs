using System;
using Quiz.Mobile.Models;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class StudentViewModel : Person
	{
        public BranchDto? Branch { get; set; }
    }
}