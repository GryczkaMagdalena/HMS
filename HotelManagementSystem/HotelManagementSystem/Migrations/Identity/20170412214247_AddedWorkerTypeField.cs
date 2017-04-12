using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class AddedWorkerTypeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Occupied",
                table: "Room",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Occupied",
                table: "Room");
        }
    }
}
