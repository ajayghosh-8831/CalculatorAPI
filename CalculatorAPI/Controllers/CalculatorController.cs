using CalculatorAPI.Contracts;
using CalculatorAPI.Helper;
using CalculatorAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorResource calculatorResource;
        private readonly ILogger<CalculatorController> logger;

        public CalculatorController(ICalculatorResource calculatorResource, ILogger<CalculatorController> logger)
        {
            this.calculatorResource = calculatorResource;
            this.logger = logger;
        }
        // GET: api/<CalculatorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            logger.LogInformation("GET api/Calculator called");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Calculate probabilities.
        /// POST /api/Calculator/calculate
        /// </summary>
        [HttpPost("calculate")]
        public async Task<ActionResult<ProbabilityResult>>  Calculate([FromBody] ProbabilityRequest request)
        {
            try
            {
                logger.LogInformation("Calculate called with PA={PA}, PB={PB}, Operation={Operation}", request.PA, request.PB, request.Operation);

                var result = await this.calculatorResource.CreateAsync(request);

                logger.LogInformation("Calculation completed. Result={Result}", result?.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while calculating probabilities");
                return BadRequest(new { error = ex.Message });
            }


        }
    }
}
