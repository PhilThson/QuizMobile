using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.Helpers
{
	public static class QuizApiSettings
	{
        public static string ClientName { get; } = "";
        public static string Host { get; } = "http://192.168.1.51:5111/api/";
        //public static string Host { get; } = "http://172.20.10.2:5111/api/";

        public static string MainController { get; } = "data";
        public static string Employees { get; } = "pracownicy";
        public static string Students { get; } = "uczniowie";
        public static string Jobs { get; } = "etaty";
        public static string Positions { get; } = "stanowiska";
        public static string Branches { get; } = "oddzialy";
        public static string Areas { get; } = "obszary";
        public static string Difficulties { get; } = "skaleTrudnosci";
        public static string Roles { get; set; } = "role";
        public static string Users { get; set; } = "uzytkownicy";
    }
}