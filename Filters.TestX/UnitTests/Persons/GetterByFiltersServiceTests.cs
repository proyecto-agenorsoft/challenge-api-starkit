using Filters.Contracts;
using Filters.Domain;
using Filters.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Filters.Test.UnitTests.Persons
{
    public class GetterByFiltersServiceTests
    {
        private readonly Mock<IGetterAllRepository> _mockRepository;
        private readonly GetterByFiltersService _service;
        private readonly Mock<ILogger<GetterByFiltersService>> _mockLogger;

        public GetterByFiltersServiceTests()
        {
            _mockRepository = new Mock<IGetterAllRepository>();
            _mockLogger = new Mock<ILogger<GetterByFiltersService>>();
            _service = new GetterByFiltersService(_mockRepository.Object, _mockLogger.Object);
        }

        /// <summary>
        /// Obtiene todos los registros personas
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Person> GetTestNames()
        {
            return new List<Person>
            {
                new Person { Name = "Alejandro", Gender = 'M' },
                new Person { Name = "Alicia", Gender = 'F' },
            };
        }

        #region Exitoso

        /// <summary>
        /// Prueba caso en el que se reciba solo el genero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFilteredNames_WithGenderFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var testNames = GetTestNames();
            _mockRepository.Setup(repo => repo.GetAllPersonAsync())
                           .ReturnsAsync(testNames);

            // Act
            var results = await _service.GetPersonsByFiltersAsync('M', null);

            // Assert
            Assert.All(results, person => Assert.Equal('M', person.Gender));
        }

        /// <summary>
        /// Prueba caso en el que se reciba solo el nombre o parte del nombre
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFilteredNames_WithStartsWithFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var testNames = GetTestNames();
            _mockRepository.Setup(repo => repo.GetAllPersonAsync())
                           .ReturnsAsync(testNames);

            // Act
            var results = await _service.GetPersonsByFiltersAsync(null, "Al");

            // Assert
            Assert.All(results, person => Assert.StartsWith("Al", person.Name));
        }

        /// <summary>
        /// Prueba el caso en que no se reciba ni el genero ni el nombre
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFilteredNames_WithNoFilters_ShouldReturnAllResults()
        {
            // Arrange
            var testNames = GetTestNames();
            _mockRepository.Setup(repo => repo.GetAllPersonAsync())
                           .ReturnsAsync(testNames);

            // Act
            var results = await _service.GetPersonsByFiltersAsync(null, null);

            // Assert
            Assert.Equal(testNames.Count(), results.Count());
        }

        #endregion

        #region Fallas

        /// <summary>
        /// Prueba de un error al obtener personas 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPersonsByFilters_WhenRepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Simula excepcion del repositorio");
            _mockRepository.Setup(repo => repo.GetAllPersonAsync())
                           .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetPersonsByFiltersAsync(null, null));
            Assert.Equal(expectedException.Message, exception.Message);
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        /// <summary>
        /// Prueba de un resultado vacío o nulo desde el repositorio
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPersonsByFilters_WhenRepositoryReturnsNull_ShouldReturnEmptyCollection()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllPersonAsync())
                           .ReturnsAsync(new List<Person>());

            // Act
            var results = await _service.GetPersonsByFiltersAsync(null, null);

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        #endregion
    }
}
