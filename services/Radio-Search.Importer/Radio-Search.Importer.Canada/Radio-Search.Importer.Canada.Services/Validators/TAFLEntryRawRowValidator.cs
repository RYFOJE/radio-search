using FluentValidation;
using NetTopologySuite.Geometries;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Validators
{
    public class TAFLEntryRawRowValidator : AbstractValidator<TAFLEntryRawRow>
    {

        public TAFLEntryRawRowValidator()
        {
            RuleFor(x => x.FrequencyMHz)
                .NotNull()
                .WithMessage("Frequency cannot be null.")
                .GreaterThan(0).WithMessage("Frequency cannot be negative.");

            RuleFor(x => x.LicenseRecordID)
                .NotNull()
                .WithMessage("LicenseRecordID Cannot be null.")
                .MinimumLength(1) // Maybe set to exact length
                .WithMessage("LicenseRecordID must be longer than 1.");

            RuleFor(x => x.RegulatoryServiceID)
                .Must(id => string.IsNullOrWhiteSpace(id) || short.TryParse(id, out _))
                .WithMessage("RegulatoryServiceID must be empty or a valid short value.")
                .When(x => !string.IsNullOrWhiteSpace(x.RegulatoryServiceID));

            RuleFor(x => x.AnalogDigitalID)
                .Must(id => string.IsNullOrWhiteSpace(id) || id.Length == 1)
                .WithMessage("AnalogDigitalID must be empty or a length of 1 Char.")
                .When(x => !string.IsNullOrWhiteSpace(x.AnalogDigitalID));

            RuleFor(x => x.OccupiedBandwidthKHz)
                .GreaterThanOrEqualTo(0)
                .WithMessage("OccupiedBandwidthKHz cannot be negative.")
                .When(x => x.OccupiedBandwidthKHz.HasValue);

            RuleFor(x => x.FiltrationInstalledTypeID)
                .Must(id => string.IsNullOrWhiteSpace(id) || id.Length == 1)
                .WithMessage("FiltrationInstalledTypeID must be empty or a length of 1 Char.")
                .When(x => !string.IsNullOrWhiteSpace(x.FiltrationInstalledTypeID));

            RuleFor(x => x.AnalogCapacityChannels)
                .GreaterThanOrEqualTo(0)
                .WithMessage("AnalogCapacityChannels cannot be negative.")
                .When(x => x.AnalogCapacityChannels.HasValue);

            RuleFor(x => x.DigitalCapacityMbps)
                .GreaterThanOrEqualTo(0)
                .WithMessage("DigitalCapacityMbps cannot be negative.")
                .When(x => x.DigitalCapacityMbps.HasValue);

            RuleFor(x => x.BeamwidthDeg)
                .GreaterThan(0)
                .WithMessage("BeamwidthDeg cannot be negative.")
                .When(x => x.BeamwidthDeg.HasValue);

            RuleFor(x => x.FrontToBackRatioDb)
                .GreaterThanOrEqualTo(0)
                .WithMessage("FrontToBackRatioDb cannot be negative.")
                .When(x => x.FrontToBackRatioDb.HasValue);

            RuleFor(x => x.PolarizationTypeID)
                .Must(id => string.IsNullOrWhiteSpace(id) || id.Length == 1)
                .WithMessage("PolarizationTypeID must be empty or a length of 1 Char.");

            RuleFor(x => x.AzimuthMainLobeDeg)
                .GreaterThanOrEqualTo(0)
                .WithMessage("AzimuthMainLobeDeg cannot be negative.")
                .When(x => x.AzimuthMainLobeDeg.HasValue);

            RuleFor(x => x.StationTypeID)
                .Must(id => string.IsNullOrWhiteSpace(id) || short.TryParse(id, out _))
                .WithMessage("StationTypeID must be empty or a valid short value.");

            RuleFor(x => x.StationCostCategoryID)
                .InclusiveBetween(short.MinValue, short.MaxValue)
                .WithMessage("StationCostCategoryID must be a valid short value.")
                .When(x => x.StationCostCategoryID.HasValue);

            RuleFor(x => x.ProvinceID)
                .Must(id => string.IsNullOrWhiteSpace(id) || id.Length == 2)
                .WithMessage("ProvinceID must be empty or a length of 2 Chars.");

            RuleFor(x => new { x.Latitude, x.Longitude })
                .Must(coords =>
                {
                    if (coords.Latitude is null || coords.Longitude is null)
                        return false;

                    try
                    {
                        var point = new Point((double)coords.Latitude.Value, (double)coords.Longitude.Value) { SRID = 4326 };
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage("If both Latitude and Longitude are set, they must form a valid Point.")
                .When(x => x.Latitude.HasValue || x.Longitude.HasValue);

            RuleFor(x => x.LicenseTypeID)
                .Must(id => !id!.Contains("DEV"))
                .WithMessage("LicenseTypeId cannot contain DEV") // TODO Gotta find a better way than this...
                .When(x => !string.IsNullOrWhiteSpace(x.LicenseTypeID));

            RuleFor(x => x.AntennaStructureHeightM)
                .GreaterThanOrEqualTo(0)
                .WithMessage("AntennaStructureHeightM must be larger or equal to 0.")
                .When(x => x.AntennaStructureHeightM.HasValue);

            RuleFor(x => x.CongestionZoneTypeID)
                .Must(id => string.IsNullOrWhiteSpace(id) || id.Length == 1)
                .WithMessage("CongestionZoneTypeID must be empty or a length of 1 Char.")
                .When(x => !string.IsNullOrEmpty(x.CongestionZoneTypeID));

            RuleFor(x => x.ServiceTypeID)
                .InclusiveBetween(short.MinValue, short.MaxValue)
                .WithMessage("ServiceTypeID must be a valid short value.")
                .When(x => x.ServiceTypeID.HasValue);

            RuleFor(x => x.SubserviceTypeID)
                .InclusiveBetween(short.MinValue, short.MaxValue)
                .WithMessage("SubserviceTypeID must be empty or a valid short value.")
                .When(x => x.SubserviceTypeID.HasValue);

            RuleFor(x => x.InServiceDate)
                .NotNull()
                .WithMessage("InServiceDate cannot be null;");

            RuleFor(x => x.HorizontalPowerW)
                .GreaterThanOrEqualTo(0)
                .WithMessage("HorizontalPowerW must be Positive.")
                .When(x => x.HorizontalPowerW.HasValue);

            RuleFor(x => x.VerticalPowerW)
                .GreaterThanOrEqualTo(0)
                .WithMessage("VerticalPowerW must be Positive.")
                .When(x => x.VerticalPowerW.HasValue);

            RuleFor(x => x.StandbyTransmitterInformationID)
                .InclusiveBetween(short.MinValue, short.MaxValue)
                .WithMessage("StandbyTransmitterInformationID must be empty or a valid short value.")
                .When(x => x.StandbyTransmitterInformationID.HasValue);
        }
    }
}
