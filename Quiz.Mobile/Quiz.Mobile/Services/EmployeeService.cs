using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Shared.ViewModels;
using Newtonsoft.Json;
using Quiz.Mobile.Helpers;
using System.Net.Http.Headers;
using Quiz.Mobile.Shared.DTOs;
using System.Text;
using Xamarin.Forms;

namespace Quiz.Mobile.Services
{
    public class EmployeeService : IEmployeeService
	{
        private readonly IHttpClientService _client;

        public EmployeeService()
        {
            _client = DependencyService.Get<IHttpClientService>(DependencyFetchTarget.GlobalInstance);
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployees()
        {
            return await _client.GetAllItems<EmployeeViewModel>();
        }

        public async Task<EmployeeViewModel> GetEmployeeById(int id)
        {
            return await _client.GetItemById<EmployeeViewModel>(id);
        }

        public async Task AddEmployee(CreateEmployeeDto employeeDto)
        {
            await _client.AddItem<CreateEmployeeDto>(employeeDto);
        }

        public async Task RemoveEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobDto>> GetAllJobs()
        {
            return await _client.GetAllItems<JobDto>();
        }

        public async Task<List<PositionDto>> GetAllPositions()
        {
            return await _client.GetAllItems<PositionDto>();
        }
    }
}

