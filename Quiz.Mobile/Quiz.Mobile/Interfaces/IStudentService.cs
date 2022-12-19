using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quiz.Mobile.Shared.ViewModels;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Interfaces
{
	public interface IStudentService
	{
        Task<List<StudentViewModel>> GetAllStudents();
        Task<StudentViewModel> GetStudentById(int id);
        Task RemoveStudent(int studentId);
        Task AddStudent(CreateStudentDto studentDto);
    }
}

