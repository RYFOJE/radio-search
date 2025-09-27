using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Radio_Search.Importer.Canada.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSpatialIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE INDEX IF NOT EXISTS idx_locations_location ON ""canada_importer"".""LicenseRecords"" USING GIST (""Location"");"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_locations_location;");
        }
    }
}
