using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddSignFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSign",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否签名");

            migrationBuilder.AddColumn<string>(
                name: "UserSign",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "用户签名");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSign",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "UserSign",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");
        }
    }
}
