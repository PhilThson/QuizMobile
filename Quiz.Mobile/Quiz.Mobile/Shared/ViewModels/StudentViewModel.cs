using System;

namespace Quiz.Mobile.Shared.ViewModels
{
	public class StudentViewModel
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public string Branch { get; set; }
    }
}