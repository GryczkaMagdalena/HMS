using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelManagementSystem.Migrations.Identity
{
    public partial class example : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Rooms_RoomID",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Tasks",
                newName: "ReceiverID");

            migrationBuilder.RenameColumn(
                name: "ListenerId",
                table: "Tasks",
                newName: "ListenerID");

            migrationBuilder.RenameColumn(
                name: "IssuerId",
                table: "Tasks",
                newName: "IssuerID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ReceiverId",
                table: "Tasks",
                newName: "IX_Tasks_ReceiverID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ListenerId",
                table: "Tasks",
                newName: "IX_Tasks_ListenerID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_IssuerId",
                table: "Tasks",
                newName: "IX_Tasks_IssuerID");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomID",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_IssuerID",
                table: "Tasks",
                column: "IssuerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ListenerID",
                table: "Tasks",
                column: "ListenerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ReceiverID",
                table: "Tasks",
                column: "ReceiverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Rooms_RoomID",
                table: "Tasks",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_IssuerID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ListenerID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ReceiverID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Rooms_RoomID",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ReceiverID",
                table: "Tasks",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "ListenerID",
                table: "Tasks",
                newName: "ListenerId");

            migrationBuilder.RenameColumn(
                name: "IssuerID",
                table: "Tasks",
                newName: "IssuerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ReceiverID",
                table: "Tasks",
                newName: "IX_Tasks_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ListenerID",
                table: "Tasks",
                newName: "IX_Tasks_ListenerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_IssuerID",
                table: "Tasks",
                newName: "IX_Tasks_IssuerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomID",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(Guid));

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Rooms_RoomID",
                table: "Tasks",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
