using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class FriendsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Friends_FriendId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FriendId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "FriendUserId",
                table: "Friends",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendUserId",
                table: "Friends",
                column: "FriendUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FriendUserId",
                table: "Friends",
                column: "FriendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FriendUserId",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_FriendUserId",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "FriendUserId",
                table: "Friends");

            migrationBuilder.AddColumn<Guid>(
                name: "FriendId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FriendId",
                table: "AspNetUsers",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Friends_FriendId",
                table: "AspNetUsers",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "Id");
        }
    }
}
