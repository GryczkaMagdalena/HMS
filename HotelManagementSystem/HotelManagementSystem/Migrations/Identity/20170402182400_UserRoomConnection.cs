using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class UserRoomConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomID = table.Column<Guid>(nullable: false),
                    GuestFirstName = table.Column<string>(nullable: true),
                    GuestLastName = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomID",
                table: "AspNetUsers",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Room_RoomID",
                table: "AspNetUsers",
                column: "RoomID",
                principalTable: "Room",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Room_RoomID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "AspNetUsers");
        }
    }
}
