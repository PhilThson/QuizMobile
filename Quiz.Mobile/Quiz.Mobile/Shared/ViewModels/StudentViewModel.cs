using System;
using Quiz.Mobile.Models;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class StudentViewModel : Person
	{
        //public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public string PersonalNumber { get; set; }
        public BranchDto? Branch { get; set; }
    }
}