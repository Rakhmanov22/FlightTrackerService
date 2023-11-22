using FluentValidation;

namespace FlightStatusControlAPI.Models
{
    public class FlightValidator : AbstractValidator<Flight>
    {
        public FlightValidator()
        {
            RuleFor(f => f.Origin).NotEmpty().WithMessage("Origin is required");
            RuleFor(f => f.Destination).NotEmpty().WithMessage("Destination is required");
        }
    }
}
