using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Shared.ViewModels;

namespace Quiz.Mobile.Interfaces
{
	public interface IEmployeeService
	{
		Task<List<EmployeeViewModel>> GetAllEmployees();
		Task<EmployeeViewModel> GetEmployeeById(int id);
		Task RemoveEmployee(int employeeId);
		Task AddEmployee(CreateEmployeeDto employeeDto);

		Task<List<JobDto>> GetAllJobs();
        Task<List<PositionDto>> GetAllPositions();
    }
}

