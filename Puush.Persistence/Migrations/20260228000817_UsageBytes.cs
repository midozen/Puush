using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puush.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UsageBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SizeBytes",
                table: "Uploads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UsageBytes",
                table: "Accounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeBytes",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "UsageBytes",
                table: "Accounts");
        }
    }
}
