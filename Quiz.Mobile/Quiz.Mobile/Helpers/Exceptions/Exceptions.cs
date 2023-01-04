using System;
namespace Quiz.Mobile.Helpers.Exceptions
{
	public class DataNotFoundException : Exception
	{
		public DataNotFoundException() : base("Nie znaleziono danych " +
			"dla podanych parametrów")
		{
		}

		public DataNotFoundException(string message) : base(message)
		{
		}
	}

	public class DataValidationException : Exception
	{
		public DataValidationException() : base("Niepoprawne dane wejściowe")
		{
		}

		public DataValidationException(string message) : base(message)
		{
		}
	}
}

