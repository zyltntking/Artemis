using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CancelFlag",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "取消筛查标识",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "取消筛查标识");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否取消筛查");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.AlterColumn<string>(
                name: "CancelFlag",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "取消筛查标识",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "取消筛查标识");
        }
    }
}
