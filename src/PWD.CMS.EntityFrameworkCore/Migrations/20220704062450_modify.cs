using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWD.CMS.Migrations
{
    public partial class modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllotmentHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllotmentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    PwdTenantId = table.Column<int>(type: "int", nullable: false),
                    AllotmentId = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostAllotment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostPhotos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreAllotment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrePhotos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllotmentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllotmentHistories_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllotmentHistories_PwdTenants_PwdTenantId",
                        column: x => x.PwdTenantId,
                        principalTable: "PwdTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllotmentHistories_ApartmentId",
                table: "AllotmentHistories",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AllotmentHistories_PwdTenantId",
                table: "AllotmentHistories",
                column: "PwdTenantId");
        }
    }
}
