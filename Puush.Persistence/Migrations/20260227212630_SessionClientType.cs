using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puush.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SessionClientType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientType",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientType",
                table: "Sessions");
        }
    }
}
