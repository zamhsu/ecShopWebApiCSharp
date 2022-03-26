using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApi.Migrations
{
    public partial class AddAdminMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminMemberStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMemberStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Account = table.Column<string>(type: "text", nullable: false),
                    Pwd = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    ErrorTimes = table.Column<int>(type: "integer", nullable: false),
                    LastLoginDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsMaster = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    HashSalt = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminMember_StatusId_To_AdminMemberStatus_Id",
                        column: x => x.StatusId,
                        principalTable: "AdminMemberStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminMember_Guid",
                table: "AdminMember",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminMember_StatusId",
                table: "AdminMember",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminMember");

            migrationBuilder.DropTable(
                name: "AdminMemberStatus");
        }
    }
}
