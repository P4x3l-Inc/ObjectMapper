using System.Collections.Generic;

namespace DynamicObjectMapper.Models
{
    public class MappingDescription
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public List<string> List { get; set; }
    }
}
