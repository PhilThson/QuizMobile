using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IQuizApiSettings _settings;

        public EmployeeService(IQuizApiSettings settings)
        {
            _settings = settings;
        }


    }
}

