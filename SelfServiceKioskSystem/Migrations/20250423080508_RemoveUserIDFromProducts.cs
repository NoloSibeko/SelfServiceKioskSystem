using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfServiceKioskSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIDFromProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UserID", 
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserID", 
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Products");

          /*  migrationBuilder.Sql(@"
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'bonolo@singular.co.za')
    BEGIN
        INSERT INTO Users (Name, Email, RoleID)
        VALUES ('Bonolo', 'bonolo@singular.co.za', 2)
    END
    ELSE
    BEGIN
        UPDATE Users
        SET RoleID = 2
        WHERE Email = 'bonolo@singular.co.za'
    END
");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Products",
                type: "int",
                nullable: true); 

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserID",
                table: "Products",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UserID",
                table: "Products",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
