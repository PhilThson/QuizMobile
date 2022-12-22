using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quiz.Mobile.Helpers;
using Quiz.Mobile.Helpers.Exceptions;
using Quiz.Mobile.Interfaces;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IDictionary<Type, string> endpoints =
            new Dictionary<Type, string>();

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

        public HttpClientService()
		{
            endpoints.Add(typeof(StudentViewModel), QuizApiSettings.Students);
            endpoints.Add(typeof(EmployeeViewModel), QuizApiSettings.Employees);
            endpoints.Add(typeof(CreateEmployeeDto), QuizApiSettings.Employees);
            endpoints.Add(typeof(JobDto), QuizApiSettings.Jobs);
            endpoints.Add(typeof(PositionDto), QuizApiSettings.Positions);
        }

        public async Task<List<T>> GetAllItems<T>()
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundExceptions();

            var response = await Client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        public async Task<T> GetItemById<T>(object id)
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundExceptions();

            var url = $"{endpoint}/{id}";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<T>(content);
        }

        public Task RemoveItemById(object id)
        {
            throw new NotImplementedException();
        }

        public async Task AddItem<T>(T item)
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundExceptions();

            var dataToSend = new StringContent(JsonConvert.SerializeObject(item),
                Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(endpoint, dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }
    }
}

