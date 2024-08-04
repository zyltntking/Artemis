using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Resource.Migrations
{
    /// <inheritdoc />
    public partial class AddStandardRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtemisStandardItem_ArtemisStandardCatalog",
                schema: "Resource",
                table: "ArtemisStandardItem");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisStandardItem_CatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem");

            migrationBuilder.AlterColumn<string>(
                name: "Template",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "标准项目模板",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "标准项目名称",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Minimum",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "标准项目最小值",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Maximum",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "标准项目最大值",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "标准项目描述",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "标准项目编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StandardCatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "标准目录标识");

            migrationBuilder.AlterColumn<bool>(
                name: "Valid",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "boolean",
                nullable: false,
                comment: "是否生效",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "标准目录类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "标准目录状态",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "标准目录名称",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "标准目录描述",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "标准目录编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_StandardCatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "StandardCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtemisStandardItem_ArtemisStandardCatalog",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "StandardCatalogId",
                principalSchema: "Resource",
                principalTable: "ArtemisStandardCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtemisStandardItem_ArtemisStandardCatalog",
                schema: "Resource",
                table: "ArtemisStandardItem");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisStandardItem_StandardCatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem");

            migrationBuilder.DropColumn(
                name: "StandardCatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem");

            migrationBuilder.AlterColumn<string>(
                name: "Template",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "标准项目模板");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "标准项目名称");

            migrationBuilder.AlterColumn<string>(
                name: "Minimum",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "标准项目最小值");

            migrationBuilder.AlterColumn<string>(
                name: "Maximum",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "标准项目最大值");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "标准项目描述");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "标准项目编码");

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<bool>(
                name: "Valid",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否生效");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "标准目录类型");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "标准目录状态");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "标准目录名称");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "标准目录描述");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "标准目录编码");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_CatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "CatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtemisStandardItem_ArtemisStandardCatalog",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "CatalogId",
                principalSchema: "Resource",
                principalTable: "ArtemisStandardCatalog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
