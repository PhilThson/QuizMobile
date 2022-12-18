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

namespace Quiz.Mobile.Services
{
    public class EmployeeService : IEmployeeService
	{
        private HttpClient _client;
        protected HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    var url = $"{QuizApiSettings.Host}{QuizApiSettings.MainController}/";
                    _client = new HttpClient
                    {
                        BaseAddress = new Uri(url)
                    };
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }
                return _client;
            }
        }

        public EmployeeService()
        {

        }

        public async Task<List<EmployeeViewModel>> GetAllEmployees()
        {
            return await GetAllItems<EmployeeViewModel>(QuizApiSettings.Employees);
        }

        public async Task<EmployeeViewModel> GetEmployeeById(int id)
        {
            var url = $"{QuizApiSettings.Employees}/{id}";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<EmployeeViewModel>(content);
        }

        public async Task AddEmployee(CreateEmployeeDto employeeDto)
        {
            var dataToSend = new StringContent(JsonConvert.SerializeObject(employeeDto),
                Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(QuizApiSettings.Employees,
                dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }

        public async Task RemoveEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobDto>> GetAllJobs()
        {
            return await GetAllItems<JobDto>(QuizApiSettings.Jobs);
        }

        public async Task<List<PositionDto>> GetAllPositions()
        {
            return await GetAllItems<PositionDto>(QuizApiSettings.Positions);
        }

        private async Task<List<T>> GetAllItems<T>(string endpoint)
        {
            var response = await Client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<List<T>>(content);
        }
    }
}

