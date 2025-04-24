using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SelfServiceKioskSystem.Migrations
{
    /// <inheritdoc />
    public partial class RoleUpdateAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Categories]");
            migrationBuilder.Sql("DELETE FROM [Roles]");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserID",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserID",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Hot Beverages" },
                    { 2, "Cold Drinks" },
                    { 3, "Snacks" },
                    { 4, "Hot Meals" },
                    { 5, "Desserts" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "UserRole" },
                columnTypes: new[] { "int", "nvarchar(max)" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Superuser" }
    });


            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.Sql("UPDATE Users SET RoleID = 1 WHERE RoleID = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AlterColumn<int>(
                name: "RoleID",
                table: "Users",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleID",
                keyValue: 1,
                column: "UserID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleID",
                keyValue: 2,
                column: "UserID",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserID",
                table: "Roles",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserID",
                table: "Roles",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
