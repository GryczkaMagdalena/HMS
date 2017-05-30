using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class AddedPriorityToTask3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Cases_CaseID",
                table: "Tasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeOfCreation",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CaseID",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Cases_CaseID",
                table: "Tasks",
                column: "CaseID",
                principalTable: "Cases",
                principalColumn: "CaseID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Cases_CaseID",
                table: "Tasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeOfCreation",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<Guid>(
                name: "CaseID",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Cases_CaseID",
                table: "Tasks",
                column: "CaseID",
                principalTable: "Cases",
                principalColumn: "CaseID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
