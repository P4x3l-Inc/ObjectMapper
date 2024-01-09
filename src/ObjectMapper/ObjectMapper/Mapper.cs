using ObjectMapper.Models;
using System.Dynamic;
using System.Xml;
using Newtonsoft.Json;

namespace ObjectMapper
{
    public static class Mapper
    {
        public static dynamic Map(dynamic source, IDictionary<string, MappingDescription> propertyMappings)
        {
            dynamic destination = new ExpandoObject();

            foreach (var mapping in propertyMappings)
            {
                var sourcePropertyName = mapping.Key;
                var destinationPropertyName = mapping.Value;

                if (destinationPropertyName.Type == "Array")
                {
                    var values = destinationPropertyName.List;
                    var result = new List<object>();
                    foreach (var val in values)
                    {
                        var propDepth = val.Split(".").ToList();
                        if (propDepth.Count() > 1)
                        {
                            var parent = source.GetType().GetProperty(propDepth[0])?.GetValue(source);

                            var value = parent.GetType().GetProperty(propDepth[1])?.GetValue(parent);

                            result.Add(value);
                        }
                        else
                        {
                            result.Add(source.GetType().GetProperty(val)?.GetValue(source));
                        }
                    }

                    ((IDictionary<string, object>)destination)[sourcePropertyName.ToString()] = string.Join(", ", result);
                }
                else
                {
                    var propDepth = (string[])destinationPropertyName.Value.ToString().Split(".");
                    if (propDepth.Count() > 1)
                    {
                        var parent = source.GetType().GetProperty(propDepth[0])?.GetValue(source);

                        var value = parent.GetType().GetProperty(propDepth[1])?.GetValue(parent);

                        ((IDictionary<string, object>)destination)[sourcePropertyName.ToString()] = value;
                    }
                    else
                    {
                        ((IDictionary<string, object>)destination)[sourcePropertyName.ToString()] = source.GetType().GetProperty(destinationPropertyName.Value.ToString())?.GetValue(source);
                    }
                }
            }

            return destination;
        }

        public static object MapXml(dynamic source, string xml)
        {
            var obj = XmlToJson(xml);

            return Map(source, obj);
        }

        static IDictionary<string, MappingDescription> XmlToJson(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            string jsonString = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

            var data = JsonConvert.DeserializeObject<XmlSource>(jsonString);

            return data.Data;
        }
    }
}