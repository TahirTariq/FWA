using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http;

namespace FWAMovies
{
    public static class FormattersConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                PreserveReferencesHandling = PreserveReferencesHandling.None             
            };

            //Forces JSON Response
            formatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;
        }
    }
}