using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using TodoList.UWP.Models;
using Windows.Web.Http;
using Newtonsoft.Json;

namespace TodoList.UWP.Data
{
    public class DataRepository : IDataRepository
    {
        private const string endpoint = "http://localhost:63644/";

        public async Task<DataFeed> GetFeedAsync()
        {
            using (var client = new HttpClient())
            {
                var str = await client.GetStringAsync(GetUri("api/items"));
                return JsonConvert.DeserializeObject<DataFeed>(str);
            }
        }

        public Task<List<Operation>> GetOperationsAsync(Guid? lastOperationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Operation>> PostOperationsAsync(Guid? lastOperationId, Operation operation)
        {
            using (var client = new HttpClient())
            {
                var data = JsonConvert.SerializeObject(operation);
                using (var content = new HttpStringContent(data, UnicodeEncoding.Utf8, "application/json"))
                {
                    var uri = string.Format("api/operations?lastOperationId={0}", lastOperationId);
                    var resp = await client.PostAsync(GetUri(uri), content);
                    if (resp.StatusCode == HttpStatusCode.Created)
                    {
                        var str = await resp.Content.ReadAsStringAsync();
                        var operations = JsonConvert.DeserializeObject<List<Operation>>(str);
                        return operations;
                    }
                    throw new Exception("Server side error");
                }
            }
        }

        private static Uri GetUri(string path)
        {
            return new Uri(string.Concat(endpoint, path), UriKind.Absolute);
        }
    }
}
