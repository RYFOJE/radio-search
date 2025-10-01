using FluentValidation;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Services.Validators
{
    public class QueryValidator : AbstractValidator<LicenseQueryOptions>
    {

        public QueryValidator()
        {
            When(x => x.LocationInformation != null, () =>
            {
                RuleFor(x => x.LocationInformation!.Latitude).InclusiveBetween(-90, 90)
                    .NotNull()
                    .WithMessage("Latitude must be between -90 and 90.");
                
                RuleFor(x => x.LocationInformation!.Longitude).InclusiveBetween(-180, 180)
                    .NotNull()
                    .WithMessage("Longitude must be between -180 and 180.");
                
                RuleFor(x => x.LocationInformation!.RadiusInMeters).GreaterThan(0)
                    .NotNull()
                    .WithMessage("RadiusInMeters must be greater than 0.");
            });

            RuleFor(x => x.frequencyMin)
                .GreaterThanOrEqualTo(0)
                .When(x => x.frequencyMin.HasValue)
                .WithMessage("frequencyMin must be greater than or equal to 0.");

            RuleFor(x => x.AnalogDigital)
                .Must(c => char.ToUpper(c!.Value) == 'A' || c == 'D')
                .When(x => x.AnalogDigital.HasValue)
                .WithMessage("AnalogDigital must be either 'A' (Analog) or 'D' (Digital).");

            RuleFor(x => x.StationFunction)
                .Must(c => string.Equals(c, "TX", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(c, "RX", StringComparison.InvariantCultureIgnoreCase))
                .When(x => x.StationFunction != null)
                .WithMessage("StationFunction must be either 'TX' or 'RX'.");
        }

    }
}
