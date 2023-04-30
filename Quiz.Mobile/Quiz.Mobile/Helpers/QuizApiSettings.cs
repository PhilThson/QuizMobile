using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.Helpers
{
	public static class QuizApiSettings
	{
        public static string ClientName { get; } = "";
        public static string Host { get; } = "http://192.168.1.50:5111/api/";
        //public static string Host { get; } = "http://192.168.1.51:5111/api/";
        //public static string Host { get; } = "http://172.20.10.2:5111/api/";

        public static string MainController { get; } = "data";
        public static string UserController { get; } = "user";
        public static string Employees { get; } = $"{MainController}/pracownicy";
        public static string Students { get; } = $"{MainController}/uczniowie";
        public static string Jobs { get; } = $"{MainController}/etaty";
        public static string Positions { get; } = $"{MainController}/stanowiska";
        public static string Branches { get; } = $"{MainController}/oddzialy";
        public static string Areas { get; } = $"{MainController}/obszary";
        public static string Difficulties { get; } = $"{MainController}/skaleTrudnosci";
        public static string Roles { get; set; } = $"{UserController}/roles";
        public static string UserByEmail { get; set; } = $"{UserController}/byEmail";
        public static string Addresses { get; set; } = $"{MainController}/adresy";
    }
}