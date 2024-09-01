using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Managment_System.EF.Migrations
{
    /// <inheritdoc />
    public partial class nullableUsignedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModels_AspNetUsers_AssignedUserId",
                table: "TaskModels");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedUserId",
                table: "TaskModels",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModels_AspNetUsers_AssignedUserId",
                table: "TaskModels",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModels_AspNetUsers_AssignedUserId",
                table: "TaskModels");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedUserId",
                table: "TaskModels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModels_AspNetUsers_AssignedUserId",
                table: "TaskModels",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
