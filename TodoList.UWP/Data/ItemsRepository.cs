using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Newtonsoft.Json;
using TodoList.UWP.Models;

namespace TodoList.UWP.Data
{
    public class ItemsRepository : IItemsRepository
    {
        private const string endpoint = "http://localhost:63644/";

        public async Task<List<Item>> GetAsync()
        {
            using (var client = new HttpClient())
            {
                var str = await client.GetStringAsync(GetUri("api/items"));
                return JsonConvert.DeserializeObject<List<Item>>(str);
            }
        }

        public async Task CreateAsync(Item item)
        {
            using (var client = new HttpClient())
            {
                var data = JsonConvert.SerializeObject(item);
                using (var content = new HttpStringContent(data, UnicodeEncoding.Utf8, "application/json"))
                {
                    var resp = await client.PostAsync(GetUri("api/items"), content);
                    if (resp.StatusCode == HttpStatusCode.Created)
                    {
                        var str = await resp.Content.ReadAsStringAsync();
                        var guid = JsonConvert.DeserializeObject<Guid>(str);
                        item.Guid = guid;
                    }
                }
            }
        }

        public Task SetIsDoneAsync(Guid guid, bool isDone)
        {
            return Task.FromResult<object>(null);
        }

        private Uri GetUri(string path)
        {
            return new Uri(string.Concat(endpoint, path), UriKind.Absolute);
        }
    }
}
