using CalculatorAPI.Contracts;

namespace CalculatorAPI.Resources
{
    public interface ICalculatorResource
    {
         Task<ProbabilityResult> CreateAsync(ProbabilityRequest request);
    }
}
