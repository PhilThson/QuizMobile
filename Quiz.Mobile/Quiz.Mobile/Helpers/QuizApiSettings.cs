using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.Helpers
{
	public static class QuizApiSettings
	{
        public static string ClientName { get; } = "";
        public static string Host { get; } = "http://192.168.1.51:5111/api/";

        public static string MainController { get; } = "data";
        public static string Employees { get; } = "pracownicy";
        public static string Students { get; } = "uczniowie";
    }
}

