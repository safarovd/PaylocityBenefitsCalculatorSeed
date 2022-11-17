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
            string openStream = File.ReadAllText(fileName);
            T item = JsonSerializer.Deserialize<T>(openStream, options)!;
            return item;
        }
    }
}
