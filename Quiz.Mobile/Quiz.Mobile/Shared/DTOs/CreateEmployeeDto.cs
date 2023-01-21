using System;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Shared.ViewModels;

namespace Quiz.Mobile.Shared.DTOs
{
	public class CreateEmployeeDto : CreatePersonDto
	{
        //[Range(2800.0, 10000.0)]
        public decimal Salary { get; set; }
        //[Range(0, 500)]
        public int? DaysOfLeave { get; set; }
        public double? HourlyRate { get; set; }
        public double? Overtime { get; set; }
        //[StringLength(11)]
        public string? PhoneNumber { get; set; }
        //[StringLength(50)]
        public string? Email { get; set; }
        public byte? JobId { get; set; }
        public byte? PositionId { get; set; }
        //[Required]
        public DateTime? DateOfEmployment { get; set; }
        public DateTime? EmploymentEndDate { get; set; }

        public static explicit operator CreateEmployeeDto(EmployeeViewModel employeeVM)
        {
            var createEmployee = new CreateEmployeeDto();
            createEmployee.CopyPropertiesExtension(employeeVM);
            createEmployee.JobId = employeeVM.Job?.Id;
            createEmployee.PositionId = employeeVM.Position?.Id;
            return createEmployee;
        }
    }
}

