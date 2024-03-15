using CsvHelper;
using CsvHelper.Configuration;
using DynamicExporter.Helpers;
using DynamicExporter.Mappers;
using DynamicExporter.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DynamicExporter.Export
{
    public class CsvExporter : ICsvExporter
    {
        public byte[] Export(IDictionary<string, MappingDescription> propertyMappings, IList<dynamic> data, bool includeHeaders = false, DelimiterType delimiterType = DelimiterType.Comma)
        {
            var dataToExport = data.Select(x => DynamicMapper.Map(x, propertyMappings)).ToList();

            // Create a CSV writer configuration
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
            if (delimiterType == DelimiterType.Tab)
            {
                csvConfig.Delimiter = "\t";
            }
            else if (delimiterType == DelimiterType.Semicolon)
            {
                csvConfig.Delimiter = ";";
            }
            else
            {
                csvConfig.Delimiter = ",";
            }

            // Create a memory stream to hold the CSV data
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                if (includeHeaders)
                {
                    var classMap = new DynamicClassMap(propertyMappings);
                    csv.Context.RegisterClassMap(classMap);
                }
                // Write records to CSV
                csv.WriteRecords<dynamic>(dataToExport);

                // Flush the writer and get the byte array
                writer.Flush();
                return memoryStream.ToArray();
            }
        }

        public byte[] Export(string xml, IList<dynamic> data, bool includeHeaders = false, DelimiterType delimiterType = DelimiterType.Comma)
        {
            var propertyMappings = XmlHelper.XmlToMappings(xml);

            return Export(propertyMappings, data, includeHeaders, delimiterType);
        }
    }

    public class DynamicClassMap : ClassMap<ExpandoObject>
    {
        private IDictionary<string, MappingDescription> _propertyMappings;

        public DynamicClassMap(IDictionary<string, MappingDescription> propertyMappings)
        {
            _propertyMappings = propertyMappings;

            foreach (var property in _propertyMappings)
            {
                var header = !string.IsNullOrEmpty(property.Value.Header) ?
                    property.Value.Header :
                    property.Key;

                Map<object>(row => GetPropertyValue((ExpandoObject)row, property.Key)).Name(header);
            }
        }

        private object GetPropertyValue(ExpandoObject obj, string propertyName)
        {
            // Dynamically retrieve property value using reflection
            IDictionary<string, object> expando = obj;
            if (expando.ContainsKey(propertyName))
            {
                return expando[propertyName];
            }
            return null;
        }
    }
}
