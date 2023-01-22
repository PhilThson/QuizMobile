using System;
using Quiz.Mobile.Interfaces;

namespace Quiz.Mobile.ViewModels.Abstract
{
    public class Mediator : IMediator
    {
        private static IMediator _Instance;
        public static IMediator Instance
        {
            get => _Instance ??= new Mediator();
        }

        private Mediator()
        {

        }

        public event Action RequestEmployeesRefresh;

        public void RaiseRequestEmployeesRefresh() =>
            RequestEmployeesRefresh?.Invoke();

        public event Action RequestStudentsRefresh;

        public void RaiseRequestStudentsRefresh() =>
            RequestStudentsRefresh?.Invoke();

        public event Action<string> RequestDictionaryListRefresh;

        public void RaiseRequestDictionaryListRefresh(string dictionaryType) =>
            RequestDictionaryListRefresh?.Invoke(dictionaryType);
    }
}

