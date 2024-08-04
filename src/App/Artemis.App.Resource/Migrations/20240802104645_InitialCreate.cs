using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Artemis.App.Resource.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Resource");

            migrationBuilder.CreateTable(
                name: "ArtemisDataDictionary",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "字典名称"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "字典代码"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "字典描述"),
                    Type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "字典类型"),
                    Valid = table.Column<bool>(type: "boolean", nullable: false, comment: "字典是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisDataDictionary", x => x.Id);
                },
                comment: "数据字典数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisDevice",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "设备名称"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "设备类型"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "设备代码"),
                    Model = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "设备型号"),
                    SerialNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "设备序列号"),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "设备状态"),
                    PurchaseDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "购买日期"),
                    InstallDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "安装日期"),
                    WarrantyDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "保修日期"),
                    MaintenanceDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "维护日期"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "设备描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisDevice", x => x.Id);
                },
                comment: "设备数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisDivision",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "行政区划名称"),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "行政区划代码"),
                    Level = table.Column<int>(type: "integer", nullable: false, comment: "行政区划级别"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "行政区划类型"),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "行政区划全名"),
                    Pinyin = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "行政区划拼音"),
                    Remark = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "行政区划备注"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "上级行政区划标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisDivision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisDivision_ArtemisDivision",
                        column: x => x.ParentId,
                        principalSchema: "Resource",
                        principalTable: "ArtemisDivision",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "行政区划数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisOrganization",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "机构名称"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "机构编码"),
                    DesignCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "设计编码"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "机构类型"),
                    EstablishTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "机构成立时间"),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "机构邮箱"),
                    WebSite = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "机构网站"),
                    ContactNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "机构联系电话"),
                    PostCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "机构邮编"),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "机构状态"),
                    DivisionCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "机构地址"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "机构描述"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "父级机构标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisOrganization_ArtemisOrganization",
                        column: x => x.ParentId,
                        principalSchema: "Resource",
                        principalTable: "ArtemisOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "组织机构数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisDataDictionaryItem",
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
                    DataDictionaryId = table.Column<Guid>(type: "uuid", nullable: false, comment: "数据字典标识"),
                    Key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "数据项目键"),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "数据项目值"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "数据项目描述"),
                    Valid = table.Column<bool>(type: "boolean", nullable: false, comment: "数据项目是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisDataDictionaryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisDataDictionaryItem_ArtemisDataDictionary",
                        column: x => x.DataDictionaryId,
                        principalSchema: "Resource",
                        principalTable: "ArtemisDataDictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "数据字典项目数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_Code",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_CreateBy",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_CreatedAt",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_DeletedAt",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_ModifyBy",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_Name",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_RemoveBy",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionary_UpdatedAt",
                schema: "Resource",
                table: "ArtemisDataDictionary",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_CreateBy",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_CreatedAt",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_DataDictionaryId_Key",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                columns: new[] { "DataDictionaryId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_DeletedAt",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_Key",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_ModifyBy",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_RemoveBy",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDataDictionaryItem_UpdatedAt",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDevice_CreateBy",
                schema: "Resource",
                table: "ArtemisDevice",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDevice_CreatedAt",
                schema: "Resource",
                table: "ArtemisDevice",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDevice_DeletedAt",
                schema: "Resource",
                table: "ArtemisDevice",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDevice_ModifyBy",
                schema: "Resource",
                table: "ArtemisDevice",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDevice_RemoveBy",
                schema: "Resource",
                table: "ArtemisDevice",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDevice_UpdatedAt",
                schema: "Resource",
                table: "ArtemisDevice",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_CreateBy",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_CreatedAt",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_DeletedAt",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_ModifyBy",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_ParentId",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_RemoveBy",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_UpdatedAt",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_CreateBy",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_CreatedAt",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_DeletedAt",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_ModifyBy",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_ParentId",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_RemoveBy",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganization_UpdatedAt",
                schema: "Resource",
                table: "ArtemisOrganization",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisDataDictionaryItem",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ArtemisDevice",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ArtemisDivision",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ArtemisOrganization",
                schema: "Resource");

            migrationBuilder.DropTable(
                name: "ArtemisDataDictionary",
                schema: "Resource");
        }
    }
}
