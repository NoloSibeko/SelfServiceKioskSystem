using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfServiceKioskSystem.Migrations
{
    /// <inheritdoc />
    public partial class RoleSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
    name: "ContactNumber",
    table: "Users",
    nullable: false,
    defaultValue: "0000000000",
    oldClrType: typeof(string),
    oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
