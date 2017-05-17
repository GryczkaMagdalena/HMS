using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class MigrateOptionalKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_UserID",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_UserID",
                table: "Rooms",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomID",
                table: "AspNetUsers",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomID",
                table: "AspNetUsers",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_UserID",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_UserID",
                table: "Rooms",
                column: "UserID",
                unique: true);
        }
    }
}
