using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations
{
    public partial class AddedCases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    CaseID = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    WorkerType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.CaseID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cases");
        }
    }
}
