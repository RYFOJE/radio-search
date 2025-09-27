using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Radio_Search.Importer.Canada.Data.Migrations
{
    /// <inheritdoc />
    public partial class licenseIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "canada_importer");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "AnalogDigital",
                schema: "canada_importer",
                columns: table => new
                {
                    AnalogDigitalID = table.Column<char>(type: "character(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogDigital", x => x.AnalogDigitalID);
                });

            migrationBuilder.CreateTable(
                name: "AntennaPatterns",
                schema: "canada_importer",
                columns: table => new
                {
                    AntennaPatternID = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntennaPatterns", x => x.AntennaPatternID);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationStatuses",
                schema: "canada_importer",
                columns: table => new
                {
                    AuthorizationStatusID = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationStatuses", x => x.AuthorizationStatusID);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    CommunicationTypeID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationTypes", x => x.CommunicationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ConformityFrequencyPlans",
                schema: "canada_importer",
                columns: table => new
                {
                    ConformityFrequencyPlanID = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConformityFrequencyPlans", x => x.ConformityFrequencyPlanID);
                });

            migrationBuilder.CreateTable(
                name: "CongestionZoneTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    CongestionZoneTypeID = table.Column<char>(type: "character(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongestionZoneTypes", x => x.CongestionZoneTypeID);
                });

            migrationBuilder.CreateTable(
                name: "FiltrationInstalledTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    FiltrationInstalledTypeID = table.Column<char>(type: "character(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiltrationInstalledTypes", x => x.FiltrationInstalledTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ImportJobs",
                schema: "canada_importer",
                columns: table => new
                {
                    ImportJobID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CurrentStep = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobs", x => x.ImportJobID);
                });

            migrationBuilder.CreateTable(
                name: "ITUClassTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    ITUClassTypeID = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITUClassTypes", x => x.ITUClassTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LicenseTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    LicenseTypeID = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseTypes", x => x.LicenseTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ModulationTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    ModulationTypeID = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulationTypes", x => x.ModulationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "OperationStatuses",
                schema: "canada_importer",
                columns: table => new
                {
                    OperationalStatusID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationStatuses", x => x.OperationalStatusID);
                });

            migrationBuilder.CreateTable(
                name: "PolarizationTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    PolarizationTypeID = table.Column<char>(type: "character(1)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolarizationTypes", x => x.PolarizationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                schema: "canada_importer",
                columns: table => new
                {
                    ProvinceID = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ProvinceID);
                });

            migrationBuilder.CreateTable(
                name: "RegulatoryServices",
                schema: "canada_importer",
                columns: table => new
                {
                    RegulatoryServiceID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulatoryServices", x => x.RegulatoryServiceID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    ServiceTypeID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.ServiceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "StandbyTransmitterInformation",
                schema: "canada_importer",
                columns: table => new
                {
                    StandbyTransmitterInformationID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandbyTransmitterInformation", x => x.StandbyTransmitterInformationID);
                });

            migrationBuilder.CreateTable(
                name: "StationClasses",
                schema: "canada_importer",
                columns: table => new
                {
                    StationClassID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationClasses", x => x.StationClassID);
                });

            migrationBuilder.CreateTable(
                name: "StationCostCategories",
                schema: "canada_importer",
                columns: table => new
                {
                    StationCostCategoryID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationCostCategories", x => x.StationCostCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "StationFunctionTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    StationFunctionTypeID = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationFunctionTypes", x => x.StationFunctionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "StationTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    StationTypeID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationTypes", x => x.StationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SubserviceTypes",
                schema: "canada_importer",
                columns: table => new
                {
                    SubserviceTypeID = table.Column<short>(type: "smallint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    DescriptionFR = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubserviceTypes", x => x.SubserviceTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ImportJobChunkFiles",
                schema: "canada_importer",
                columns: table => new
                {
                    ImportJobID = table.Column<int>(type: "integer", nullable: false),
                    FileID = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobChunkFiles", x => new { x.ImportJobID, x.FileID });
                    table.ForeignKey(
                        name: "FK_ImportJobChunkFiles_ImportJobs_ImportJobID",
                        column: x => x.ImportJobID,
                        principalSchema: "canada_importer",
                        principalTable: "ImportJobs",
                        principalColumn: "ImportJobID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportJobStats",
                schema: "canada_importer",
                columns: table => new
                {
                    ImportJobID = table.Column<int>(type: "integer", nullable: false),
                    PreprocessingSkippedRows = table.Column<int>(type: "integer", nullable: false),
                    NewRecordCount = table.Column<int>(type: "integer", nullable: false),
                    UpdatedRecordCount = table.Column<int>(type: "integer", nullable: false),
                    DeletedRecordCount = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobStats", x => x.ImportJobID);
                    table.ForeignKey(
                        name: "FK_ImportJobStats_ImportJobs_ImportJobID",
                        column: x => x.ImportJobID,
                        principalSchema: "canada_importer",
                        principalTable: "ImportJobs",
                        principalColumn: "ImportJobID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRecords",
                schema: "canada_importer",
                columns: table => new
                {
                    CanadaLicenseRecordID = table.Column<int>(type: "integer", maxLength: 30, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false),
                    FrequencyMHz = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    FrequencyAllocationName = table.Column<string>(type: "text", nullable: true),
                    Channel = table.Column<string>(type: "text", nullable: true),
                    InternationalCoordinationNumber = table.Column<string>(type: "text", nullable: true),
                    OccupiedBandwidthKHz = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    DesignationOfEmission = table.Column<string>(type: "text", nullable: true),
                    TxERPdBW = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    TxPowerW = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    TotalLossesDb = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    AnalogCapacityChannels = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    DigitalCapacityMbps = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    RxUnfadedReceivedSignalLevel = table.Column<string>(type: "text", nullable: true),
                    RxThresholdSignalLevel = table.Column<string>(type: "text", nullable: true),
                    AntennaManufacturer = table.Column<string>(type: "text", nullable: true),
                    AntennaModel = table.Column<string>(type: "text", nullable: true),
                    AntennaGainDbi = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    BeamwidthDeg = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    FrontToBackRatioDb = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    HeightAboveGroundM = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    AzimuthMainLobeDeg = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    VerticalElevationAngleDeg = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    StationLocation = table.Column<string>(type: "text", nullable: true),
                    StationReference = table.Column<string>(type: "text", nullable: true),
                    CallSign = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    NumberOfIdenticalStations = table.Column<int>(type: "integer", nullable: true),
                    ReferenceIdentifier = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<Point>(type: "geometry", nullable: true),
                    GroundElevationM = table.Column<int>(type: "integer", nullable: true),
                    AntennaStructureHeightM = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    RadiusOfOperationKm = table.Column<string>(type: "text", nullable: true),
                    SatelliteName = table.Column<string>(type: "text", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "text", nullable: true),
                    InServiceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    LicenseeName = table.Column<string>(type: "text", nullable: true),
                    LicenseeAddress = table.Column<string>(type: "text", nullable: true),
                    HorizontalPowerW = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    VerticalPowerW = table.Column<decimal>(type: "numeric(24,12)", precision: 24, scale: 12, nullable: true),
                    StationFunctionID = table.Column<string>(type: "character varying(3)", nullable: true),
                    RegulatoryServiceID = table.Column<short>(type: "smallint", nullable: true),
                    CommunicationTypeID = table.Column<string>(type: "character varying(5)", nullable: true),
                    ConformityFrequencyPlanID = table.Column<string>(type: "text", nullable: true),
                    AnalogDigitalID = table.Column<char>(type: "character(1)", nullable: true),
                    ModulationTypeID = table.Column<string>(type: "character varying(25)", nullable: true),
                    FiltrationInstalledTypeID = table.Column<char>(type: "character(1)", nullable: true),
                    AntennaPatternID = table.Column<string>(type: "character varying(15)", nullable: true),
                    PolarizationTypeID = table.Column<char>(type: "character(1)", nullable: true),
                    StationTypeID = table.Column<short>(type: "smallint", nullable: true),
                    ITUClassTypeID = table.Column<string>(type: "character varying(3)", nullable: true),
                    StationCostCategoryID = table.Column<short>(type: "smallint", nullable: true),
                    ProvinceID = table.Column<string>(type: "character varying(2)", nullable: true),
                    CongestionZoneTypeID = table.Column<char>(type: "character(1)", nullable: true),
                    ServiceTypeID = table.Column<short>(type: "smallint", nullable: true),
                    SubserviceTypeID = table.Column<short>(type: "smallint", nullable: true),
                    LicenseTypeID = table.Column<string>(type: "character varying(7)", nullable: true),
                    AuthorizationStatusID = table.Column<string>(type: "character varying(3)", nullable: true),
                    OperationalStatusID = table.Column<string>(type: "character varying(5)", nullable: true),
                    StationClassID = table.Column<string>(type: "character varying(5)", nullable: true),
                    StandbyTransmitterInformationID = table.Column<short>(type: "smallint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRecords", x => new { x.CanadaLicenseRecordID, x.Version });
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AnalogDigital_AnalogDigitalID",
                        column: x => x.AnalogDigitalID,
                        principalSchema: "canada_importer",
                        principalTable: "AnalogDigital",
                        principalColumn: "AnalogDigitalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AntennaPatterns_AntennaPatternID",
                        column: x => x.AntennaPatternID,
                        principalSchema: "canada_importer",
                        principalTable: "AntennaPatterns",
                        principalColumn: "AntennaPatternID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_AuthorizationStatuses_AuthorizationStatusID",
                        column: x => x.AuthorizationStatusID,
                        principalSchema: "canada_importer",
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "AuthorizationStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_CommunicationTypes_CommunicationTypeID",
                        column: x => x.CommunicationTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "CommunicationTypes",
                        principalColumn: "CommunicationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ConformityFrequencyPlans_ConformityFrequency~",
                        column: x => x.ConformityFrequencyPlanID,
                        principalSchema: "canada_importer",
                        principalTable: "ConformityFrequencyPlans",
                        principalColumn: "ConformityFrequencyPlanID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_CongestionZoneTypes_CongestionZoneTypeID",
                        column: x => x.CongestionZoneTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "CongestionZoneTypes",
                        principalColumn: "CongestionZoneTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_FiltrationInstalledTypes_FiltrationInstalled~",
                        column: x => x.FiltrationInstalledTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "FiltrationInstalledTypes",
                        principalColumn: "FiltrationInstalledTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ITUClassTypes_ITUClassTypeID",
                        column: x => x.ITUClassTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "ITUClassTypes",
                        principalColumn: "ITUClassTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_LicenseTypes_LicenseTypeID",
                        column: x => x.LicenseTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "LicenseTypes",
                        principalColumn: "LicenseTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ModulationTypes_ModulationTypeID",
                        column: x => x.ModulationTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "ModulationTypes",
                        principalColumn: "ModulationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_OperationStatuses_OperationalStatusID",
                        column: x => x.OperationalStatusID,
                        principalSchema: "canada_importer",
                        principalTable: "OperationStatuses",
                        principalColumn: "OperationalStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_PolarizationTypes_PolarizationTypeID",
                        column: x => x.PolarizationTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "PolarizationTypes",
                        principalColumn: "PolarizationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_Provinces_ProvinceID",
                        column: x => x.ProvinceID,
                        principalSchema: "canada_importer",
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_RegulatoryServices_RegulatoryServiceID",
                        column: x => x.RegulatoryServiceID,
                        principalSchema: "canada_importer",
                        principalTable: "RegulatoryServices",
                        principalColumn: "RegulatoryServiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_ServiceTypes_ServiceTypeID",
                        column: x => x.ServiceTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "ServiceTypes",
                        principalColumn: "ServiceTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StandbyTransmitterInformation_StandbyTransmi~",
                        column: x => x.StandbyTransmitterInformationID,
                        principalSchema: "canada_importer",
                        principalTable: "StandbyTransmitterInformation",
                        principalColumn: "StandbyTransmitterInformationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationClasses_StationClassID",
                        column: x => x.StationClassID,
                        principalSchema: "canada_importer",
                        principalTable: "StationClasses",
                        principalColumn: "StationClassID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationCostCategories_StationCostCategoryID",
                        column: x => x.StationCostCategoryID,
                        principalSchema: "canada_importer",
                        principalTable: "StationCostCategories",
                        principalColumn: "StationCostCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationFunctionTypes_StationFunctionID",
                        column: x => x.StationFunctionID,
                        principalSchema: "canada_importer",
                        principalTable: "StationFunctionTypes",
                        principalColumn: "StationFunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_StationTypes_StationTypeID",
                        column: x => x.StationTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "StationTypes",
                        principalColumn: "StationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseRecords_SubserviceTypes_SubserviceTypeID",
                        column: x => x.SubserviceTypeID,
                        principalSchema: "canada_importer",
                        principalTable: "SubserviceTypes",
                        principalColumn: "SubserviceTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LicenseRecordsHistory",
                schema: "canada_importer",
                columns: table => new
                {
                    LicenseRecordHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanadaLicenseRecordID = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    ChangeType = table.Column<int>(type: "integer", nullable: false),
                    EditedByImportJobID = table.Column<int>(type: "integer", nullable: true),
                    EditedByUserID = table.Column<int>(type: "integer", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseRecordsHistory", x => x.LicenseRecordHistoryId);
                    table.ForeignKey(
                        name: "FK_LicenseRecordsHistory_ImportJobs_EditedByImportJobID",
                        column: x => x.EditedByImportJobID,
                        principalSchema: "canada_importer",
                        principalTable: "ImportJobs",
                        principalColumn: "ImportJobID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LicenseRecordsHistory_LicenseRecords_CanadaLicenseRecordID_~",
                        columns: x => new { x.CanadaLicenseRecordID, x.Version },
                        principalSchema: "canada_importer",
                        principalTable: "LicenseRecords",
                        principalColumns: new[] { "CanadaLicenseRecordID", "Version" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportJobs_Status",
                schema: "canada_importer",
                table: "ImportJobs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_AnalogDigitalID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "AnalogDigitalID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_AntennaPatternID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "AntennaPatternID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_AuthorizationStatusID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "AuthorizationStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_CanadaLicenseRecordID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "CanadaLicenseRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_CommunicationTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "CommunicationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ConformityFrequencyPlanID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "ConformityFrequencyPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_CongestionZoneTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "CongestionZoneTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_FiltrationInstalledTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "FiltrationInstalledTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_FrequencyMHz",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "FrequencyMHz");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "IsValid");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid_CallSign",
                schema: "canada_importer",
                table: "LicenseRecords",
                columns: new[] { "IsValid", "CallSign" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid_CanadaLicenseRecordID",
                schema: "canada_importer",
                table: "LicenseRecords",
                columns: new[] { "IsValid", "CanadaLicenseRecordID" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_IsValid_FrequencyMHz",
                schema: "canada_importer",
                table: "LicenseRecords",
                columns: new[] { "IsValid", "FrequencyMHz" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ITUClassTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "ITUClassTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_LicenseTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "LicenseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ModulationTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "ModulationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_OperationalStatusID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "OperationalStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_PolarizationTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "PolarizationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ProvinceID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_RegulatoryServiceID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "RegulatoryServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_ServiceTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "ServiceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StandbyTransmitterInformationID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "StandbyTransmitterInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationClassID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "StationClassID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationCostCategoryID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "StationCostCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationFunctionID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "StationFunctionID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_StationTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "StationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_SubserviceTypeID",
                schema: "canada_importer",
                table: "LicenseRecords",
                column: "SubserviceTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_CanadaLicenseRecordID",
                schema: "canada_importer",
                table: "LicenseRecordsHistory",
                column: "CanadaLicenseRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_CanadaLicenseRecordID_Version",
                schema: "canada_importer",
                table: "LicenseRecordsHistory",
                columns: new[] { "CanadaLicenseRecordID", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_EditedByImportJobID",
                schema: "canada_importer",
                table: "LicenseRecordsHistory",
                column: "EditedByImportJobID");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecordsHistory_EditedByImportJobID_ChangeType",
                schema: "canada_importer",
                table: "LicenseRecordsHistory",
                columns: new[] { "EditedByImportJobID", "ChangeType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportJobChunkFiles",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "ImportJobStats",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "LicenseRecordsHistory",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "ImportJobs",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "LicenseRecords",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "AnalogDigital",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "AntennaPatterns",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "AuthorizationStatuses",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "CommunicationTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "ConformityFrequencyPlans",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "CongestionZoneTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "FiltrationInstalledTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "ITUClassTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "LicenseTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "ModulationTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "OperationStatuses",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "PolarizationTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "Provinces",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "RegulatoryServices",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "ServiceTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "StandbyTransmitterInformation",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "StationClasses",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "StationCostCategories",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "StationFunctionTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "StationTypes",
                schema: "canada_importer");

            migrationBuilder.DropTable(
                name: "SubserviceTypes",
                schema: "canada_importer");
        }
    }
}
