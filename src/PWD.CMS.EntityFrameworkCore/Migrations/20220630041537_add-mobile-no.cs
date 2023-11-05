using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWD.CMS.Migrations
{
    public partial class addmobileno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "Otps");
        }
    }
}
