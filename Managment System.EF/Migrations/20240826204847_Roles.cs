using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Managment_System.EF.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
       table: "AspNetRoles",
       columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
       values: new object[] { Guid.NewGuid().ToString(), "User", "User", Guid.NewGuid().ToString() }
   );

            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Leader", "Leader", Guid.NewGuid().ToString() }
            );

            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin", Guid.NewGuid().ToString() }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM [AspNetRoles ]");
        }
    }
}
