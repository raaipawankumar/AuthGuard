using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthGuard.Host.Migrations
{
    /// <inheritdoc />
    public partial class addissuperadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperAdmin",
                table: "AspNetRoles",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuperAdmin",
                table: "AspNetRoles");
        }
    }
}
