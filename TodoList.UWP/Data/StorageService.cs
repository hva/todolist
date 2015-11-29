using System;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using TodoList.UWP.Data.Interfaces;

namespace TodoList.UWP.Data
{
    public class StorageService : IStorageService
    {
        private static readonly StorageFolder folder = ApplicationData.Current.LocalFolder;

        public async Task<T> LoadAsync<T>(string fileName) where T : class
        {
            var content = await ReadAsync(fileName);

            if (content != null)
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }

        public Task SaveAsync<T>(T items, string fileName) where T : class
        {
            var content = JsonConvert.SerializeObject(items, Formatting.Indented);
            return WriteAsync(content, fileName);
        }

        private async static Task<string> ReadAsync(string fileName)
        {
            var item = await folder.TryGetItemAsync(fileName);
            if (item == null || !item.IsOfType(StorageItemTypes.File))
            {
                return null;
            }

            var file = (StorageFile)item;

            return await FileIO.ReadTextAsync(file);
        }

        private async static Task WriteAsync(string content, string fileName)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }
    }
}
