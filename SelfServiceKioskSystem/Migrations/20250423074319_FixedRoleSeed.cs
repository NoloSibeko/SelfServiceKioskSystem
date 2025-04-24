using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfServiceKioskSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixedRoleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Roles",
                newName: "RoleName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "Roles",
                newName: "UserRole");
        }
    }
}
