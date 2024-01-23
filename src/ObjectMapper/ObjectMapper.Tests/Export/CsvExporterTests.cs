using Newtonsoft.Json;
using DynamicObjectMapper.Export;
using DynamicObjectMapper.Models;

namespace DynamicObjectMapper.Tests.Export
{
    public class CsvExporterTests
    {
        [Fact]
        public void Export_ShouldExportData()
        {
            // Arrange
            var jsonMapping = "{\"Personnr\": { \"Type\": \"String\", \"Value\": \"application.StudentSSN\" }," +
                "\"Adress\": { \"Type\": \"String\", \"Value\": \"application.Address\"}," +
                "\"Name\": { \"Type\": \"Array\", \"List\": [\"application.Firstname\", \"application.Lastname\"]}," +
                "\"Postnr\": { \"Type\": \"String\", \"Value\": \"application.ZipCode\"}," +
                "\"Padress\": { \"Type\": \"String\", \"Value\": \"application.City\"}," +
                "\"Kommunnr\": { \"Type\": \"String\", \"Value\": \"municiaplity.AreaCode\"}," +
                "\"Omrade\": { \"Type\": \"String\", \"Value\": \"municiaplity.County\"}," +
                "\"Lan\": { \"Type\": \"String\", \"Value\": \"municiaplity.CountyCode\"}," +
                "\"Arskurs\": { \"Type\": \"String\", \"Value\": \"application.Grade\"}," +
                "\"Skola\": { \"Type\": \"String\", \"Value\": \"school.School\"}" +
                "}";

            var propertyMappings = JsonConvert.DeserializeObject<Dictionary<string, MappingDescription>>(jsonMapping);

            dynamic source = new List<dynamic> {
                new
                {
                    application = new
                    {
                        StudentSSN = "860803-0097",
                        Address = "Address 123",
                        Firstname = "Tomas",
                        Lastname = "Svensson",
                        ZipCode = "67133",
                        City = "Arvika",
                        Grade = 5
                    },
                    school = new
                    {
                        School = "84DO",
                    },
                    municiaplity = new
                    {
                        AreaCode = 84,
                        County = "Arvika",
                        CountyCode = 17
                    },
                },
                new
                {
                    application = new
                    {
                        StudentSSN = "900803-1234",
                        Address = "Other Address 13",
                        Firstname = "Sven",
                        Lastname = "Tomasson",
                        ZipCode = "67132",
                        City = "Arvika",
                        Grade = 7
                    },
                    school = new
                    {
                        School = "84DO",
                    },
                    municiaplity = new
                    {
                        AreaCode = 84,
                        County = "Arvika",
                        CountyCode = 17
                    },
                },
            };

            var exporter = new CsvExporter();

            // Act
            var result = exporter.Export(propertyMappings, source);

            // Assert
            File.WriteAllBytes(Environment.CurrentDirectory + "/test.csv", result);
        }
    }
}
