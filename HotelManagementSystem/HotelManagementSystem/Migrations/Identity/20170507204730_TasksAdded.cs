using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class TasksAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Room_RoomID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GuestFirstName",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "GuestLastName",
                table: "Room",
                newName: "UserID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Room",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    ShiftID = table.Column<Guid>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.ShiftID);
                    table.ForeignKey(
                        name: "FK_Shift_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_UserID",
                table: "Room",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shift_UserId",
                table: "Shift",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_AspNetUsers_UserID",
                table: "Room",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_AspNetUsers_UserID",
                table: "Room");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropIndex(
                name: "IX_Room_UserID",
                table: "Room");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Room",
                newName: "GuestLastName");

            migrationBuilder.AlterColumn<string>(
                name: "GuestLastName",
                table: "Room",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestFirstName",
                table: "Room",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomID",
                table: "AspNetUsers",
                nullable: true);

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
    }
}
