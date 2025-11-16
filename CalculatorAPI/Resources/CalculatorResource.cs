using CalculatorAPI.Contracts;
using CalculatorAPI.Helper;

namespace CalculatorAPI.Resources
{
    public class CalculatorResource:ICalculatorResource
    {
        private readonly ILogger<CalculatorResource>? logger;
        public CalculatorResource(ILogger<CalculatorResource>? logger = null)
        {
            this.logger = logger;
        }

        public async Task<ProbabilityResult> CreateAsync( ProbabilityRequest request)
        {
            logger?.LogInformation("CreateAsync called with PA={PA}, PB={PB}, Operation={Operation}", request.PA, request.PB, request.Operation);
            if (request.PA < 0 || request.PA > 1 || request.PB < 0 || request.PB > 1)
            {
                logger?.LogWarning("CreateAsync received PA or PB outside [0,1] range: PA={PA}, PB={PB}", request.PA, request.PB);
            }

            try { 
            double result = request.Operation.ToString() switch
            {

                nameof(OperationType.CombinedWith) => request.PA * request.PB,
                nameof(OperationType.Either) => request.PA + request.PB - (request.PA * request.PB),
                _ => throw new ArgumentOutOfRangeException(nameof(request.Operation))
            };
           
            var probabilityResult = new ProbabilityResult
            {
                PA = request.PA,
                PB = request.PB,
                operation = request.Operation.ToString(),
                Result = result
            };

                logger?.LogInformation("Calculation completed. Operation={Operation}, Result={Result}", probabilityResult.operation, probabilityResult.Result);

                return await Task.FromResult(probabilityResult);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to calculate probability for PA={PA}, PB={PB}, Operation={Operation}", request.PA, request.PB, request.Operation);
                throw new InvalidOperationException("Failed to calculate probability", ex);
            }
        }
           
}
}
