using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Managment_System.EF.Migrations
{
    /// <inheritdoc />
    public partial class nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModels_Teams_TeamId",
                table: "TaskModels");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TaskModels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModels_Teams_TeamId",
                table: "TaskModels",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModels_Teams_TeamId",
                table: "TaskModels");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TaskModels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModels_Teams_TeamId",
                table: "TaskModels",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
