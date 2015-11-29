using System.Threading.Tasks;

namespace TodoList.UWP.Data.Interfaces
{
    public interface IStorageService
    {
        Task<T> LoadAsync<T>(string fileName) where T : class;
        Task SaveAsync<T>(T list, string fileName) where T : class;
    }
}
