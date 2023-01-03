using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Quiz.Mobile.Interfaces
{
	public interface IHttpClientService
	{
        Task<List<T>> GetAllItems<T>();
        Task<T> GetItemById<T>(object id);
        Task RemoveItemById<T>(object id);
        Task AddItem<T>(T item, string dict = null);
    }
}