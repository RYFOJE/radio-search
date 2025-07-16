using Spire.Pdf;
using Spire.Pdf.Fields;
using System.Drawing;
using System.Reflection.PortableExecutable;

namespace Radio_Search.Importer.Canada.Services.Data.Enums
{
    public enum TableDefinitions
    {
        Skip = -1,
        StationFunction = 0,
        RegulatoryService = 4,
        CommunicationType = 5,
        ConformityToFrequencyPlan = 6,
        AnalogDigital = 10,
        ModulationType = 13,
        FiltrationType = 14,
        AntennaPattern = 25,
        Polarization = 28,
        TypeOfStation = 35,
        ITUClass = 36,
        StationCostCategory = 37,
        Provinces = 40,
        CongestionZone = 45,
        Service = 49,
        Subservice = 50,
        LicenseType = 51,
        AuthorizationStatus = 52,
        OperationalStatus = 57,
        StationClass = 58,
        StandbyTransmitterInformation = 61
    }
}
