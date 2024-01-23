using DynamicObjectMapper.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml;

namespace DynamicObjectMapper.Helpers
{
    internal static class XmlHelper
    {
        internal static IDictionary<string, MappingDescription> XmlToMappings(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            string jsonString = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

            var data = JsonConvert.DeserializeObject<XmlSource>(jsonString);

            return data != null ? data.Data : new Dictionary<string, MappingDescription>();
        }
    }
}
