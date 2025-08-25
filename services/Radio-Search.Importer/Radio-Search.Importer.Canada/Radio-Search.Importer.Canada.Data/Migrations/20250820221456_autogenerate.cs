using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Radio_Search.Importer.Canada.Data.Migrations
{
    /// <inheritdoc />
    public partial class autogenerate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Canada_Importer");

            migrationBuilder.CreateTable(
                name: "AnalogDigital",
                schema: "Canada_Importer",
                columns: table => new
                {
                    AnalogDigitalID = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogDigital", x => x.AnalogDigitalID);
                });

            migrationBuilder.CreateTable(
                name: "AntennaPatterns",
                schema: "Canada_Importer",
                columns: table => new
                {
                    AntennaPatternID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
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
                    AuthorizationStatusID = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
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
                    CommunicationTypeID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
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
                    FiltrationInstalledTypeID = table.Column<string>(type: "nvarchar(1)", nullable: false),
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
                name: "ImportJobs",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ImportJobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentStep = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobs", x => x.ImportJobID);
                });

            migrationBuilder.CreateTable(
                name: "ITUClassTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ITUClassTypeID = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
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
                    LicenseTypeID = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
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
                    ModulationTypeID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
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
                    OperationalStatusID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationStatuses", x => x.OperationalStatusID);
                });

            migrationBuilder.CreateTable(
                name: "PolarizationTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    PolarizationTypeID = table.Column<string>(type: "nvarchar(1)", nullable: false),
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
                    ProvinceID = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
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
                    RegulatoryServiceID = table.Column<short>(type: "smallint", nullable: false),
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
                    ServiceTypeID = table.Column<short>(type: "smallint", nullable: false),
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
                    StandbyTransmitterInformationID = table.Column<short>(type: "smallint", nullable: false),
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
                    StationClassID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
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
                    StationCostCategoryID = table.Column<short>(type: "smallint", nullable: false),
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
                name: "StationFunctionTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StationFunctionTypeID = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationFunctionTypes", x => x.StationFunctionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "StationTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    StationTypeID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationTypes", x => x.StationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SubserviceTypes",
                schema: "Canada_Importer",
                columns: table => new
                {
                    SubserviceTypeID = table.Column<short>(type: "smallint", nullable: false),
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
                name: "ImportJobChunkFiles",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ImportJobID = table.Column<int>(type: "int", nullable: false),
                    FileID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobChunkFiles", x => new { x.ImportJobID, x.FileID });
                    table.ForeignKey(
                        name: "FK_ImportJobChunkFiles_ImportJobs_ImportJobID",
                        column: x => x.ImportJobID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ImportJobs",
                        principalColumn: "ImportJobID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportJobStats",
                schema: "Canada_Importer",
                columns: table => new
                {
                    ImportJobID = table.Column<int>(type: "int", nullable: false),
                    PreprocessingSkippedRows = table.Column<int>(type: "int", nullable: false),
                    NewRecordCount = table.Column<int>(type: "int", nullable: false),
                    UpdatedRecordCount = table.Column<int>(type: "int", nullable: false),
                    DeletedRecordCount = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobStats", x => x.ImportJobID);
                    table.ForeignKey(
                        name: "FK_ImportJobStats_ImportJobs_ImportJobID",
                        column: x => x.ImportJobID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ImportJobs",
                        principalColumn: "ImportJobID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRecords",
                schema: "Canada_Importer",
                columns: table => new
                {
                    CanadaLicenseRecordID = table.Column<int>(type: "int", maxLength: 30, nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    FrequencyMHz = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    FrequencyAllocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternationalCoordinationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupiedBandwidthKHz = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    DesignationOfEmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TxERPdBW = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    TxPowerW = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    TotalLossesDb = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    AnalogCapacityChannels = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    DigitalCapacityMbps = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    RxUnfadedReceivedSignalLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RxThresholdSignalLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntennaManufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntennaModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntennaGainDbi = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    BeamwidthDeg = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    FrontToBackRatioDb = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    HeightAboveGroundM = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    AzimuthMainLobeDeg = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    VerticalElevationAngleDeg = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    StationLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallSign = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    NumberOfIdenticalStations = table.Column<int>(type: "int", nullable: true),
                    ReferenceIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true),
                    GroundElevationM = table.Column<int>(type: "int", nullable: true),
                    AntennaStructureHeightM = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    RadiusOfOperationKm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SatelliteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InServiceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HorizontalPowerW = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    VerticalPowerW = table.Column<decimal>(type: "decimal(24,12)", precision: 24, scale: 12, nullable: true),
                    StationFunctionID = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    RegulatoryServiceID = table.Column<short>(type: "smallint", nullable: true),
                    CommunicationTypeID = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    ConformityFrequencyPlanID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnalogDigitalID = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    ModulationTypeID = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    FiltrationInstalledTypeID = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    AntennaPatternID = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    PolarizationTypeID = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    StationTypeID = table.Column<short>(type: "smallint", nullable: true),
                    ITUClassTypeID = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    StationCostCategoryID = table.Column<short>(type: "smallint", nullable: true),
                    ProvinceID = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    CongestionZoneTypeID = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    ServiceTypeID = table.Column<short>(type: "smallint", nullable: true),
                    SubserviceTypeID = table.Column<short>(type: "smallint", nullable: true),
                    LicenseTypeID = table.Column<string>(type: "nvarchar(7)", nullable: true),
                    AuthorizationStatusID = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    OperationalStatusID = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    StationClassID = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    StandbyTransmitterInformationID = table.Column<short>(type: "smallint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRecords", x => new { x.CanadaLicenseRecordID, x.Version })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AnalogDigital_AnalogDigitalID",
                        column: x => x.AnalogDigitalID,
                        principalSchema: "Canada_Importer",
                        principalTable: "AnalogDigital",
                        principalColumn: "AnalogDigitalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AntennaPatterns_AntennaPatternID",
                        column: x => x.AntennaPatternID,
                        principalSchema: "Canada_Importer",
                        principalTable: "AntennaPatterns",
                        principalColumn: "AntennaPatternID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AuthorizationStatuses_AuthorizationStatusID",
                        column: x => x.AuthorizationStatusID,
                        principalSchema: "Canada_Importer",
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "AuthorizationStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_CommunicationTypes_CommunicationTypeID",
                        column: x => x.CommunicationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "CommunicationTypes",
                        principalColumn: "CommunicationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ConformityFrequencyPlans_ConformityFrequencyPlanID",
                        column: x => x.ConformityFrequencyPlanID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ConformityFrequencyPlans",
                        principalColumn: "ConformityFrequencyPlanID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_CongestionZoneTypes_CongestionZoneTypeID",
                        column: x => x.CongestionZoneTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "CongestionZoneTypes",
                        principalColumn: "CongestionZoneTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_FiltrationInstalledTypes_FiltrationInstalledTypeID",
                        column: x => x.FiltrationInstalledTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "FiltrationInstalledTypes",
                        principalColumn: "FiltrationInstalledTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ITUClassTypes_ITUClassTypeID",
                        column: x => x.ITUClassTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ITUClassTypes",
                        principalColumn: "ITUClassTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_LicenseTypes_LicenseTypeID",
                        column: x => x.LicenseTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "LicenseTypes",
                        principalColumn: "LicenseTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ModulationTypes_ModulationTypeID",
                        column: x => x.ModulationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ModulationTypes",
                        principalColumn: "ModulationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_OperationStatuses_OperationalStatusID",
                        column: x => x.OperationalStatusID,
                        principalSchema: "Canada_Importer",
                        principalTable: "OperationStatuses",
                        principalColumn: "OperationalStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_PolarizationTypes_PolarizationTypeID",
                        column: x => x.PolarizationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "PolarizationTypes",
                        principalColumn: "PolarizationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalSchema: "Canada_Importer",
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_RegulatoryServices_RegulatoryServiceID",
                        column: x => x.RegulatoryServiceID,
                        principalSchema: "Canada_Importer",
                        principalTable: "RegulatoryServices",
                        principalColumn: "RegulatoryServiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StandbyTransmitterInformation_StandbyTransmitterInformationID",
                        column: x => x.StandbyTransmitterInformationID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StandbyTransmitterInformation",
                        principalColumn: "StandbyTransmitterInformationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationClasses_StationClassID",
                        column: x => x.StationClassID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StationClasses",
                        principalColumn: "StationClassID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationCostCategories_StationCostCategoryID",
                        column: x => x.StationCostCategoryID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StationCostCategories",
                        principalColumn: "StationCostCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationFunctionTypes_StationFunctionID",
                        column: x => x.StationFunctionID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StationFunctionTypes",
                        principalColumn: "StationFunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationTypes_StationTypeID",
                        column: x => x.StationTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "StationTypes",
                        principalColumn: "StationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_SubserviceTypes_SubserviceTypeID",
                        column: x => x.SubserviceTypeID,
                        principalSchema: "Canada_Importer",
                        principalTable: "SubserviceTypes",
                        principalColumn: "SubserviceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRecordsHistory",
                schema: "Canada_Importer",
                columns: table => new
                {
                    LicenseRecordHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanadaLicenseRecordID = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    EditedByImportJobID = table.Column<int>(type: "int", nullable: true),
                    EditedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRecordsHistory", x => x.LicenseRecordHistoryId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_LicenseRecordsHistory_ImportJobs_EditedByImportJobID",
                        column: x => x.EditedByImportJobID,
                        principalSchema: "Canada_Importer",
                        principalTable: "ImportJobs",
                        principalColumn: "ImportJobID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LicenseRecordsHistory_LicenseRecords_CanadaLicenseRecordID_Version",
                        columns: x => new { x.CanadaLicenseRecordID, x.Version },
                        principalSchema: "Canada_Importer",
                        principalTable: "LicenseRecords",
                        principalColumns: new[] { "CanadaLicenseRecordID", "Version" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportJobs_Status",
                schema: "Canada_Importer",
                table: "ImportJobs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_AnalogDigitalID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "AnalogDigitalID");

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
                name: "IX_LicenseRecords_ConformityFrequencyPlanID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ConformityFrequencyPlanID");

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
                name: "IX_LicenseRecords_IsValid",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "IsValid");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid_CallSign",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                columns: new[] { "IsValid", "CallSign" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid_CanadaLicenseRecordID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                columns: new[] { "IsValid", "CanadaLicenseRecordID" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid_FrequencyMHz",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                columns: new[] { "IsValid", "FrequencyMHz" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ITUClassTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ITUClassTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_LicenseTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "LicenseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ModulationTypeID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "ModulationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_OperationalStatusID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "OperationalStatusID");

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
                name: "IX_LicenseRecords_StationFunctionID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "StationFunctionID");

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
                name: "IX_LicenseRecordsHistory_CanadaLicenseRecordID",
                schema: "Canada_Importer",
                table: "LicenseRecordsHistory",
                column: "CanadaLicenseRecordID")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_CanadaLicenseRecordID_Version",
                schema: "Canada_Importer",
                table: "LicenseRecordsHistory",
                columns: new[] { "CanadaLicenseRecordID", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_EditedByImportJobID",
                schema: "Canada_Importer",
                table: "LicenseRecordsHistory",
                column: "EditedByImportJobID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_EditedByImportJobID_ChangeType",
                schema: "Canada_Importer",
                table: "LicenseRecordsHistory",
                columns: new[] { "EditedByImportJobID", "ChangeType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportJobChunkFiles",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ImportJobStats",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "LicenseRecordsHistory",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "ImportJobs",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "LicenseRecords",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "AnalogDigital",
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
                name: "StationFunctionTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "StationTypes",
                schema: "Canada_Importer");

            migrationBuilder.DropTable(
                name: "SubserviceTypes",
                schema: "Canada_Importer");
        }
    }
}
