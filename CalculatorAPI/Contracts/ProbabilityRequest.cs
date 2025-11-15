using CalculatorAPI.Helper;
using System.ComponentModel.DataAnnotations;

namespace CalculatorAPI.Contracts
{

    public class ProbabilityRequest
    {
        //[Range(0.0, 1.0, ErrorMessage = "PA must be between 0.0 and 1.0")]
        public double PA { get; set; }

        //[Range(0.0, 1.0, ErrorMessage = "PB must be between 0.0 and 1.0")]
        public double PB { get; set; }

        [Required]
        public string Operation
        {
            get;set;

        }

    }
}
