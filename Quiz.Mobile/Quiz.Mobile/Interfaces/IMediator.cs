using System;
using Quiz.Mobile.ViewModels.Abstract;

namespace Quiz.Mobile.Interfaces
{
    public interface IMediator
    {
        static IMediator Instance { get; }

        /// <summary>
        /// Zdarzenie do wysłania żądania odświeżenia widoku pracowników
        /// </summary>
        event Action RequestEmployeesRefresh;

        /// <summary>
        /// Metoda do wywołania zdarzenia do zażądania odświeżenia widoku pracowników
        /// </summary>
        void RaiseRequestEmployeesRefresh();

        /// <summary>
        /// Zdarzenie do wysłania żądania odświeżenia widoku uczniów
        /// </summary>
        event Action RequestStudentsRefresh;

        /// <summary>
        /// Metoda do wywołania zdarzenia do zażądania odświeżenia widoku uczniów
        /// </summary>
        void RaiseRequestStudentsRefresh();

        /// <summary>
        /// Zdarzenie do wysłania żądania odświeżenia widoku listy słownikowej
        /// </summary>
        event Action<string> RequestDictionaryListRefresh;

        /// <summary>
        /// Metoda do wywołania zdarzenia do zażądania odświeżenia widoku określonego typu
        /// </summary>
        /// <param name="dictionaryType">Typ listy która ma zostać odświeżona.</param>
        void RaiseRequestDictionaryListRefresh(string dictionaryType);

        public event Action RequestAddressesRefresh;

        public void RaiseRequestAddressesRefresh();
    }
}