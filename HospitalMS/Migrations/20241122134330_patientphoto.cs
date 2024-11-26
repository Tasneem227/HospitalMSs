using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMS.Migrations
{
    /// <inheritdoc />
    public partial class patientphoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imag",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imag",
                table: "Patients");
        }
    }
}
