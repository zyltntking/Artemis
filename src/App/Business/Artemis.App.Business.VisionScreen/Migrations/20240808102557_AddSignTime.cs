using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddSignTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UserSignTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "用户签名时间");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSignTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");
        }
    }
}
