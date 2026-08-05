using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHire.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesToOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Offers");
        }
    }
}
