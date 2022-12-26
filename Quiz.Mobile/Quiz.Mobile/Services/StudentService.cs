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

        public async Task<List<StudentViewModel>> GetAllStudents() =>
            await _client.GetAllItems<StudentViewModel>();

        public async Task<StudentViewModel> GetStudentById(int id) =>
            await _client.GetItemById<StudentViewModel>(id);

        public async Task AddStudent(CreateStudentDto studentDto)
        {
            await _client.AddItem<CreateStudentDto>(studentDto);
        }

        public Task RemoveStudent(int studentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BranchDto>> GetAllBranches() =>
            await _client.GetAllItems<BranchDto>();
    }
}

