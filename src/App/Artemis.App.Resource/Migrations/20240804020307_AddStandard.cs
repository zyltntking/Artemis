using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Resource.Migrations
{
    /// <inheritdoc />
    public partial class AddStandard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "数据项目描述",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "数据项目描述");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "字典类型",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "字典类型");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "字典描述",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "字典描述");

            migrationBuilder.CreateTable(
                name: "ArtemisStandardCatalog",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "删除者标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Valid = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStandardCatalog", x => x.Id);
                },
                comment: "标准目录数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisStandardItem",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "删除者标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    CatalogId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Minimum = table.Column<string>(type: "text", nullable: false),
                    Maximum = table.Column<string>(type: "text", nullable: false),
                    Template = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStandardItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisStandardItem_ArtemisStandardCatalog",
                        column: x => x.CatalogId,
                        principalSchema: "Resource",
                        principalTable: "ArtemisStandardCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "标准项目数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_Code",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_DesignCode",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "DesignCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_Name",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_Status",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_Type",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_Code",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_Level",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_Name",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_Type",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_Code",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_CreateBy",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_CreatedAt",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_DeletedAt",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_ModifyBy",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_Name",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_RemoveBy",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_Type",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardCatalog_UpdatedAt",
                schema: "Resource",
                table: "ArtemisStandardCatalog",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_CatalogId",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_Code",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_CreateBy",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_CreatedAt",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_DeletedAt",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_ModifyBy",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_Name",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_RemoveBy",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStandardItem_UpdatedAt",
                schema: "Resource",
                table: "ArtemisStandardItem",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisStandardItem",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ArtemisStandardCatalog",
                schema: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisOrganization_Code",
                schema: "Resource",
                table: "ArtemisOrganization");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisOrganization_DesignCode",
                schema: "Resource",
                table: "ArtemisOrganization");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisOrganization_Name",
                schema: "Resource",
                table: "ArtemisOrganization");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisOrganization_Status",
                schema: "Resource",
                table: "ArtemisOrganization");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisOrganization_Type",
                schema: "Resource",
                table: "ArtemisOrganization");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisDivision_Code",
                schema: "Resource",
                table: "ArtemisDivision");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisDivision_Level",
                schema: "Resource",
                table: "ArtemisDivision");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisDivision_Name",
                schema: "Resource",
                table: "ArtemisDivision");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisDivision_Type",
                schema: "Resource",
                table: "ArtemisDivision");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "数据项目描述",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "数据项目描述");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "字典类型",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "字典类型");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "字典描述",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "字典描述");
        }
    }
}
