using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyUser.Migrations
{
    /// <inheritdoc />
    public partial class UsernameAndPasswordAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Backpacks");
            migrationBuilder.Sql("DELETE FROM Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "FirstName", "LastName", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("340d235d-a94d-41e7-9af4-05aae2df2120"), 0, "Jake", "Esh", "User22", "User2" },
                    { new Guid("3b87b87c-808b-4bc7-a3b6-3e98bfba9e2a"), 0, "Jahonger", "Ahmedov", "User11", "User1" },
                    { new Guid("dabc6326-02f5-4f10-95f3-3995da0ca8f9"), 0, "Rasul", "Azimov", "User33", "User3" }
                });

            migrationBuilder.InsertData(
                table: "Backpacks",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("5593aafe-f4bd-40e0-a5ea-5a85eba786ef"), "Second", new Guid("3b87b87c-808b-4bc7-a3b6-3e98bfba9e2a") },
                    { new Guid("763b50f2-b262-4be1-9986-c88f5495cfca"), "First", new Guid("3b87b87c-808b-4bc7-a3b6-3e98bfba9e2a") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("08d1aab3-6119-4868-a4fb-1e18c1317531"), 0, "Jake", "Esh" },
                    { new Guid("192933a1-ef16-4f6b-826d-e7c2fb7de4e0"), 0, "Jahonger", "Ahmedov" },
                    { new Guid("cb32b7cf-bbc5-483c-82ca-0817fe54e030"), 0, "Rasul", "Azimov" }
                });

            migrationBuilder.InsertData(
                table: "Backpacks",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("5b62a464-7a18-41a4-8d4e-27da2923eefc"), "Second", new Guid("192933a1-ef16-4f6b-826d-e7c2fb7de4e0") },
                    { new Guid("a3a7e2b1-d94c-49cc-86eb-91a563b9b8c2"), "First", new Guid("192933a1-ef16-4f6b-826d-e7c2fb7de4e0") }
                });
        }
    }
}
