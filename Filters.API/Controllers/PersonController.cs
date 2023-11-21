using Filters.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Filters.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IGetterByFiltersService _getterByFiltersService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IGetterByFiltersService service, ILogger<PersonController> logger)
        {
            _getterByFiltersService = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] char? gender, [FromQuery] string? name)
        {
            try
            {
                var result = await _getterByFiltersService.GetPersonsByFiltersAsync(gender, name);
                _logger.LogInformation("Successfully retrieved filtered names.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving names.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
