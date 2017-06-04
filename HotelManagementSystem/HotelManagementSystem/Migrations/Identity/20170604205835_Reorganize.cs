using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class Reorganize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_UserID",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Shifts",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_UserID",
                table: "Shifts",
                newName: "IX_Shifts_WorkerId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Shifts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ManagerId",
                table: "Shifts",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_ManagerId",
                table: "Shifts",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_WorkerId",
                table: "Shifts",
                column: "WorkerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_ManagerId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_WorkerId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ManagerId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Shifts",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_WorkerId",
                table: "Shifts",
                newName: "IX_Shifts_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_UserID",
                table: "Shifts",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
