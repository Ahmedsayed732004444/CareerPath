using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPath.Migrations
{
    /// <inheritdoc />
    public partial class prepearRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevockedOn",
                table: "RefreshToken",
                newName: "RevokedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevokedOn",
                table: "RefreshToken",
                newName: "RevockedOn");
        }
    }
}
