using CalculatorAPI.Contracts;
using CalculatorAPI.Helper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CalculatorAPI.Resources
{
    public class CalculatorResource:ICalculatorResource
    {
        public async Task<ProbabilityResult> CreateAsync( ProbabilityRequest request)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            try { 
            double result = request.Operation.ToString() switch
            {

                nameof(OperationType.CombinedWith) => request.PA * request.PB,
                nameof(OperationType.Either) => request.PA + request.PB - (request.PA * request.PB),
                _ => throw new ArgumentOutOfRangeException(nameof(request.Operation))
            };
           
            return await Task.FromResult(
                
                new ProbabilityResult
                {
                PA = request.PA,
                PB = request.PB,
                operation = request.Operation.ToString(),
                Result = result
                });
        }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to calculate probability", ex);
            }
        }
           
}
}
