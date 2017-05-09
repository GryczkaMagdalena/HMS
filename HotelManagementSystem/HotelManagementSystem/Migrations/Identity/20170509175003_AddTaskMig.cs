using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class AddTaskMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CaseID",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CaseID",
                table: "Tasks",
                column: "CaseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Cases_CaseID",
                table: "Tasks",
                column: "CaseID",
                principalTable: "Cases",
                principalColumn: "CaseID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Cases_CaseID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CaseID",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CaseID",
                table: "Tasks");
        }
    }
}
