using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminAPI.Migrations
{
    /// <inheritdoc />
    public partial class adminDbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_approvals_admins_AdminId",
                table: "approvals");

            migrationBuilder.DropIndex(
                name: "IX_approvals_AdminId",
                table: "approvals");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "approvals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "admins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_admins_AdminId",
                table: "admins",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_admins_admins_AdminId",
                table: "admins",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_admins_admins_AdminId",
                table: "admins");

            migrationBuilder.DropIndex(
                name: "IX_admins_AdminId",
                table: "admins");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "admins");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "approvals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_approvals_AdminId",
                table: "approvals",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_approvals_admins_AdminId",
                table: "approvals",
                column: "AdminId",
                principalTable: "admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
