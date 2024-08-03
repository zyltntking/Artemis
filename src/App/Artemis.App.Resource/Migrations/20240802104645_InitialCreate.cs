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

            migrationBuilder.InsertData(
                schema: "Resource",
                table: "ArtemisDataDictionary",
                columns: new[] { "Id", "Code", "ConcurrencyStamp", "CreateBy", "CreatedAt", "DeletedAt", "Description", "ModifyBy", "Name", "RemoveBy", "Type", "UpdatedAt", "Valid" },
                values: new object[,]
                {
                    { new Guid("040346dc-1201-453b-906a-637d666f40f1"), "ClaimTypes", "2618f8b8-7472-4609-ac08-c8cae00586bd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8306), null, "凭据类型", "00000000-0000-0000-0000-000000000000", "ClaimTypes", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8323), true },
                    { new Guid("3db25ae6-6702-4e94-9636-de59e1fe347e"), "StudyPhase", "1b3b885a-9b35-467a-9fb1-3479858a779e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9416), null, "学段", "00000000-0000-0000-0000-000000000000", "StudyPhase", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9417), true },
                    { new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), "RegionLevel", "f5949da6-305c-48df-9156-92ceba10cac8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9280), null, "行政区划等级", "00000000-0000-0000-0000-000000000000", "RegionLevel", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9280), true },
                    { new Guid("5a7b37b4-ee09-4564-9a8b-6f55977dfd68"), "TaskMode", "fbdcf2bd-2b14-41fa-9753-3d2ce6297063", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9492), null, "任务模式", "00000000-0000-0000-0000-000000000000", "TaskMode", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9492), true },
                    { new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), "SchoolLength", "cc9e608d-c8c3-4cba-9ae4-6b06d7e34304", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9354), null, null, "00000000-0000-0000-0000-000000000000", "SchoolLength", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9354), true },
                    { new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), "ChineseNation", "f21f9830-5ae7-44c6-9d87-4459ccf82b88", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8630), null, "民族类型", "00000000-0000-0000-0000-000000000000", "ChineseNation", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8630), true },
                    { new Guid("9a14dc28-56c3-4714-a8dd-118acdcd3a26"), "DictionaryType", "8231e9f8-f455-4a0b-b4f4-a0aedacc5258", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9069), null, "字典类型", "00000000-0000-0000-0000-000000000000", "DictionaryType", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9069), true },
                    { new Guid("ac1d3f30-f182-4d95-ae41-3c207a82e70f"), "OrganizationStatus", "e49660fb-3953-454c-a2b4-f90781a6f77e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9593), null, "机构状态", "00000000-0000-0000-0000-000000000000", "OrganizationStatus", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9593), true },
                    { new Guid("baa6cd48-e0e9-4ab0-b047-a47e1c6b067d"), "TaskState", "c6c3a70b-4a85-446b-b356-c1afc4fb1f75", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9536), null, "任务状态", "00000000-0000-0000-0000-000000000000", "TaskState", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9536), true },
                    { new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), "EndType", "82f28f6f-0cd8-466a-a992-1478d46ce430", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9110), null, "端类型", "00000000-0000-0000-0000-000000000000", "EndType", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9110), true },
                    { new Guid("de169de5-c08c-486d-8933-7b4f597ee7b9"), "TaskShip", "af6f844a-7a32-4b6e-9c9f-868608bc90dc", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9514), null, "任务归属", "00000000-0000-0000-0000-000000000000", "TaskShip", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9514), true },
                    { new Guid("eff36a62-65df-42ff-a28e-b63e93d11de9"), "IdentityPolicy", "5fd56c9b-36aa-41b2-bee2-8932308b312f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9234), null, "认证策略", "00000000-0000-0000-0000-000000000000", "IdentityPolicy", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9235), true },
                    { new Guid("f42a15a1-ed32-4bd2-9746-07d58f1d4ad4"), "Gender", "78cc02fe-c075-40b3-bf8d-e736319ac239", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9196), null, "性别类型", "00000000-0000-0000-0000-000000000000", "Gender", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9196), true },
                    { new Guid("ff6e2096-327c-403b-b0b2-d6355470c1d7"), "OrganizationType", "0277aae3-5b5f-4151-b251-befaac55162a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9556), null, "机构类型", "00000000-0000-0000-0000-000000000000", "OrganizationType", null, "Public", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9556), true }
                });

            migrationBuilder.InsertData(
                schema: "Resource",
                table: "ArtemisOrganization",
                columns: new[] { "Id", "Address", "Code", "ConcurrencyStamp", "ContactNumber", "CreateBy", "CreatedAt", "DeletedAt", "Description", "DesignCode", "DivisionCode", "Email", "EstablishTime", "ModifyBy", "Name", "ParentId", "PostCode", "RemoveBy", "Status", "Type", "UpdatedAt", "WebSite" },
                values: new object[] { new Guid("116d61b4-bf6d-47c9-bce8-02c7a63081fa"), null, "ORG5325001", "7b051946-9985-4c18-ad6e-0ea3d25c1abd", null, "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9662), null, null, "ORG5325001", "5325", null, null, "00000000-0000-0000-0000-000000000000", "红河州教体局", null, null, null, "InOperation", "Management", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9662), null });

            migrationBuilder.InsertData(
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                columns: new[] { "Id", "ConcurrencyStamp", "CreateBy", "CreatedAt", "DataDictionaryId", "DeletedAt", "Description", "Key", "ModifyBy", "RemoveBy", "UpdatedAt", "Valid", "Value" },
                values: new object[,]
                {
                    { new Guid("00ec7d50-5a79-4009-8a08-74ea7dd7f2cd"), "587fd7a3-4711-4ea3-a40b-11c34d9e10a5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8580), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "设备名称凭据", "DeviceName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8581), true, "DeviceName" },
                    { new Guid("01f2f691-2c7f-47ad-9750-6c057538a216"), "68cd139a-c382-46f8-b902-6538ce57e100", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9056), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "壮族", "壮族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9057), true, "壮族" },
                    { new Guid("021b0bc2-41c1-4a49-a6f5-a0e9aefe6063"), "ad03def9-38ad-4516-a62d-357364f09aa2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9140), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "Web端", "Web", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9140), true, "Web" },
                    { new Guid("0310a4a0-342e-4b52-bbc4-3bfb304968f7"), "0a20e57b-0c4a-4d0e-9d60-a737cfedeec8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8739), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "俄罗斯族", "俄罗斯族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8739), true, "俄罗斯族" },
                    { new Guid("04a1a7a4-81d9-4790-89e8-9518a48f76ef"), "5e81b02c-50c2-4d74-946d-67c7a33b10f3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8880), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "蒙古族", "蒙古族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8881), true, "蒙古族" },
                    { new Guid("080c8c82-5143-486f-8f8f-d5274e6aa820"), "1429287d-5321-4efa-82b1-24c64d6929f6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9008), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "锡伯族", "锡伯族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9008), true, "锡伯族" },
                    { new Guid("0825ae99-0631-4262-bd0a-e560c227ff63"), "9e40dcfe-bcf6-4b47-873f-5a269d714d5b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8443), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "用户标识凭据", "UserId", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8443), true, "UserId" },
                    { new Guid("0870f8c1-227d-486e-b621-049c757cd75c"), "446994bd-a557-4e6a-9693-bb03ba85d27f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9485), new Guid("3db25ae6-6702-4e94-9636-de59e1fe347e"), null, "大学阶段", "University", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9486), true, "University" },
                    { new Guid("08b1476f-475e-41a1-890a-29e30f11782d"), "b9126c02-5081-475a-8267-ea4ece41317f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9347), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "街道级", "Street", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9348), true, "Street" },
                    { new Guid("0dcfb70f-b985-445b-b29b-0fe172620e48"), "cdc82392-89a5-48b9-9040-5aad4506cdc0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8696), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "布朗族", "布朗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8696), true, "布朗族" },
                    { new Guid("0dd622bc-0cd9-49c1-98e7-1e3fd5b7e301"), "7c63b7ff-07a6-4366-8a0f-6e5c3d357d8a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8735), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "鄂伦春族", "鄂伦春族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8736), true, "鄂伦春族" },
                    { new Guid("0fbc77ed-e035-490a-81d0-d8c32a7d6418"), "57e01f6b-a93d-4496-ab7a-2a273c807047", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8817), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "京族", "京族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8818), true, "京族" },
                    { new Guid("1029157b-bf1a-4aeb-bf49-d91a2983b5b5"), "4945f0c1-d50d-45a4-a6c5-f7952a722229", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8860), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "满族", "满族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8861), true, "满族" },
                    { new Guid("122c6058-7517-4971-85fc-b7536f6d5c9b"), "ffdf54f6-267c-4f42-bf67-107dd6ecbf01", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8660), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "阿昌族", "阿昌族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8660), true, "阿昌族" },
                    { new Guid("132951da-8b07-4b84-922c-008851aa6f45"), "f85c86ce-34fd-4324-b6f3-0eb6d92a6b2c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8899), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "仫佬族", "仫佬族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8900), true, "仫佬族" },
                    { new Guid("16161718-98a1-4064-9559-f07a2456ec66"), "62dc4734-50fe-4ebd-a73f-b04de7762109", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9394), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "五年制", "FiveYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9395), true, "FiveYears" },
                    { new Guid("1a556962-ab74-4916-a561-759d8a55284d"), "3524af54-e7c8-46fb-aec7-f9d7173f649e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8874), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "门巴族", "门巴族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8875), true, "门巴族" },
                    { new Guid("1b117f80-2be9-4ba7-84d5-bbfa25a18e45"), "b83d7f18-3fff-4ad4-aabf-60e85436875c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9270), new Guid("eff36a62-65df-42ff-a28e-b63e93d11de9"), null, "操作名策略", "ActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9271), true, "ActionName" },
                    { new Guid("201e512d-9067-4016-a97a-492a15520e69"), "5b8672b3-6669-4334-81f4-133a808d6fd8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9302), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "未知等级", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9303), true, "Unknown" },
                    { new Guid("2372226e-32b0-4f29-8f35-6de0652b7721"), "92f00587-988b-4d56-9c90-68479c641f90", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9386), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "三年制", "ThreeYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9386), true, "ThreeYears" },
                    { new Guid("2394f7de-e94e-4284-95ea-125b1e002063"), "b3d1bf6c-f436-453d-acb3-1bd63a7a832c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8709), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "达斡尔族", "达斡尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8710), true, "达斡尔族" },
                    { new Guid("23de1a8f-112a-4974-800e-91ac583b6f2b"), "f1e0ef38-bf93-4826-ab1e-80028d64ef09", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9398), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "六年制", "SixYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9399), true, "SixYears" },
                    { new Guid("242e1c55-932f-48f9-81c6-e5b1e0937c6f"), "4324eefb-3fb7-4945-ab12-0ca51e201f5d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8690), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "保安族", "保安族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8690), true, "保安族" },
                    { new Guid("2867b837-0852-4615-b0b2-2efff9faae72"), "5c217762-8729-4560-9349-93f083d8218a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8700), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "布依族", "布依族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8701), true, "布依族" },
                    { new Guid("2bb1b203-7d1d-4ccc-84e0-2945d2aabe39"), "2266afe0-a4c1-4001-ba94-b79d4165f3ea", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8809), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "景颇族", "景颇族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8810), true, "景颇族" },
                    { new Guid("2ca02c0e-6269-423e-87ee-22e630ee6d54"), "f09c81c2-9c30-4167-bb55-1c3bb155c5e3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9189), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "微信小程序端", "WxApp", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9190), true, "WxApp" },
                    { new Guid("2ee2ff55-ee4b-41bc-b387-475a313e415f"), "d7be206a-8a2d-4467-ab7d-dbd53d607bb5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9468), new Guid("3db25ae6-6702-4e94-9636-de59e1fe347e"), null, "学前阶段(幼儿园)", "PreSchool", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9468), true, "PreSchool" },
                    { new Guid("2f086d33-990f-4079-a601-ebadf955d22c"), "b35a7106-b971-4814-b818-20e8477db463", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9340), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "县级", "County", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9340), true, "County" },
                    { new Guid("2ff309d1-363a-4f54-a1f0-a449cb9d5681"), "f9459319-b37c-49de-9a61-ccb706b6e454", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8586), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "设备类型凭据", "DeviceType", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8586), true, "DeviceType" },
                    { new Guid("35cce2d4-1efd-4948-936b-ee867df8197f"), "432c3fc2-1b6e-4361-b503-7e36243717ee", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9376), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "一年制", "OneYear", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9376), true, "OneYear" },
                    { new Guid("3b32b7da-4ea2-4ae1-b727-8beabca871a7"), "909d8f33-3b68-4b4c-bec7-accf95ee73bd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9225), new Guid("f42a15a1-ed32-4bd2-9746-07d58f1d4ad4"), null, "男性", "Male", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9225), true, "Male" },
                    { new Guid("3dd3f8bb-acd6-4130-9d21-c3c780c9c1b7"), "0c0bfeed-7f5c-4a94-8440-abcfa886247a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9402), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "七年制", "SevenYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9402), true, "SevenYears" },
                    { new Guid("40035ca1-e2c1-485c-a10c-24e75439629f"), "e8c4d1f7-3c60-4f91-a11d-024815415093", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9261), new Guid("eff36a62-65df-42ff-a28e-b63e93d11de9"), null, "令牌策略", "Token", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9261), true, "Token" },
                    { new Guid("417a9b63-7a32-48ed-b9be-04d2232ebc5b"), "e45d29d0-78d2-4451-813a-8a58b5dbfef2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8474), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "角色标识凭据", "Role", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8475), true, "Role" },
                    { new Guid("41db952e-3421-4eba-86ec-4c704bafb9ae"), "831996b3-291d-469d-99da-839e42cdcdcd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9582), new Guid("ff6e2096-327c-403b-b0b2-d6355470c1d7"), null, "管理机构", "Management", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9583), true, "Management" },
                    { new Guid("4303ea23-105d-456c-b879-d4de671225b6"), "d8d747fb-78d1-4bdd-8820-7855ae01fbab", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9000), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "维吾尔族", "维吾尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9001), true, "维吾尔族" },
                    { new Guid("45748469-62eb-453a-9230-8a31ec762262"), "35286c8e-fc90-45c2-b9a0-80dd8fe9b3e4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8607), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "性别凭据", "Gender", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8607), true, "Gender" },
                    { new Guid("4dbfbeb0-9145-4ce9-8cd5-d7e20279dd6f"), "976b15dd-d486-4788-a652-0b9a17ab3111", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8796), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "回族", "回族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8797), true, "回族" },
                    { new Guid("4e92a28d-860c-4808-841c-7dd82a6ae8d5"), "1f4fe514-5409-4577-be26-734d9376e173", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8927), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "怒族", "怒族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8928), true, "怒族" },
                    { new Guid("4ecb9c39-fa38-4fb5-a6e0-be495714e45f"), "5d029a1e-a4a9-41e7-a2fa-7c9629012e4e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9105), new Guid("9a14dc28-56c3-4714-a8dd-118acdcd3a26"), null, "外部字典", "Public", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9105), true, "Public" },
                    { new Guid("4ffad6d1-7841-4cc1-b69a-c35e8aae3e49"), "ac1f8173-9d6e-424d-a1ab-f494c9412bd6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8664), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "白族", "白族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8664), true, "白族" },
                    { new Guid("51456235-9679-418c-89bf-e4ab4514db91"), "33e5bc69-0f86-4f33-9cc4-92a2d34fc7e1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8836), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "珞巴族", "珞巴族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8836), true, "珞巴族" },
                    { new Guid("520ffe5c-5cef-49a6-aacf-09f6e56274bb"), "67311eba-f08e-4686-8451-1cd67f1e5989", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9149), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "Android端", "Android", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9149), true, "Android" },
                    { new Guid("523ff106-8d41-4244-b6c5-e04c1833a349"), "497b8b37-b982-42e9-be5c-aaed1adfa9b7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8496), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "操作名凭据", "ActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8496), true, "ActionName" },
                    { new Guid("54ac145a-2186-465a-823d-5122d1ac3f98"), "7ba9c028-cb40-4f69-b0be-074f02bb13a0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8487), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "路由凭据", "RoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8487), true, "RoutePath" },
                    { new Guid("560be502-a3fd-49e6-9934-8d7508810a12"), "448ca2f2-bca2-432f-b4a6-66dfb51827d6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8842), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "僳僳族", "僳僳族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8842), true, "僳僳族" },
                    { new Guid("58f85519-2334-406e-879c-111648a6a5a6"), "6aab1ca5-fdf5-4255-8fba-78dae5a476fc", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9257), new Guid("eff36a62-65df-42ff-a28e-b63e93d11de9"), null, "匿名策略", "Anonymous", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9257), true, "Anonymous" },
                    { new Guid("5a62be52-5ee7-4267-8146-d3d14f964ccf"), "25425d2c-b0a6-42a3-8f6a-fc689fd9a00f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8723), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "东乡族", "东乡族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8723), true, "东乡族" },
                    { new Guid("5b2a5714-2bbb-4d07-98e7-8258b75e3f8f"), "3198024b-7981-42c1-9311-1f7fbe5fda37", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9131), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "签名初始化", "SignInitial", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9132), true, "SignInitial" },
                    { new Guid("5d216184-0da2-4261-b267-3c53e6ee9749"), "36a2b7fd-20c6-4f94-9802-cf259e750511", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8464), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "用户名凭据", "UserName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8465), true, "UserName" },
                    { new Guid("5daf8dc8-eca4-4052-ab83-54fae80b4e49"), "f3a58fca-fa72-42a1-b832-21b29e3205dd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9588), new Guid("ff6e2096-327c-403b-b0b2-d6355470c1d7"), null, "职能机构", "Functional", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9588), true, "Functional" },
                    { new Guid("5fedbde1-f5d1-4b5a-9771-f714aa44b6bc"), "02aa47e7-7c83-4282-af45-fced13e00cdb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8732), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "独龙族", "独龙族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8732), true, "独龙族" },
                    { new Guid("60047184-4619-4151-920f-c04d81c00167"), "6747e251-0d87-4b19-babe-0ece7780eb0b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9096), new Guid("9a14dc28-56c3-4714-a8dd-118acdcd3a26"), null, "未知类型", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9096), true, "Unknown" },
                    { new Guid("6308b4b5-5566-436d-9a95-a6ad4cee2cad"), "8aa7bb39-df11-4818-9a5f-76de4a63213b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9043), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "彝族", "彝族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9043), true, "彝族" },
                    { new Guid("634778fa-c5d3-4bda-905b-b7f673aadf00"), "f9e1206f-7eb4-42dc-9b78-ef7ba71584cb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8612), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "生日凭据", "Birthday", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8612), true, "Birthday" },
                    { new Guid("63c830b6-bfa0-476a-a937-bb1fa3fe74ea"), "9f55b09e-1902-4066-9d29-d2d6d067259c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8938), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "普米族", "普米族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8938), true, "普米族" },
                    { new Guid("653d6d8c-dce7-4fcd-9683-576c37e10034"), "bdcef5d6-33d8-4eca-b368-e5a0a0456965", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8949), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "羌族", "羌族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8950), true, "羌族" },
                    { new Guid("67bd6718-59a8-4e3c-9358-b93e49d71a55"), "0cb3a637-216b-45f5-9be2-aaed2c4a2263", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8490), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "元路由凭据", "MateRoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8491), true, "MateRoutePath" },
                    { new Guid("6a5aee52-ea23-4c31-869b-89c14e00eb2b"), "dc7e2d13-3f97-49c8-8be8-44ab0c688266", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8744), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "鄂温克族", "鄂温克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8745), true, "鄂温克族" },
                    { new Guid("6c6d08d7-e16a-41b1-8828-a27904b4e614"), "286e3e74-198e-4972-9679-cd5b83f7a7b5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9633), new Guid("ac1d3f30-f182-4d95-ae41-3c207a82e70f"), null, "运营中", "InOperation", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9633), true, "InOperation" },
                    { new Guid("6d4b8e86-9289-43ec-aef5-e28fe51e5dd6"), "c8f42967-46f6-4f1c-8f7f-c7837206783e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9344), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "乡级", "Township", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9344), true, "Township" },
                    { new Guid("6d717c04-15e2-49d1-b518-b942b8f992d6"), "afb8dc87-9d4e-43d9-87fc-075ec01ddcd3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9638), new Guid("ac1d3f30-f182-4d95-ae41-3c207a82e70f"), null, "停止运营", "CeaseOperation", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9639), true, "CeaseOperation" },
                    { new Guid("6dfb2366-da9a-4ee7-b95c-b03d0d558134"), "80a242f4-1c2b-4f0e-8822-7bbaf2617c9c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8760), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "哈尼族", "哈尼族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8761), true, "哈尼族" },
                    { new Guid("6f39d4da-9ff9-4a02-9754-9e0b44b86252"), "7c0c60a8-69af-4011-8f1b-b0be4a5ce3ce", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9048), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "裕固族", "裕固族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9048), true, "裕固族" },
                    { new Guid("6f9543d4-e699-4c46-899b-edfcf495f9f3"), "5de19979-5ac2-4c18-a982-0b6c27a5699c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9229), new Guid("f42a15a1-ed32-4bd2-9746-07d58f1d4ad4"), null, "女性", "Female", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9229), true, "Female" },
                    { new Guid("6fafa8f8-09fd-4a45-a23e-332af3d5379a"), "c328c016-ddfd-4da3-ad89-73fcc08d4c8b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9381), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "二年制", "TwoYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9382), true, "TwoYears" },
                    { new Guid("6fb0ffd4-cb22-4003-8443-23f9750e131d"), "d115aaf7-fb4c-4de7-86ee-95cfecda7515", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9100), new Guid("9a14dc28-56c3-4714-a8dd-118acdcd3a26"), null, "内部字典", "Internal", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9100), true, "Internal" },
                    { new Guid("70c26a70-1d7d-4db0-8389-a6dea0d3f97c"), "d4ef644c-d820-4c47-b4a9-ed2820694503", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8619), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "截止日期凭据", "Expiration", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8620), true, "Expiration" },
                    { new Guid("7100fc46-ab4a-4940-acb0-c035eb424da3"), "635b193d-4419-49f6-8f6f-54c3fc4bca32", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8654), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "未知", "未知", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8654), true, "未知" },
                    { new Guid("72100222-6db5-49f8-b4ed-f627581ad5f7"), "29572d02-62f1-4f94-9895-76fcc7ef9224", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8829), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "拉祜族", "拉祜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8829), true, "拉祜族" },
                    { new Guid("73340ad5-83a2-4493-9039-eda6903baf8e"), "2ac51cd0-c1a6-42ca-ba85-2d264f05bafa", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8616), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "过期时间凭据", "Expired", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8616), true, "Expired" },
                    { new Guid("755b84c3-4f37-4db4-824e-70fa7eed6f23"), "61c54d29-6e40-49e4-815d-637256a92f94", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8848), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "黎族", "黎族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8848), true, "黎族" },
                    { new Guid("79cb8631-b83a-44e3-b95f-77d853fad86f"), "e366215f-9f43-48a0-bcd5-3931128cbd5d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8704), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "朝鲜族", "朝鲜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8704), true, "朝鲜族" },
                    { new Guid("7b8f08fa-cc16-4e98-b5dd-78435387e229"), "fc07f4fb-30c8-4a29-839f-39cb01960c8b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8793), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "赫哲族", "赫哲族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8793), true, "赫哲族" },
                    { new Guid("7e24e508-2c17-40b6-8dff-01e45178724f"), "996e4143-44b8-484f-be59-b8cbb7cfe9ca", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9013), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "瑶族", "瑶族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9013), true, "瑶族" },
                    { new Guid("81e8cb78-6504-4137-83bc-0b5a17a999a9"), "07b7be4e-af78-4f68-a5ac-52c176e17fa0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8713), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "傣族", "傣族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8713), true, "傣族" },
                    { new Guid("84662471-8549-46bf-b97d-8ad9196d21df"), "252f9e0b-10d4-4884-b32d-24de70401e68", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9472), new Guid("3db25ae6-6702-4e94-9636-de59e1fe347e"), null, "小学阶段", "Primary", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9472), true, "Primary" },
                    { new Guid("8d4de0af-f88a-4768-a987-b9e2a59f5b84"), "0a6a1146-cc8e-40dc-8f55-026696360673", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8977), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "塔吉克族", "塔吉克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8977), true, "塔吉克族" },
                    { new Guid("8ec199b6-b3f9-483f-94cb-0cea40582521"), "51832b16-907a-4589-8096-aa53ba29f7b6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9053), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "藏族", "藏族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9053), true, "藏族" },
                    { new Guid("90b167af-7d0e-4a78-a022-45f715aea4f0"), "d2f04d41-5514-4ca6-b698-6a3b220a2d7a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8727), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "侗族", "侗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8727), true, "侗族" },
                    { new Guid("90dbbaa5-4578-45a8-95ba-404e58922d5d"), "880e6dda-67f1-41aa-b209-19a97af8c6ef", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8986), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "土家族", "土家族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8986), true, "土家族" },
                    { new Guid("9157b7c3-333a-4040-b648-b6066f3ce74c"), "b29e845f-9d36-45a8-83c0-cc075fe4d61e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9478), new Guid("3db25ae6-6702-4e94-9636-de59e1fe347e"), null, "初中阶段", "Junior", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9478), true, "Junior" },
                    { new Guid("936dca21-2964-447d-ae90-b2a0738693c0"), "e233dea0-231d-4141-9f00-d57b073c70cb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8804), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "基诺族", "基诺族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8804), true, "基诺族" },
                    { new Guid("94652647-306d-4245-81d7-dc370517b542"), "dc3bd35e-60c7-422e-b1fd-6765d4d4d0a6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8480), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "认证令牌凭据", "Authorization", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8480), true, "Authorization" },
                    { new Guid("9a564d24-568b-4aea-98f4-8a2a0bea80f8"), "5892cc01-e444-4b8e-bceb-de4e1a674139", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8905), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "纳西族", "纳西族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8905), true, "纳西族" },
                    { new Guid("9be937af-4486-40cc-ac13-4f79d489d9bf"), "9d47c5a4-f0cb-49bf-a33c-55817e6ab81a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9274), new Guid("eff36a62-65df-42ff-a28e-b63e93d11de9"), null, "路由路径策略", "RoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9274), true, "RoutePath" },
                    { new Guid("a26ecc41-8f00-4d6f-88aa-3c53a2a920f2"), "539b1833-0544-4f10-a304-fe12aaa40859", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8991), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "土族", "土族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8992), true, "土族" },
                    { new Guid("a29449f3-ab50-4c30-a2f5-7864316f84be"), "8c776b0e-9846-454e-9091-087a5c681b57", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8969), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "水族", "水族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8969), true, "水族" },
                    { new Guid("a5b4ec3c-7d94-43a6-977f-e800aeb9c9b2"), "6f869bc7-7106-4835-9ccc-3c886c2c8cfd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8510), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "Ip地址凭据", "IpAddress", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8511), true, "IpAddress" },
                    { new Guid("a70a6f70-6218-4977-9880-62537956ad76"), "49c37586-fe3b-45ad-8ced-8bb011d5ce2d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9410), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "九年制", "NineYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9411), true, "NineYears" },
                    { new Guid("abc35280-001a-46e9-b7b5-d58b5bd6d36b"), "52def2f5-1ecf-4ad1-9c00-9f8c26ab8363", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8592), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "邮箱凭据", "Email", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8592), true, "Email" },
                    { new Guid("aed4c885-3eae-4e9f-9bb0-010f2aeee321"), "b57a9679-87bb-4803-8870-d1e273c58b95", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8470), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "端类型凭据", "EndType", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8470), true, "EndType" },
                    { new Guid("aef7de54-7367-419e-b5bc-c4a68fd218bb"), "38586b7c-984b-412e-bb97-b44fffeb5a57", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8566), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "Dns凭据", "Dns", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8566), true, "Dns" },
                    { new Guid("af5d69fd-3caa-4c4c-8ee6-6c517b67df23"), "0a7347cf-a7ca-4f88-af0c-dba424459530", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9136), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "签入端", "SignUpEnd", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9137), true, "SignUpEnd" },
                    { new Guid("b38417aa-789e-4b33-8481-27d2d8c6bf24"), "993af045-4c8d-47fc-bb55-444244122bfa", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8603), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "地址凭据", "Address", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8603), true, "Address" },
                    { new Guid("b7029dc4-6562-4c1c-a89a-785ea35c21e6"), "c15075fb-05c8-4c67-853f-2bcea8718567", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8748), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "高山族", "高山族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8748), true, "高山族" },
                    { new Guid("ba30f3e3-ee3b-4b70-99cc-4a881308f014"), "0e7dbbf4-4917-4829-ab1e-c606902ae601", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8757), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "汉族", "汉族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8757), true, "汉族" },
                    { new Guid("bcc82d6e-a41d-4693-8f18-fb595c0b80ee"), "8e1aaf11-71aa-40a0-9d00-4082624e0784", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9307), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "国家级", "State", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9308), true, "State" },
                    { new Guid("bd40e52c-9ffb-4f0d-9dac-d8b2451b6fc2"), "1f8b08da-928c-4dbb-9fd7-4d99253a9980", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8718), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "德昂族", "德昂族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8718), true, "德昂族" },
                    { new Guid("bf6dbf42-9b8c-4e4f-a28e-fc82ae18518a"), "740f0900-c252-4d9e-bde8-41cc6de0f4c8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8891), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "苗族", "苗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8892), true, "苗族" },
                    { new Guid("c223426a-86a9-4440-8c87-4f9e6a0a5118"), "f0f4c09d-ba32-4feb-9ef9-f52bc260b36a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8982), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "塔塔尔族", "塔塔尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8982), true, "塔塔尔族" },
                    { new Guid("c340b837-3c03-4d41-85a7-7ddca9727300"), "d12eeba4-09a8-4f56-a23a-84e0e38c3eb2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8995), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "佤族", "佤族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8995), true, "佤族" },
                    { new Guid("c86e035e-1529-4dac-b13d-2571f0f249cf"), "140a2b60-e11e-4300-a5f9-da65f6c06790", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9482), new Guid("3db25ae6-6702-4e94-9636-de59e1fe347e"), null, "高中阶段", "Senior", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9482), true, "Senior" },
                    { new Guid("cce91ec6-caa9-44cc-aead-f19bef6b9318"), "758d712b-35bf-4826-8c7b-e66838bad8f5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9153), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "微信端", "WeChat", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9153), true, "WeChat" },
                    { new Guid("ce9fe143-0e1a-45a1-a0ca-c3ee00135ad4"), "beb4fd6b-bf1a-4231-a6ac-c220d2935823", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8599), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "移动电话凭据", "Phone", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8599), true, "Phone" },
                    { new Guid("cf2bc487-b150-41fd-99fa-23389b04727c"), "b2bc0fe7-ea07-4b9b-ae54-1dcb7b4092a0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8753), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "仡佬族", "仡佬族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8753), true, "仡佬族" },
                    { new Guid("d20277b8-4a6c-4a5c-adb6-cd1aaf9fbb46"), "75b47d01-bfde-4fdb-b680-b277b097fd1a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9407), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "八年制", "EightYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9407), true, "EightYears" },
                    { new Guid("d31aca02-2f64-4587-9f6b-2cc80c50bafe"), "d1a2e4e4-48c1-4b0b-9b9a-2127da7a7b64", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8572), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "Mac地址凭据", "MacAddress", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8572), true, "MacAddress" },
                    { new Guid("d6df4171-5424-4dbb-8aa1-9eb8b34f7203"), "750380da-5a41-46b0-a142-80b29ef34012", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9219), new Guid("f42a15a1-ed32-4bd2-9746-07d58f1d4ad4"), null, "未知性别", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9219), true, "Unknown" },
                    { new Guid("d9c1ff6b-b4c3-419d-b12e-75dd68086ff1"), "8093fc6e-e875-47c8-bd9b-1ffd123d418a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9266), new Guid("eff36a62-65df-42ff-a28e-b63e93d11de9"), null, "管理员策略", "Admin", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9266), true, "Admin" },
                    { new Guid("dcd08240-ffc3-4be1-b4c3-27a8f9cac151"), "8901bbc4-ea28-461e-b379-9088f1dd50ba", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9389), new Guid("78796012-72fb-449e-b9fb-27f529559eb9"), null, "四年制", "FourYears", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9390), true, "FourYears" },
                    { new Guid("dfb011c2-d0be-473f-a580-b370c53c09bc"), "384ab15d-03a9-4298-b8c4-6832c9f654a5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9335), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "市级", "Prefecture", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9335), true, "Prefecture" },
                    { new Guid("e3441b3a-570f-4c4a-84be-62f2cf32d509"), "6a6503a3-7f04-4b89-98f2-f89765408088", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9145), new Guid("d851278d-8447-4c28-ae10-b56fef91034c"), null, "IOS端", "IOS", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9145), true, "IOS" },
                    { new Guid("e3f4c3a4-ba25-4abe-9feb-c059ee261da2"), "20617d1d-aa28-4959-8509-48c7c71a38f8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8964), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "畲族", "畲族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8964), true, "畲族" },
                    { new Guid("e4b5e103-5d72-43c9-af02-6f3341c1c271"), "cbfee226-1894-43f7-b816-190c0fb5e00d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9004), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "乌孜别克族", "乌孜别克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9005), true, "乌孜别克族" },
                    { new Guid("eeb9bf1d-7c9b-4445-84a7-cdbbfa2448f4"), "58a07b77-2452-4a50-ac90-5a2b20dd7354", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8788), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "哈萨克族", "哈萨克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8788), true, "哈萨克族" },
                    { new Guid("eeeda6f7-40e5-46db-b903-6818249f95ad"), "589d208b-f25d-48ff-9c2e-a06b7d33e508", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8823), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "柯尔克孜族", "柯尔克孜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8824), true, "柯尔克孜族" },
                    { new Guid("f25a90a0-3006-4681-aa7b-a7333cd09855"), "7e467872-7285-4373-b686-6997dcd47847", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8957), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "撒拉族", "撒拉族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8957), true, "撒拉族" },
                    { new Guid("f52aed4d-f07c-4357-a869-e3306dc6aa38"), "9a53a2ac-0e8a-41d1-843a-c81ebd3f25aa", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8500), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "元操作名凭据", "MateActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8500), true, "MateActionName" },
                    { new Guid("f6241ee4-b521-4296-9c39-1b18135be686"), "e36d4396-1e2a-49ab-87ec-e96ab5afdfa9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8576), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "设备标识凭据", "DeviceId", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8577), true, "DeviceId" },
                    { new Guid("f6d595ef-0d80-44a8-a708-ee4e73010c04"), "c78f3672-c2b0-4b88-a7ae-806e36917c8f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8506), new Guid("040346dc-1201-453b-906a-637d666f40f1"), null, "签名凭据", "Signature", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8506), true, "Signature" },
                    { new Guid("fc223761-cbea-4c2a-9046-c51ac7357b17"), "a6f6902c-6d79-48b9-96b9-cc4b06efb894", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8867), new Guid("796d1660-3bf5-4d05-a43f-0971ff96c68b"), null, "毛南族", "毛南族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(8867), true, "毛南族" },
                    { new Guid("fd61fa6d-9159-449a-a3e5-db8bce333cdd"), "c4a191e7-5fed-4d97-8806-dbaeb5216dc2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9330), new Guid("42ceee56-8a93-4abf-bd24-dfe2e4f753ac"), null, "省级", "Province", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9330), true, "Province" }
                });

            migrationBuilder.InsertData(
                schema: "Resource",
                table: "ArtemisOrganization",
                columns: new[] { "Id", "Address", "Code", "ConcurrencyStamp", "ContactNumber", "CreateBy", "CreatedAt", "DeletedAt", "Description", "DesignCode", "DivisionCode", "Email", "EstablishTime", "ModifyBy", "Name", "ParentId", "PostCode", "RemoveBy", "Status", "Type", "UpdatedAt", "WebSite" },
                values: new object[,]
                {
                    { new Guid("a4e5a519-03f3-4874-bd7e-07a2442dd0cf"), null, "ORG5325001001", "ce910cc4-2e76-4d47-a8bf-70c53806dc02", null, "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9704), null, null, "ORG5325001001", "532503", null, null, "00000000-0000-0000-0000-000000000000", "蒙自市教体局", new Guid("116d61b4-bf6d-47c9-bce8-02c7a63081fa"), null, null, "InOperation", "Management", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9704), null },
                    { new Guid("afb15322-eeea-41e4-afdd-542ce20946b9"), null, "ORG5325001001001", "ccba720e-f2a8-487c-9799-efe49eb655af", null, "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9714), null, null, "ORG5325001001001", "53250301", null, null, "00000000-0000-0000-0000-000000000000", "西南联大蒙自小学", new Guid("a4e5a519-03f3-4874-bd7e-07a2442dd0cf"), null, null, "InOperation", "Management", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9715), null },
                    { new Guid("f39b4f1c-fe1e-4439-896c-98e67341202a"), null, "ORG5325001001002", "d2096992-674e-4f83-aaca-e6e135ea4910", null, "00000000-0000-0000-0000-000000000000", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9721), null, null, "ORG5325001001002", "532503101", null, null, "00000000-0000-0000-0000-000000000000", "蒙自市机关幼儿园", new Guid("a4e5a519-03f3-4874-bd7e-07a2442dd0cf"), null, null, "InOperation", "Management", new DateTime(2024, 8, 2, 18, 46, 45, 609, DateTimeKind.Local).AddTicks(9722), null }
                });

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
