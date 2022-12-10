using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.Services
{
	public class QuizApiSettings : IQuizApiSettings
	{
        public string ClientName { get; } = "";
        public string Host { get; } = "http://localhost:5111/api";

        public string MainController { get; } = "data";
        public string Employees { get; } = "pracownicy";
        public string Students { get; } = "uczniowie";
    }
}

