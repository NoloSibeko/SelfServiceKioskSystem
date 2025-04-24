using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfServiceKioskSystem.Migrations
{
    /// <inheritdoc />
    public partial class SuperUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AccountStatus", "ContactNumber", "Email", "Name", "Password", "RoleID", "Surname" },
                values: new object[] { 10, "Active", "1111111111", "superuser@singular.co.za", "SuperU", "$up3rUser", 2, "SUser" });
        }
    }
}
