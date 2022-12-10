using System;

namespace Quiz.Mobile.Shared.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public decimal Salary { get; set; }
        public string? Email { get; set; }
        public string Job { get; set; }
        public string Position { get; set; }
        public DateTime DateOfEmployment { get; set; }
    }
}
