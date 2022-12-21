using System;
using Quiz.Mobile.Models;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class StudentViewModel : Person
	{
        //public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public string PersonalNumber { get; set; }
        public string Branch { get; set; }
    }
}