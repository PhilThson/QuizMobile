using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.ViewModels.Abstract
{
    public class Mediator : IMediator
    {
        private static IMediator _Instance;
        public static IMediator Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new Mediator();
                return _Instance;
            }
        }

        private Mediator()
        {

        }

        /// <summary>
        /// Zdarzenie do wysłania żądania odświeżenia widoku pracowników
        /// </summary>
        public event Action RequestEmployeesRefresh;

        public void RaiseRequestEmployeesRefresh() =>
            RequestEmployeesRefresh?.Invoke();

        public event Action<string> RequestDictionaryListRefresh;

        public void RaiseRequestDictionaryListRefresh(string dictionaryType) =>
            RequestDictionaryListRefresh?.Invoke(dictionaryType);
    }
}

