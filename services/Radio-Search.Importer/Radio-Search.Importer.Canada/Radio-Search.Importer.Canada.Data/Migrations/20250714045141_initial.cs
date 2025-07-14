using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Radio_Search.Importer.Canada.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Canada_Importer");

            migrationBuilder.CreateTable(
                name: "AntennaPatterns",
                schema: "Canada_Importer",
                columns: table => new
                {
                    AntennaPatternID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntennaPatterns", x => x.AntennaPatternID);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationStatuses",
                schema: "Canada_Importer",
                columns: table => new
                {
                    AuthorizationStatusID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationStatuses", x => x.AuthorizationStatusID);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    CommunicationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTypes", x => x.CommunicationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ConformityFrequencyPlans",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ConformityFrequencyPlanID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConformityFrequencyPlans", x => x.ConformityFrequencyPlanID);
                });

            migrationBuilder.CreateTable(
                name: "CongestionZoneTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    CongestionZoneTypeID = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongestionZoneTypes", x => x.CongestionZoneTypeID);
                });

            migrationBuilder.CreateTable(
                name: "FiltrationInstalledTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    FiltrationInstalledTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiltrationInstalledTypes", x => x.FiltrationInstalledTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ImportHistory",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ImportHistoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ImportRowCount = table.Column<int>(type: "int", nullable: true),
                    SkippedRowCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportHistory", x => x.ImportHistoryID);
                });

            migrationBuilder.CreateTable(
                name: "ITUClassTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ITUClassTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITUClassTypes", x => x.ITUClassTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LicenseTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    LicenseTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseTypes", x => x.LicenseTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ModulationTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ModulationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulationTypes", x => x.ModulationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "OperationStatuses",
                schema: "Canada_Importer",
                columns: table => new
                {
                    OperationStatusID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationStatuses", x => x.OperationStatusID);
                });

            migrationBuilder.CreateTable(
                name: "PolarizationTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    PolarizationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolarizationTypes", x => x.PolarizationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ProvinceID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ProvinceID);
                });

            migrationBuilder.CreateTable(
                name: "RegulatoryServices",
                schema: "Canada_Importer",
                columns: table => new
                {
                    RegulatoryServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulatoryServices", x => x.RegulatoryServiceID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ServiceTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "StandbyTransmitterInformation",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StandbyTransmitterInformationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandbyTransmitterInformation", x => x.StandbyTransmitterInformationID);
                });

            migrationBuilder.CreateTable(
                name: "StationClasses",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StationClassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationClasses", x => x.StationClassID);
                });

            migrationBuilder.CreateTable(
                name: "StationCostCategories",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StationCostCategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationCostCategories", x => x.StationCostCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "stationFunctionTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StationFunctionTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stationFunctionTypes", x => x.StationFunctionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "stationTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stationTypes", x => x.StationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SubserviceTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    SubserviceTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubserviceTypes", x => x.SubserviceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRecords",
                schema: "Canada_Importer",
                columns: table => new
                {
                    LicenseRecordID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    StationFunctionTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FrequencyMHz = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegulatoryServiceID = table.Column<int>(type: "int", nullable: true),
                    CommunicationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConformityToFrequencyPlanConformityFrequencyPlanID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FrequencyAllocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternationalCoordinationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalogDigital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupiedBandwidthKHz = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DesignationOfEmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModulationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FiltrationInstalledTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TxERPdBW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TxPowerW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLossesDb = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AnalogCapacityChannels = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DigitalCapacityMbps = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RxUnfadedReceivedSignalLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RxThresholdSignalLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntennaManufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntennaModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntennaGainDbi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AntennaPatternID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BeamwidthDeg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FrontToBackRatioDb = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PolarizationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HeightAboveGroundM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AzimuthMainLobeDeg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerticalElevationAngleDeg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StationLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ITUClassOfStationITUClassTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StationCostCategoryID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NumberOfIdenticalStations = table.Column<int>(type: "int", nullable: true),
                    ReferenceIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true),
                    GroundElevationM = table.Column<int>(type: "int", nullable: true),
                    AntennaStructureHeightM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CongestionZoneTypeID = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    RadiusOfOperationKm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SatelliteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypeID = table.Column<int>(type: "int", nullable: true),
                    SubserviceTypeID = table.Column<int>(type: "int", nullable: true),
                    LicenceTypeLicenseTypeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorizationStatusID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InServiceDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationalStatusOperationStatusID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StationClassID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HorizontalPowerW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerticalPowerW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StandbyTransmitterInformationID = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRecords", x => x.LicenseRecordID);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AntennaPatterns_AntennaPatternID",
                        column: x => x.AntennaPatternID,
                        principalSchema: "Canada_Importer",
                        principalTable: "AntennaPatterns",
                        principalColumn: "AntennaPatternID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AuthorizationStatuses_AuthorizationStatusID",
                        column: x => x.AuthorizationStatusID,
                        principalSchema: "Canada_Importer",
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "AuthorizationStatusID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_CommunicationTypes_CommunicationTypeID",
                        column: x => x.CommunicationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "CommunicationTypes",
                        principalColumn: "CommunicationTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ConformityFrequencyPlans_ConformityToFrequencyPlanConformityFrequencyPlanID",
                        column: x => x.ConformityToFrequencyPlanConformityFrequencyPlanID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ConformityFrequencyPlans",
                        principalColumn: "ConformityFrequencyPlanID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_CongestionZoneTypes_CongestionZoneTypeID",
                        column: x => x.CongestionZoneTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "CongestionZoneTypes",
                        principalColumn: "CongestionZoneTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_FiltrationInstalledTypes_FiltrationInstalledTypeID",
                        column: x => x.FiltrationInstalledTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "FiltrationInstalledTypes",
                        principalColumn: "FiltrationInstalledTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ITUClassTypes_ITUClassOfStationITUClassTypeID",
                        column: x => x.ITUClassOfStationITUClassTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ITUClassTypes",
                        principalColumn: "ITUClassTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_LicenseTypes_LicenceTypeLicenseTypeID",
                        column: x => x.LicenceTypeLicenseTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "LicenseTypes",
                        principalColumn: "LicenseTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ModulationTypes_ModulationTypeID",
                        column: x => x.ModulationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ModulationTypes",
                        principalColumn: "ModulationTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_OperationStatuses_OperationalStatusOperationStatusID",
                        column: x => x.OperationalStatusOperationStatusID,
                        principalSchema: "Canada_Importer",
                        principalTable: "OperationStatuses",
                        principalColumn: "OperationStatusID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_PolarizationTypes_PolarizationTypeID",
                        column: x => x.PolarizationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "PolarizationTypes",
                        principalColumn: "PolarizationTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalSchema: "Canada_Importer",
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_RegulatoryServices_RegulatoryServiceID",
                        column: x => x.RegulatoryServiceID,
                        principalSchema: "Canada_Importer",
                        principalTable: "RegulatoryServices",
                        principalColumn: "RegulatoryServiceID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StandbyTransmitterInformation_StandbyTransmitterInformationID",
                        column: x => x.StandbyTransmitterInformationID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StandbyTransmitterInformation",
                        principalColumn: "StandbyTransmitterInformationID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationClasses_StationClassID",
                        column: x => x.StationClassID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StationClasses",
                        principalColumn: "StationClassID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationCostCategories_StationCostCategoryID",
                        column: x => x.StationCostCategoryID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StationCostCategories",
                        principalColumn: "StationCostCategoryID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_SubserviceTypes_SubserviceTypeID",
                        column: x => x.SubserviceTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "SubserviceTypes",
                        principalColumn: "SubserviceTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_stationFunctionTypes_StationFunctionTypeID",
                        column: x => x.StationFunctionTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "stationFunctionTypes",
                        principalColumn: "StationFunctionTypeID");
                    table.ForeignKey(
                        name: "FK_LicenseRecords_stationTypes_StationTypeID",
                        column: x => x.StationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "stationTypes",
                        principalColumn: "StationTypeID");
                });

            migrationBuilder.CreateTable(
                name: "LicenseRecordsHistory",
                schema: "Canada_Importer",
                columns: table => new
                {
                    LicenseRecordHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseRecordID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImportHistoryRecordImportHistoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    JSONRepresentation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRecordsHistory", x => x.LicenseRecordHistoryID);
                    table.ForeignKey(
                        name: "FK_LicenseRecordsHistory_ImportHistory_ImportHistoryRecordImportHistoryID",
                        column: x => x.ImportHistoryRecordImportHistoryID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ImportHistory",
                        principalColumn: "ImportHistoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseRecordsHistory_LicenseRecords_LicenseRecordID",
                        column: x => x.LicenseRecordID,
                        principalSchema: "Canada_Importer",
                        principalTable: "LicenseRecords",
                        principalColumn: "LicenseRecordID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_AntennaPatternID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "AntennaPatternID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_AuthorizationStatusID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "AuthorizationStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_CommunicationTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "CommunicationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ConformityToFrequencyPlanConformityFrequencyPlanID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ConformityToFrequencyPlanConformityFrequencyPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_CongestionZoneTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "CongestionZoneTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_FiltrationInstalledTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "FiltrationInstalledTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_FrequencyMHz",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "FrequencyMHz");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ITUClassOfStationITUClassTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ITUClassOfStationITUClassTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_LicenceTypeLicenseTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "LicenceTypeLicenseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ModulationTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ModulationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_OperationalStatusOperationStatusID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "OperationalStatusOperationStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_PolarizationTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "PolarizationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ProvinceID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_RegulatoryServiceID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "RegulatoryServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ServiceTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StandbyTransmitterInformationID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "StandbyTransmitterInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationClassID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "StationClassID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationCostCategoryID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "StationCostCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationFunctionTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "StationFunctionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "StationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_SubserviceTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "SubserviceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_ImportHistoryRecordImportHistoryID",
                schema: "Canada_Importer",
                table: "LicenseRecordsHistory",
                column: "ImportHistoryRecordImportHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_LicenseRecordID",
                schema: "Canada_Importer",
                table: "LicenseRecordsHistory",
                column: "LicenseRecordID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseRecordsHistory",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ImportHistory",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "LicenseRecords",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "AntennaPatterns",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "AuthorizationStatuses",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "CommunicationTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ConformityFrequencyPlans",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "CongestionZoneTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "FiltrationInstalledTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ITUClassTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "LicenseTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ModulationTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "OperationStatuses",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "PolarizationTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "Provinces",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "RegulatoryServices",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ServiceTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "StandbyTransmitterInformation",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "StationClasses",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "StationCostCategories",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "SubserviceTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "stationFunctionTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "stationTypes",
                schema: "Canada_Importer");
        }
    }
}
