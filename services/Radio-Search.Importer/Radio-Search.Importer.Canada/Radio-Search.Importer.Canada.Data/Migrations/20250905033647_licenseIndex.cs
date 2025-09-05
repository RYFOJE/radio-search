using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Radio_Search.Importer.Canada.Data.Migrations
{
    /// <inheritdoc />
    public partial class licenseIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseRecords_CanadaLicenseRecordID",
                schema: "Canada_Importer",
                table: "LicenseRecords",
                column: "CanadaLicenseRecordID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseRecords_CanadaLicenseRecordID",
                schema: "Canada_Importer",
                table: "LicenseRecords");
        }
    }
}
