using System.Text.Json;

namespace ObjectMapper.Extensions
{
    public static class JsonElementExtensions
    {
        public static List<T> ToList<T>(this JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                List<T> list = new List<T>();
                foreach (var element in jsonElement.EnumerateArray())
                {
                    // Use JsonSerializer to deserialize each element to the desired type
                    T item = JsonSerializer.Deserialize<T>(element.GetRawText());
                    list.Add(item);
                }
                return list;
            }
            else
            {
                throw new InvalidOperationException("JsonElement is not an array.");
            }
        }
    }
}
