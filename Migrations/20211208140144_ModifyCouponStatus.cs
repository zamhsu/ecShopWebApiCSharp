using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApi.Migrations
{
    public partial class ModifyCouponStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Coupon",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CouponStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_StatusId",
                table: "Coupon",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_StatusId_To_CouponStatus_Id",
                table: "Coupon",
                column: "StatusId",
                principalTable: "CouponStatus",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_StatusId_To_CouponStatus_Id",
                table: "Coupon");

            migrationBuilder.DropTable(
                name: "CouponStatus");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_StatusId",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Coupon");
        }
    }
}
