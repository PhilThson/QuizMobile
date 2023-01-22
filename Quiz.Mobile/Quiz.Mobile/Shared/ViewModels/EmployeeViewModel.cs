using System;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Models;

namespace Quiz.Mobile.Shared.ViewModels
{
    public class EmployeeViewModel : Person
    {
        public decimal Salary { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public JobDto? Job { get; set; }
        public PositionDto? Position { get; set; }
        public DateTime? DateOfEmployment { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
    }
}
