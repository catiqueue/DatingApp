using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class BigRefucktor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Groups_DbGroupName",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_DbUserId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "DbUserId",
                table: "Photos",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_DbUserId",
                table: "Photos",
                newName: "IX_Photos_UserId");

            migrationBuilder.RenameColumn(
                name: "DbGroupName",
                table: "Connections",
                newName: "GroupName");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_DbGroupName",
                table: "Connections",
                newName: "IX_Connections_GroupName");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Photos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Groups_GroupName",
                table: "Connections",
                column: "GroupName",
                principalTable: "Groups",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_UserId",
                table: "Photos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Groups_GroupName",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_UserId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Photos",
                newName: "DbUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                newName: "IX_Photos_DbUserId");

            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "Connections",
                newName: "DbGroupName");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_GroupName",
                table: "Connections",
                newName: "IX_Connections_DbGroupName");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Groups_DbGroupName",
                table: "Connections",
                column: "DbGroupName",
                principalTable: "Groups",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_DbUserId",
                table: "Photos",
                column: "DbUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
