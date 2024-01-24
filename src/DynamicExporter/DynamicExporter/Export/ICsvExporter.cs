using DynamicExporter.Models;
using System.Collections.Generic;

namespace DynamicExporter.Export
{
    public interface ICsvExporter
    {
        byte[] Export(IDictionary<string, MappingDescription> propertyMappings, IList<dynamic> data, bool includeHeaders = false, DelimiterType delimiterType = DelimiterType.Comma);

        byte[] Export(string xml, IList<dynamic> data, bool includeHeaders = false, DelimiterType delimiterType = DelimiterType.Comma);
    }
}
