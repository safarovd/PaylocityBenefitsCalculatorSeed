using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.BenefitsServices.BenefitsHelper
{
    public class JsonLoader
    {   
        // Make the JsonLoader generic so that we can load json into any class type.
        public T LoadJson<T>(string fileName)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            string json = File.ReadAllText(fileName);
            T item = JsonSerializer.Deserialize<T>(json, options)!;
            return item;
        }

        public T WriteJson<T>(T data, string fileName)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<T>(data, options);
            File.WriteAllText(fileName, json);
            return data;
        }
    }
}
