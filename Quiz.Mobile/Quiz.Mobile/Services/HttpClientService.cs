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
using System.Linq;

namespace Quiz.Mobile.Services
{
    public class HttpClientService : IHttpClientService
    {
        #region Pola i właściwości
        private readonly IDictionary<Type, string> endpoints;

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
        #endregion

        #region Konstruktor
        public HttpClientService()
        {
            endpoints = GetEndpointsDictionary();
        }
        #endregion

        #region Metody

        public async Task<List<T>> GetAllItems<T>()
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var response = await Client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        public async Task<T> GetItemById<T>(object id)
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var url = $"{endpoint}/{id}";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> GetItemByKey<T>(string key, string value)
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var url = $"{endpoint}?{key}={value}";
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task RemoveItemById<T>(object id)
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
                throw new DataNotFoundException();

            var url = $"{endpoint}/{id}";
            var response = await Client.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                    content = response.ReasonPhrase;
                throw new HttpRequestException(content);
            }
        }

        public async Task<object> AddItem<T>(T item, string dict = null)
        {
            //Ze względu na wykorzystanie jednego Dto'sa do tworzenia
            //Areas oraz Difficulties, to w parametrze jest przekazywany endpoint
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
            {
                if (!string.IsNullOrEmpty(dict))
                    endpoint = dict;
                else
                    throw new DataNotFoundException();
            }

            var dataToSend = new StringContent(JsonConvert.SerializeObject(item),
                Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(endpoint, dataToSend);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);

            //var objectWithId = JsonConvert.DeserializeObject<IHasId>(content);
            //Drugi sposób na wyciągnięcie identyfikatora utworzonego zasobu
            if (!int.TryParse(response.Headers.Location.Segments.Last(), out int objectId))
                throw new DataNotFoundException(
                    "Nie udało się odczytać identyfikatora utworzonego obiektu");

            return objectId;
        }

        public async Task UpdateItem<T>(T item, string dict = null)
        {
            if (!endpoints.TryGetValue(typeof(T), out string endpoint))
            {
                if (!string.IsNullOrEmpty(dict))
                    endpoint = dict;
                else
                    throw new DataNotFoundException();
            }

            var dataToSend = new StringContent(JsonConvert.SerializeObject(item),
                Encoding.UTF8, "application/json");
            var response = await Client.PutAsync(endpoint, dataToSend);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(content);
        }

        #region Metody prywatne

        private Dictionary<Type, string> GetEndpointsDictionary() =>
            new Dictionary<Type, string>
            {
                { typeof(EmployeeViewModel), QuizApiSettings.Employees },
                { typeof(CreateEmployeeDto), QuizApiSettings.Employees },
                { typeof(StudentViewModel), QuizApiSettings.Students },
                { typeof(CreateStudentDto), QuizApiSettings.Students },
                { typeof(JobDto), QuizApiSettings.Jobs },
                { typeof(PositionDto), QuizApiSettings.Positions },
                { typeof(BranchDto), QuizApiSettings.Branches },
                { typeof(DifficultyViewModel), QuizApiSettings.Difficulties },
                { typeof(AreaViewModel), QuizApiSettings.Areas },
                { typeof(RoleDto), QuizApiSettings.Roles },
                { typeof(UserDto), QuizApiSettings.Users },
                { typeof(CreateUserDto), QuizApiSettings.Users },
                { typeof(AddressDto), QuizApiSettings.Addresses }
            };

        #endregion

        #endregion
    }
}