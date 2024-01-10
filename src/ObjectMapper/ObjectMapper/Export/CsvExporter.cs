using CsvHelper;
using CsvHelper.Configuration;
using DynamicObjectMapper.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicObjectMapper.Export
{
    public class CsvExporter
    {
        public byte[] Export(IDictionary<string, MappingDescription> propertyMappings, IList<dynamic> data, bool includeHeaders = false)
        {
            var dataToExport = data.Select(x => Mapper.Map(x, propertyMappings)).ToList();

            // Create a CSV writer configuration
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);

            // Create a memory stream to hold the CSV data
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                if (includeHeaders)
                {
                    csv.Context.RegisterClassMap<DynamicClassMap>();
                }
                // Write records to CSV
                csv.WriteRecords<dynamic>(dataToExport);

                // Flush the writer and get the byte array
                writer.Flush();
                return memoryStream.ToArray();
            }
        }
    }

    public class DynamicClassMap : ClassMap<ExpandoObject>
    {
        public DynamicClassMap()
        {
            // Assuming dynamic objects have properties like Name, Age, City
            Map(x => ((IDictionary<string, object>)x)["Name"]).Name("Name");
            Map(x => ((IDictionary<string, object>)x)["Age"]).Name("Age");
            Map(x => ((IDictionary<string, object>)x)["City"]).Name("City");
        }
    }
}
