using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Radio_Search.Importer.Canada.Data.Migrations
{
    /// <inheritdoc />
    public partial class indexingOnStringSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE EXTENSION IF NOT EXISTS pg_trgm;"
            );

            migrationBuilder.Sql(
                @"CREATE INDEX IF NOT EXISTS idx_licenses_licenseeName ON canada_importer.""LicenseRecords"" USING GIN (""LicenseeName"" gin_trgm_ops);"
            );

            migrationBuilder.Sql(
                @"CREATE INDEX IF NOT EXISTS idx_license_stationReference ON canada_importer.""LicenseRecords"" USING GIN (""StationReference"" gin_trgm_ops);"
            );


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_license_stationReference;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_licenses_licenseeName;");
            migrationBuilder.Sql(@"DROP EXTENSION IF EXISTS pg_trgm;");
        }
    }
}
