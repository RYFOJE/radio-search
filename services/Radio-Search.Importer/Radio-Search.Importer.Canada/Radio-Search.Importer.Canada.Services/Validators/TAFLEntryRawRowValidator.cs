using FluentValidation;
using NetTopologySuite.Geometries;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Validators
{
    public class TAFLEntryRawRowValidator : AbstractValidator<TAFLEntryRawRow>
    {
        public TAFLEntryRawRowValidator()
        {
            // Index 0 - StationFunctionID
            RuleFor(x => x.StationFunctionID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.StationFunctions.Contains(id);
                    }
                    return false;
                })
                .WithMessage("StationFunctionID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.StationFunctionID));

            // Index 1 - FrequencyMHz
            RuleFor(x => x.FrequencyMHz)
                .NotNull()
                .WithMessage("Frequency cannot be null.")
                .GreaterThan(0).WithMessage("Frequency cannot be negative.");

            // Index 2 - LicenseRecordID
            RuleFor(x => x.LicenseRecordID)
                .NotNull()
                .WithMessage("LicenseRecordID Cannot be null.")
                .MinimumLength(1) // Maybe set to exact length
                .WithMessage("LicenseRecordID must be longer than 1.");

            // Index 3 - RegulatoryServiceID
            RuleFor(x => x.RegulatoryServiceID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.RegulatoryServices.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("RegulatoryServiceID must be empty or a valid short value present in definitions.")
                .When(x => x.RegulatoryServiceID != null);

            // Index 4 - CommunicationTypeID
            RuleFor(x => x.CommunicationTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.CommunicationTypes.Contains(id);
                    }
                    return false;
                })
                .WithMessage("CommunicationTypeID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.CommunicationTypeID));

            // Index 5 - ConformityFrequencyPlanID
            RuleFor(x => x.ConformityFrequencyPlanID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.ConformityToFrequencyPlans.Contains(id);
                    }
                    return false;
                })
                .WithMessage("ConformityFrequencyPlanID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.ConformityFrequencyPlanID));

            // Index 6 - FrequencyAllocationName
            RuleFor(x => x.FrequencyAllocationName)
                .MaximumLength(255)
                .WithMessage("FrequencyAllocationName cannot exceed 255 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.FrequencyAllocationName));

            // Index 7 - Channel
            RuleFor(x => x.Channel)
                .MaximumLength(50)
                .WithMessage("Channel cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Channel));

            // Index 8 - InternationalCoordinationNumber
            RuleFor(x => x.InternationalCoordinationNumber)
                .MaximumLength(100)
                .WithMessage("InternationalCoordinationNumber cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.InternationalCoordinationNumber));

            // Index 9 - AnalogDigitalID
            RuleFor(x => x.AnalogDigitalID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.AnalogDigitals.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("AnalogDigitalID must be empty or a valid char value present in definitions.")
                .When(x => x.AnalogDigitalID != null);

            // Index 10 - OccupiedBandwidthKHz
            RuleFor(x => x.OccupiedBandwidthKHz)
                .GreaterThanOrEqualTo(0)
                .WithMessage("OccupiedBandwidthKHz cannot be negative.")
                .When(x => x.OccupiedBandwidthKHz.HasValue);

            // Index 11 - DesignationOfEmission
            RuleFor(x => x.DesignationOfEmission)
                .MaximumLength(50)
                .WithMessage("DesignationOfEmission cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.DesignationOfEmission));

            // Index 12 - ModulationTypeID
            RuleFor(x => x.ModulationTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.ModulationTypes.Contains(id);
                    }
                    return false;
                })
                .WithMessage("ModulationTypeID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.ModulationTypeID));

            // Index 13 - FiltrationInstalledTypeID
            RuleFor(x => x.FiltrationInstalledTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.FiltrationInstalledTypes.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("FiltrationInstalledTypeID must be empty or a valid char value present in definitions.")
                .When(x => x.FiltrationInstalledTypeID != null);

            // Index 14 - TxERPdBW
            RuleFor(x => x.TxERPdBW)
                .GreaterThanOrEqualTo(-200)
                .LessThanOrEqualTo(200)
                .WithMessage("TxERPdBW must be between -200 and 200 dBW.")
                .When(x => x.TxERPdBW.HasValue);

            // Index 15 - TxPowerW
            RuleFor(x => x.TxPowerW)
                .GreaterThanOrEqualTo(0)
                .WithMessage("TxPowerW must be positive.")
                .When(x => x.TxPowerW.HasValue);

            // Index 16 - TotalLossesDb
            RuleFor(x => x.TotalLossesDb)
                .GreaterThanOrEqualTo(0)
                .WithMessage("TotalLossesDb cannot be negative.")
                .When(x => x.TotalLossesDb.HasValue);

            // Index 17 - AnalogCapacityChannels
            RuleFor(x => x.AnalogCapacityChannels)
                .GreaterThanOrEqualTo(0)
                .WithMessage("AnalogCapacityChannels cannot be negative.")
                .When(x => x.AnalogCapacityChannels.HasValue);

            // Index 18 - DigitalCapacityMbps
            RuleFor(x => x.DigitalCapacityMbps)
                .GreaterThanOrEqualTo(0)
                .WithMessage("DigitalCapacityMbps cannot be negative.")
                .When(x => x.DigitalCapacityMbps.HasValue);

            // Index 19 - RxUnfadedReceivedSignalLevel
            RuleFor(x => x.RxUnfadedReceivedSignalLevel)
                .MaximumLength(50)
                .WithMessage("RxUnfadedReceivedSignalLevel cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.RxUnfadedReceivedSignalLevel));

            // Index 20 - RxThresholdSignalLevel
            RuleFor(x => x.RxThresholdSignalLevel)
                .MaximumLength(50)
                .WithMessage("RxThresholdSignalLevel cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.RxThresholdSignalLevel));

            // Index 21 - AntennaManufacturer
            RuleFor(x => x.AntennaManufacturer)
                .MaximumLength(100)
                .WithMessage("AntennaManufacturer cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.AntennaManufacturer));

            // Index 22 - AntennaModel
            RuleFor(x => x.AntennaModel)
                .MaximumLength(100)
                .WithMessage("AntennaModel cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.AntennaModel));

            // Index 23 - AntennaGainDbi
            RuleFor(x => x.AntennaGainDbi)
                .GreaterThanOrEqualTo(-50)
                .LessThanOrEqualTo(100)
                .WithMessage("AntennaGainDbi must be between -50 and 100 dBi.")
                .When(x => x.AntennaGainDbi.HasValue);

            // Index 24 - AntennaPatternID
            RuleFor(x => x.AntennaPatternID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.AntennaPatterns.Contains(id);
                    }
                    return false;
                })
                .WithMessage("AntennaPatternID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.AntennaPatternID));

            // Index 25 - BeamwidthDeg
            RuleFor(x => x.BeamwidthDeg)
                .GreaterThan(0)
                .LessThanOrEqualTo(360)
                .WithMessage("BeamwidthDeg must be between 0 and 360 degrees.")
                .When(x => x.BeamwidthDeg.HasValue);

            // Index 26 - FrontToBackRatioDb
            RuleFor(x => x.FrontToBackRatioDb)
                .GreaterThanOrEqualTo(0)
                .WithMessage("FrontToBackRatioDb cannot be negative.")
                .When(x => x.FrontToBackRatioDb.HasValue);

            // Index 27 - PolarizationTypeID
            RuleFor(x => x.PolarizationTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.Polarizations.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("PolarizationTypeID must be empty or a valid char value present in definitions.")
                .When(x => x.PolarizationTypeID != null);

            // Index 28 - HeightAboveGroundM
            RuleFor(x => x.HeightAboveGroundM)
                .GreaterThanOrEqualTo(0)
                .WithMessage("HeightAboveGroundM cannot be negative.")
                .When(x => x.HeightAboveGroundM.HasValue);

            // Index 29 - AzimuthMainLobeDeg
            RuleFor(x => x.AzimuthMainLobeDeg)
                .GreaterThanOrEqualTo(0)
                .LessThan(360)
                .WithMessage("AzimuthMainLobeDeg must be between 0 and 359.9 degrees.")
                .When(x => x.AzimuthMainLobeDeg.HasValue);

            // Index 30 - VerticalElevationAngleDeg
            RuleFor(x => x.VerticalElevationAngleDeg)
                .GreaterThanOrEqualTo(-90)
                .LessThanOrEqualTo(90)
                .WithMessage("VerticalElevationAngleDeg must be between -90 and 90 degrees.")
                .When(x => x.VerticalElevationAngleDeg.HasValue);

            // Index 31 - StationLocation
            RuleFor(x => x.StationLocation)
                .MaximumLength(255)
                .WithMessage("StationLocation cannot exceed 255 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.StationLocation));

            // Index 32 - StationReference
            RuleFor(x => x.StationReference)
                .MaximumLength(100)
                .WithMessage("StationReference cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.StationReference));

            // Index 33 - CallSign
            RuleFor(x => x.CallSign)
                .MaximumLength(20)
                .WithMessage("CallSign cannot exceed 20 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.CallSign));

            // Index 34 - StationTypeID
            RuleFor(x => x.StationTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.StationTypes.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("StationTypeID must be empty or a valid short value present in definitions.")
                .When(x => x.StationTypeID != null);

            // Index 35 - ITUClassTypeID
            RuleFor(x => x.ITUClassTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.ITUClassOfStations.Contains(id);
                    }
                    return false;
                })
                .WithMessage("ITUClassTypeID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.ITUClassTypeID));

            // Index 36 - StationCostCategoryID
            RuleFor(x => x.StationCostCategoryID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.StationCostCategories.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("StationCostCategoryID must be empty or a valid short value present in definitions.")
                .When(x => x.StationCostCategoryID != null);

            // Index 37 - NumberOfIdenticalStations
            RuleFor(x => x.NumberOfIdenticalStations)
                .GreaterThan(0)
                .WithMessage("NumberOfIdenticalStations must be positive.")
                .When(x => x.NumberOfIdenticalStations.HasValue);

            // Index 38 - ReferenceIdentifier
            RuleFor(x => x.ReferenceIdentifier)
                .MaximumLength(100)
                .WithMessage("ReferenceIdentifier cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ReferenceIdentifier));

            // Index 39 - ProvinceID
            RuleFor(x => x.ProvinceID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.Provinces.Contains(id);
                    }
                    return false;
                })
                .WithMessage("ProvinceID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.ProvinceID));

            // Index 40 & 41 - Latitude and Longitude
            RuleFor(x => new { x.Latitude, x.Longitude })
                .Must(coords =>
                {
                    if (coords.Latitude is null || coords.Longitude is null)
                        return false;

                    try
                    {
                        var point = new Point((double)coords.Longitude.Value, (double)coords.Latitude.Value) { SRID = 4326 };
                        return coords.Latitude >= -90 && coords.Latitude <= 90 && 
                               coords.Longitude >= -180 && coords.Longitude <= 180;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage("If both Latitude and Longitude are set, they must form a valid Point with Latitude between -90 and 90, Longitude between -180 and 180.")
                .When(x => x.Latitude.HasValue || x.Longitude.HasValue);

            // Index 42 - GroundElevationM
            RuleFor(x => x.GroundElevationM)
                .GreaterThanOrEqualTo(-500)
                .LessThanOrEqualTo(10000)
                .WithMessage("GroundElevationM must be between -500 and 10000 meters.")
                .When(x => x.GroundElevationM.HasValue);

            // Index 43 - AntennaStructureHeightM
            RuleFor(x => x.AntennaStructureHeightM)
                .GreaterThanOrEqualTo(0)
                .WithMessage("AntennaStructureHeightM must be larger or equal to 0.")
                .When(x => x.AntennaStructureHeightM.HasValue);

            // Index 44 - CongestionZoneTypeID
            RuleFor(x => x.CongestionZoneTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.CongestionZones.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("CongestionZoneTypeID must be empty or a valid char value present in definitions.")
                .When(x => x.CongestionZoneTypeID != null);

            // Index 45 - RadiusOfOperationKm
            RuleFor(x => x.RadiusOfOperationKm)
                .MaximumLength(50)
                .WithMessage("RadiusOfOperationKm cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.RadiusOfOperationKm));

            // Index 46 - SatelliteName
            RuleFor(x => x.SatelliteName)
                .MaximumLength(100)
                .WithMessage("SatelliteName cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.SatelliteName));

            // Index 47 - AuthorizationNumber
            RuleFor(x => x.AuthorizationNumber)
                .MaximumLength(100)
                .WithMessage("AuthorizationNumber cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.AuthorizationNumber));

            // Index 48 - ServiceTypeID
            RuleFor(x => x.ServiceTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.Services.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("ServiceTypeID must be empty or a valid short value present in definitions.")
                .When(x => x.ServiceTypeID != null);

            // Index 49 - SubserviceTypeID
            RuleFor(x => x.SubserviceTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.Subservices.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("SubserviceTypeID must be empty or a valid short value present in definitions.")
                .When(x => x.SubserviceTypeID != null);

            // Index 50 - LicenseTypeID
            RuleFor(x => x.LicenseTypeID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.LicenseTypes.Contains(id) && !id.Contains("DEV");
                    }
                    return false;
                })
                .WithMessage("LicenseTypeID must be empty, cannot contain DEV, and must be a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.LicenseTypeID));

            // Index 51 - AuthorizationStatusID
            RuleFor(x => x.AuthorizationStatusID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.AuthorizationStatuses.Contains(id);
                    }
                    return false;
                })
                .WithMessage("AuthorizationStatusID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.AuthorizationStatusID));

            // Index 52 - InServiceDate
            RuleFor(x => x.InServiceDate)
                .NotNull()
                .WithMessage("InServiceDate cannot be null.")
                .Must(date => date.HasValue && date.Value <= DateOnly.FromDateTime(DateTime.Now.AddYears(10)))
                .WithMessage("InServiceDate cannot be more than 10 years in the future.")
                .When(x => x.InServiceDate.HasValue);

            // Index 53 - AccountNumber
            RuleFor(x => x.AccountNumber)
                .MaximumLength(50)
                .WithMessage("AccountNumber cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.AccountNumber));

            // Index 54 - LicenseeName
            RuleFor(x => x.LicenseeName)
                .MaximumLength(255)
                .WithMessage("LicenseeName cannot exceed 255 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.LicenseeName));

            // Index 55 - LicenseeAddress
            RuleFor(x => x.LicenseeAddress)
                .MaximumLength(500)
                .WithMessage("LicenseeAddress cannot exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.LicenseeAddress));

            // Index 56 - OperationalStatusID
            RuleFor(x => x.OperationalStatusID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.OperationalStatuses.Contains(id);
                    }
                    return false;
                })
                .WithMessage("OperationalStatusID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.OperationalStatusID));

            // Index 57 - StationClassID
            RuleFor(x => x.StationClassID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (string.IsNullOrWhiteSpace(id))
                            return true;
                        return defs.StationClasses.Contains(id);
                    }
                    return false;
                })
                .WithMessage("StationClassID must be empty or a valid value present in definitions.")
                .When(x => !string.IsNullOrWhiteSpace(x.StationClassID));

            // Index 58 - HorizontalPowerW
            RuleFor(x => x.HorizontalPowerW)
                .GreaterThanOrEqualTo(0)
                .WithMessage("HorizontalPowerW must be positive.")
                .When(x => x.HorizontalPowerW.HasValue);

            // Index 59 - VerticalPowerW
            RuleFor(x => x.VerticalPowerW)
                .GreaterThanOrEqualTo(0)
                .WithMessage("VerticalPowerW must be positive.")
                .When(x => x.VerticalPowerW.HasValue);

            // Index 60 - StandbyTransmitterInformationID
            RuleFor(x => x.StandbyTransmitterInformationID)
                .Must((row, id, context) =>
                {
                    if (context.RootContextData.TryGetValue("AllDefinitions", out var defsObj) && defsObj is TAFLDefinitionAllDbRows defs)
                    {
                        if (id == null)
                            return true;
                        return defs.StandbyTransmitterInfos.Contains(id.Value);
                    }
                    return false;
                })
                .WithMessage("StandbyTransmitterInformationID must be empty or a valid short value present in definitions.")
                .When(x => x.StandbyTransmitterInformationID != null);
        }
    }
}
