using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWD.CMS.Migrations
{
    public partial class tenantfeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantFeedback",
                table: "Complains",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantFeedback",
                table: "Complains");
        }
    }
}
