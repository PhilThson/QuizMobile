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

        void RaiseRequestEmployeesRefresh();
    }
}