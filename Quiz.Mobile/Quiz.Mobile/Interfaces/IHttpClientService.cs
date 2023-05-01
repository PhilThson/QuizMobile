using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quiz.Mobile.Shared.DTOs;

namespace Quiz.Mobile.Interfaces
{
	public interface IHttpClientService
	{
        Task<List<T>> GetAllItems<T>();
        Task<T> GetItemById<T>(object id);
        Task<T> GetItemByKey<T>(string key, string value);
        Task RemoveItemById<T>(object id);
        Task<object> AddItem<T>(T item, string dict = null);
        Task UpdateItem<T>(T item, string dict = null);
        Task<IEnumerable<string>> Login(SimpleUserDto simpleUser);
    }
}