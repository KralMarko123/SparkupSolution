using Xunit;
using AutoMapper;
using SparkUpSolution.Application.Mapping;

namespace SparkUpSolution.Tests.Unit
{
    public class MappingTests
    {

        public MappingTests() { }

        [Fact]
        public void validate_mapping_configuration()
        {
            // Arrange
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new BonusProfile()));

            // Act

            // Assert
            mapperConfig.AssertConfigurationIsValid();
        }
    }
}
