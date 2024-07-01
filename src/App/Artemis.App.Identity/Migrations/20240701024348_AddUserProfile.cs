using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityUserProfile",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "用户档案数据键"),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "用户档案数据值")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserProfile", x => new { x.UserId, x.Key });
                    table.ForeignKey(
                        name: "FK_IdentityUserProfile_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色档案数据集");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUserProfile",
                schema: "identity");
        }
    }
}
