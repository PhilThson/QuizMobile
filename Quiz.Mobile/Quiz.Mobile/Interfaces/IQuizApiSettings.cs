using System;
namespace Quiz.Mobile.Interfaces
{
	public interface IQuizApiSettings
	{
        string ClientName { get; }
        string Host { get; }

        string MainController { get; }
        string Employees { get; }
        string Students { get; }
    }
}

