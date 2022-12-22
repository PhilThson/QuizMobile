using System;
namespace Quiz.Mobile.Helpers.Exceptions
{
	public class DataNotFoundExceptions : Exception
	{
		public DataNotFoundExceptions() : base("Nie znaleziono danych " +
			"dla podanych parametrów")
		{
		}

		public DataNotFoundExceptions(string message) : base(message)
		{
		}
	}
}

