using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class PleaseWorkThisTime_IssuerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuerId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListenerId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IssuerId",
                table: "Tasks",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ListenerId",
                table: "Tasks",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ReceiverId",
                table: "Tasks",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_IssuerId",
                table: "Tasks",
                column: "IssuerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ListenerId",
                table: "Tasks",
                column: "ListenerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ReceiverId",
                table: "Tasks",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_IssuerId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ListenerId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ReceiverId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_IssuerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ListenerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ReceiverId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ListenerId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Tasks");
        }
    }
}
