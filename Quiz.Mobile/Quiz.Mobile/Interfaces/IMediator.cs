using System;
using Quiz.Mobile.ViewModels.Abstract;

namespace Quiz.Mobile.Interfaces
{
    public interface IMediator
    {
        static IMediator Instance { get; }

        event Action RequestEmployeesRefresh;

        void RaiseRequestEmployeesRefresh();
    }
}