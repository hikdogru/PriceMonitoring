using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PriceMonitoring.WebUI.Models
{
    public static class JsonModel
    {
        public static string SerializeObject(object value, JsonSerializerSettings settings = null)
        {
           return JsonConvert.SerializeObject(value, Formatting.Indented, settings == null ? new JsonSerializerSettings
           {
               PreserveReferencesHandling = PreserveReferencesHandling.Objects,
               ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
           }: settings);
        }
    }
}
