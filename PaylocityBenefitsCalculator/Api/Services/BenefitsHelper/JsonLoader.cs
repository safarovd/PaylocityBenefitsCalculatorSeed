using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Services.BenefitsHelper
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
            // setup the option to convert strings to enum
            var stringEnumConverter = new JsonStringEnumConverter();
            options.Converters.Add(stringEnumConverter);
            // read in all the json
            string json = File.ReadAllText(fileName);
            // deserialize the json file into our generic type
            T item = JsonSerializer.Deserialize<T>(json, options)!;
            return item;
        }

        public T WriteJson<T>(T data, string fileName)
        {
            // prettify the json string
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize<T>(data, options);
            // write updated data to our Json file
            File.WriteAllText(fileName, json);
            return data;
        }
    }
}
