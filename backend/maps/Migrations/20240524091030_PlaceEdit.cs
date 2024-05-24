using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace maps.Migrations
{
    public partial class PlaceEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_routes_routeid",
                table: "feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_users_userid",
                table: "feedbacks");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "feedbacks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "routeid",
                table: "feedbacks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_routes_routeid",
                table: "feedbacks",
                column: "routeid",
                principalTable: "routes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_users_userid",
                table: "feedbacks",
                column: "userid",
                principalTable: "users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_routes_routeid",
                table: "feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_users_userid",
                table: "feedbacks");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "feedbacks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "routeid",
                table: "feedbacks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_routes_routeid",
                table: "feedbacks",
                column: "routeid",
                principalTable: "routes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_users_userid",
                table: "feedbacks",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
