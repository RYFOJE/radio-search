namespace Radio_Search.Importer.Canada.Services.Data
{
    public class TAFLDefinitionAllDbRows
    {
        public HashSet<short> StandbyTransmitterInfos { get; set; } = [];
        public HashSet<short> RegulatoryServices { get; set; } = new();
        public HashSet<string> CommunicationTypes { get; set; } = new();
        public HashSet<string> ConformityToFrequencyPlans { get; set; } = new();
        public HashSet<string> OperationalStatuses { get; set; } = new();
        public HashSet<string> StationClasses { get; set; } = new();
        public HashSet<short> Services { get; set; } = new();
        public HashSet<short> Subservices { get; set; } = new();
        public HashSet<string> LicenseTypes { get; set; } = new();
        public HashSet<string> AuthorizationStatuses { get; set; } = new();
        public HashSet<char> CongestionZones { get; set; } = new();
        public HashSet<short> StationTypes { get; set; } = new();
        public HashSet<string> ITUClassOfStations { get; set; } = new();
        public HashSet<short> StationCostCategories { get; set; } = new();
        public HashSet<string> Provinces { get; set; } = new();
        public HashSet<char> Polarizations { get; set; } = new();
        public HashSet<string> AntennaPatterns { get; set; } = new();
        public HashSet<string> ModulationTypes { get; set; } = new();
        public HashSet<char> FiltrationInstalledTypes { get; set; } = new();
        public HashSet<char> AnalogDigitals { get; set; } = new();
        public HashSet<string> StationFunctions { get; set; } = new();
    }
}
