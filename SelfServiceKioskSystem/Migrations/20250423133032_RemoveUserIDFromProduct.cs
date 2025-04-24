using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfServiceKioskSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIDFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserID",
                table: "Products");*/

           /* migrationBuilder.DropIndex(
                name: "IX_Products_UserID",
                table: "Products");*/

            /*migrationBuilder.DropColumn(
                name: "UserID",
                table: "Products");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserID",
                table: "Products",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserID",
                table: "Products",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
