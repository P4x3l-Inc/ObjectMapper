namespace ObjectMapper.Models
{
    public class MappingDescription
    {
        public string Type { get; set; }

        public dynamic Value { get; set; }

        public List<dynamic> List { get; set; }
    }
}
