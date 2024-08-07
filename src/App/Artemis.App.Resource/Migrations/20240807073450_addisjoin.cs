using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Resource.Migrations
{
    /// <inheritdoc />
    public partial class addisjoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "Resource",
                table: "ArtemisSystemModule",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "模块类型",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "模块类型");

            migrationBuilder.AddColumn<bool>(
                name: "IsJoin",
                schema: "Resource",
                table: "ArtemisOrganization",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否接入");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJoin",
                schema: "Resource",
                table: "ArtemisOrganization");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "Resource",
                table: "ArtemisSystemModule",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                comment: "模块类型",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "模块类型");
        }
    }
}
