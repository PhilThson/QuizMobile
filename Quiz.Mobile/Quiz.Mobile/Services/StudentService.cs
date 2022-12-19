using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Shared.DTOs;
using Quiz.Mobile.Shared.ViewModels;

namespace Quiz.Mobile.Services
{
	public class StudentService : IStudentService
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

        public StudentService()
		{
		}

        public Task AddStudent(CreateStudentDto studentDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StudentViewModel>> GetAllStudents()
        {
            return await GetAllItems<StudentViewModel>(QuizApiSettings.Students);
        }

        public Task<StudentViewModel> GetStudentById(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveStudent(int studentId)
        {
            throw new NotImplementedException();
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

