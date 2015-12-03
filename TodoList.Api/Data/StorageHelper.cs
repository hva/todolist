using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace TodoList.Api.Data
{
    // StorageHelper is used to read/write data from/to file system.
    // File name is mapped to web site's App_Data folder.
    public class StorageHelper
    {
        public static T Read<T>(string fileName) where T : class
        {
            var path = GetAbsolutePath(fileName);
            if (!File.Exists(path))
            {
                return null;
            }
            using (var file = File.OpenText(path))
            {
                var serializer = new JsonSerializer();
                return (T)serializer.Deserialize(file, typeof(T));
            }
        }

        public static void Write<T>(T data, string fileName) where T : class
        {
            var path = GetAbsolutePath(fileName);
            using (var file = File.CreateText(path))
            {
                var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(file, data);
            }
        }

        private static string GetAbsolutePath(string fileName)
        {
            return HostingEnvironment.MapPath(@"~/App_Data/" + fileName);
        }
    }
}
