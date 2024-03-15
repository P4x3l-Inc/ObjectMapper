using System.Collections.Generic;

namespace DynamicExporter.Models
{
    public class MappingDescription
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public List<string> List { get; set; }

        public string Header { get; set; }
    }
}
