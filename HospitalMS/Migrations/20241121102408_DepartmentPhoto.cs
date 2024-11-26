using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMS.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Departments");

          
        }
    }
}
