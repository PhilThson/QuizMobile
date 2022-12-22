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
using Xamarin.Forms;

namespace Quiz.Mobile.Services
{
	public class StudentService : IStudentService
	{
        private readonly IHttpClientService _client;

        public StudentService()
		{
            _client = DependencyService.Get<IHttpClientService>(DependencyFetchTarget.GlobalInstance);
        }

        public async Task<List<StudentViewModel>> GetAllStudents()
        {
            return await _client.GetAllItems<StudentViewModel>();
        }

        public async Task<StudentViewModel> GetStudentById(int id)
        {
            return await _client.GetItemById<StudentViewModel>(id);
        }

        public Task AddStudent(CreateStudentDto studentDto)
        {
            throw new NotImplementedException();
        }

        public Task RemoveStudent(int studentId)
        {
            throw new NotImplementedException();
        }
    }
}

