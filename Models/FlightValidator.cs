using FluentValidation;

namespace FlightStatusControlAPI.Models
{
    public class FlightValidator : AbstractValidator<Flight>
    {
        public FlightValidator()
        {
            RuleFor(f => f.Destination).NotEmpty().WithMessage("Destination is required");
        }
    }
}
