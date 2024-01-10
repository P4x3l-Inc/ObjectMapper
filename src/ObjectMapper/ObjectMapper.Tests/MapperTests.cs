using Newtonsoft.Json;
using DynamicObjectMapper.Models;
using System.Reflection.Emit;
using System.Text.Json;
using DynamicObjectMapper.Tests.TestModels;

namespace DynamicObjectMapper.Tests
{
    public class MapperTests
    {
        [Fact]
        public void Map_ShouldMapObject()
        {
            // Arrange
            var jsonMapping = "{\"Personnr\": { \"Type\": \"String\", \"Value\": \"StudentSSN\" }," +
                "\"Adress\": { \"Type\": \"String\", \"Value\": \"Address\"}," +
                "\"Name\": { \"Type\": \"Array\", \"List\": [\"Firstname\", \"Lastname\"]}," +
                "\"Postnr\": { \"Type\": \"String\", \"Value\": \"ZipCode\"}," +
                "\"Padress\": { \"Type\": \"String\", \"Value\": \"City\"}," +
                "\"Arskurs\": { \"Type\": \"String\", \"Value\": \"Grade\"}" +
                "}";

            var propertyMappings = JsonConvert.DeserializeObject<Dictionary<string, MappingDescription>>(jsonMapping);

            dynamic sourceApplication = new
            {
                StudentSSN = "860803-0097",
                Address = "Address 123",
                Firstname = "Tomas",
                Lastname = "Svensson",
                ZipCode = "67133",
                City = "Arvika",
                Grade = 5
            };

            // Act
            var result = Mapper.Map(sourceApplication, propertyMappings);

            // Assert
            ((string)result.Personnr).Should().Be(sourceApplication.StudentSSN);
            ((string)result.Adress).Should().Be(sourceApplication.Address);
            ((string)result.Name).Should().Be($"{sourceApplication.Firstname}, {sourceApplication.Lastname}");
            ((string)result.Postnr).Should().Be(sourceApplication.ZipCode);
            ((string)result.Padress).Should().Be(sourceApplication.City);
            ((int)result.Arskurs).Should().Be(sourceApplication.Grade);
        }

        [Fact]
        public void MapXml_ShouldMapObject()
        {
            // Arrange
            var xml = @"<?xml version='1.0' encoding='UTF-8'?>
                        <Data>
                            <Personnr>
                                <Type>String</Type>
                                <Value>StudentSSN</Value>
                            </Personnr>
                            <Adress>
                                <Type>String</Type>
                                <Value>Address</Value>
                            </Adress>
                            <Name>
                                <Type>Array</Type>
                                <List>Firstname</List>
                                <List>Lastname</List>
                            </Name>
                            <Postnr>
                                <Type>String</Type>
                                <Value>ZipCode</Value>
                            </Postnr>
                            <Padress>
                                <Type>String</Type>
                                <Value>City</Value>
                            </Padress>
                            <Arskurs>
                                <Type>String</Type>
                                <Value>Grade</Value>
                            </Arskurs>
                         </Data>";

            dynamic sourceApplication = new
            {
                StudentSSN = "860803-0097",
                Address = "Address 123",
                Firstname = "Tomas",
                Lastname = "Svensson",
                ZipCode = "67133",
                City = "Arvika",
                Grade = 5
            };

            // Act
            var result = Mapper.MapXml(sourceApplication, xml);

            // Assert
            ((string)result.Personnr).Should().Be(sourceApplication.StudentSSN);
            ((string)result.Adress).Should().Be(sourceApplication.Address);
            ((string)result.Name).Should().Be($"{sourceApplication.Firstname}, {sourceApplication.Lastname}");
            ((string)result.Postnr).Should().Be(sourceApplication.ZipCode);
            ((string)result.Padress).Should().Be(sourceApplication.City);
            ((int)result.Arskurs).Should().Be(sourceApplication.Grade);
        }

        [Fact]
        public void Map_ShouldMapNestedObject_WhenSourceIsDynamic()
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

            dynamic sourceApplication = new
            {
                StudentSSN = "860803-0097",
                Address = "Address 123",
                Firstname = "Tomas",
                Lastname = "Svensson",
                ZipCode = "67133",
                City = "Arvika",
                Grade = 5
            };

            dynamic sourceSchool = new
            {
                School = "84DO",
            };

            dynamic sourceMuniciaplity = new
            {
                AreaCode = 84,
                County = "Arvika",
                CountyCode = 17
            };

            dynamic source = new
            {
                application = sourceApplication,
                school = sourceSchool,
                municiaplity = sourceMuniciaplity,
            };

            // Act
            var result = Mapper.Map(source, propertyMappings);

            // Assert
            ((string)result.Personnr).Should().Be(sourceApplication.StudentSSN);
            ((string)result.Adress).Should().Be(sourceApplication.Address);
            ((string)result.Name).Should().Be($"{sourceApplication.Firstname}, {sourceApplication.Lastname}");
            ((string)result.Postnr).Should().Be(sourceApplication.ZipCode);
            ((string)result.Padress).Should().Be(sourceApplication.City);
            ((int)result.Kommunnr).Should().Be(sourceMuniciaplity.AreaCode);
            ((string)result.Omrade).Should().Be(sourceMuniciaplity.County);
            ((int)result.Lan).Should().Be(sourceMuniciaplity.CountyCode);
            ((int)result.Arskurs).Should().Be(sourceApplication.Grade);
            ((string)result.Skola).Should().Be(sourceSchool.School);
        }

        [Fact]
        public void Map_ShouldMapNestedObject_WhenSourceIsTyped()
        {
            // Arrange
            var jsonMapping = "{\"Personnr\": { \"Type\": \"String\", \"Value\": \"Application.StudentSSN\" }," +
                "\"Adress\": { \"Type\": \"String\", \"Value\": \"Application.Address\"}," +
                "\"Name\": { \"Type\": \"Array\", \"List\": [\"Application.Firstname\", \"Application.Lastname\"]}," +
                "\"Postnr\": { \"Type\": \"String\", \"Value\": \"Application.ZipCode\"}," +
                "\"Padress\": { \"Type\": \"String\", \"Value\": \"Application.City\"}," +
                "\"Kommunnr\": { \"Type\": \"String\", \"Value\": \"Municiaplity.AreaCode\"}," +
                "\"Omrade\": { \"Type\": \"String\", \"Value\": \"Municiaplity.County\"}," +
                "\"Lan\": { \"Type\": \"String\", \"Value\": \"Municiaplity.CountyCode\"}," +
                "\"Arskurs\": { \"Type\": \"String\", \"Value\": \"Application.Grade\"}," +
                "\"Skola\": { \"Type\": \"String\", \"Value\": \"School.SchoolName\"}" +
                "}";

            var propertyMappings = JsonConvert.DeserializeObject<Dictionary<string, MappingDescription>>(jsonMapping);

            var sourceApplication = new Application
            {
                StudentSSN = "860803-0097",
                Address = "Address 123",
                Firstname = "Tomas",
                Lastname = "Svensson",
                ZipCode = "67133",
                City = "Arvika",
                Grade = 5
            };

            var sourceSchool = new School
            {
                SchoolName = "84DO",
            };

            var sourceMuniciaplity = new Municiaplity
            {
                AreaCode = 84,
                County = "Arvika",
                CountyCode = 17
            };

            var source = new Source
            {
                Application = sourceApplication,
                School = sourceSchool,
                Municiaplity = sourceMuniciaplity,
            };

            // Act
            var result = Mapper.Map(source, propertyMappings);

            // Assert
            ((string)result.Personnr).Should().Be(sourceApplication.StudentSSN);
            ((string)result.Adress).Should().Be(sourceApplication.Address);
            ((string)result.Name).Should().Be($"{sourceApplication.Firstname}, {sourceApplication.Lastname}");
            ((string)result.Postnr).Should().Be(sourceApplication.ZipCode);
            ((string)result.Padress).Should().Be(sourceApplication.City);
            ((int)result.Kommunnr).Should().Be(sourceMuniciaplity.AreaCode);
            ((string)result.Omrade).Should().Be(sourceMuniciaplity.County);
            ((int)result.Lan).Should().Be(sourceMuniciaplity.CountyCode);
            ((int)result.Arskurs).Should().Be(sourceApplication.Grade);
            ((string)result.Skola).Should().Be(sourceSchool.SchoolName);
        }
    }
}