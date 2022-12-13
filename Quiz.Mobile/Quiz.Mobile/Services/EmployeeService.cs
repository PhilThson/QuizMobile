using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Shared.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Quiz.Mobile.Services;
using Quiz.Mobile.Helpers;
using System.Net.Http.Headers;

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
            var response = await Client.GetAsync(QuizApiSettings.Employees);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(content);
            }
            return JsonConvert.DeserializeObject<List<EmployeeViewModel>>(content);
        }

        public async Task<EmployeeViewModel> GetEmployeeById(int id)
        {
            var url = $"{QuizApiSettings.Employees}/{id}";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(content);
            }
            return JsonConvert.DeserializeObject<EmployeeViewModel>(content);
        }

        public async Task AddEmployee(EmployeeViewModel employee)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveEmployee(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}

