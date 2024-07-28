using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Artemis.App.Resource.Migrations
{
    /// <inheritdoc />
    public partial class InitialResource : Migration
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
                    CountyCode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true, computedColumnSql: "substring(\"Code\", 1, 6)", stored: true, comment: "县级行政区划编码"),
                    PrefectureCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true, computedColumnSql: "substring(\"Code\", 1, 4)", stored: true, comment: "地级行政区划编码"),
                    ProvinceCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true, computedColumnSql: "substring(\"Code\", 1, 2)", stored: true, comment: "省级行政区划编码"),
                    TownshipCode = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true, computedColumnSql: "substring(\"Code\", 1, 9)", stored: true, comment: "乡级行政区划编码"),
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
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "机构编码"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "机构类型"),
                    EstablishTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "机构成立时间"),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "机构邮箱"),
                    WebSite = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "机构网站"),
                    ContactNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "机构联系电话"),
                    PostCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "机构邮编"),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "机构状态"),
                    DivisionCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "机构地址"),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "机构描述"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "父级机构标识")
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
                    { new Guid("27e279dd-f075-44b9-83d9-e8333b8f2a2e"), "DictionaryType", "182a807e-667a-4e69-b624-9bfbdaa35e01", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(444), null, "字典类型", "00000000-0000-0000-0000-000000000000", "DictionaryType", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(444), true },
                    { new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), "ClaimTypes", "f09bd2ea-5841-4920-a229-ee7637308cc7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9707), null, "凭据类型", "00000000-0000-0000-0000-000000000000", "ClaimTypes", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9718), true },
                    { new Guid("54d206dd-0989-47d4-ab39-7a543889e4c0"), "IdentityPolicy", "9aa12cd6-6d41-4d67-a5a8-8b9a00cb1a5f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(18), null, "认证策略", "00000000-0000-0000-0000-000000000000", "IdentityPolicy", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(18), true },
                    { new Guid("807e3219-e94e-48c1-816c-48b5e794fecf"), "TaskShip", "932af59c-ff41-4c77-be0e-59473ab5056a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(676), null, "任务归属", "00000000-0000-0000-0000-000000000000", "TaskShip", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(677), true },
                    { new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), "ChineseNation", "d78169b7-5c20-4589-bc07-5c795a5ffc7a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(66), null, "民族类型", "00000000-0000-0000-0000-000000000000", "ChineseNation", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(66), true },
                    { new Guid("aa0010a7-032c-4c35-9d11-970d09a0e8b6"), "TaskMode", "ef40cf5b-720c-41cd-80d7-f7252eb641ac", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(654), null, "任务模式", "00000000-0000-0000-0000-000000000000", "TaskMode", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(654), true },
                    { new Guid("c90ae04a-d987-48ba-b5b3-be14daad39c2"), "Gender", "4ed6f681-668d-481b-8c63-49396d1ac33a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(534), null, "性别类型", "00000000-0000-0000-0000-000000000000", "Gender", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(535), true },
                    { new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), "EndType", "78be663a-8c4d-47cb-88fe-e1fa90bbef24", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(481), null, "端类型", "00000000-0000-0000-0000-000000000000", "EndType", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(481), true },
                    { new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), "RegionLevel", "e62e2f93-1c2b-4cf4-ad35-e5f1a73f5300", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(602), null, "行政区划等级", "00000000-0000-0000-0000-000000000000", "RegionLevel", null, "Public", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(602), true }
                });

            migrationBuilder.InsertData(
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                columns: new[] { "Id", "ConcurrencyStamp", "CreateBy", "CreatedAt", "DataDictionaryId", "DeletedAt", "Description", "Key", "ModifyBy", "RemoveBy", "UpdatedAt", "Valid", "Value" },
                values: new object[,]
                {
                    { new Guid("020e046b-d5cb-4601-9a41-09c1b1d8b271"), "cc18a499-e361-4366-bc3b-d04b5fe65040", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(472), new Guid("27e279dd-f075-44b9-83d9-e8333b8f2a2e"), null, "内部字典", "Internal", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(472), true, "Internal" },
                    { new Guid("05222ec9-5805-4d87-af5e-476d953d6d91"), "0ee0b32a-17e8-4063-9d16-868c01a4d1cc", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(521), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "Android端", "Android", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(521), true, "Android" },
                    { new Guid("072c40c6-2d60-45d5-bcd7-31e19117c78a"), "e410a9c7-b442-42b4-a288-c81ee76e24cf", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(507), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "签入端", "SignUpEnd", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(508), true, "SignUpEnd" },
                    { new Guid("08105ed6-d73f-4fcc-8664-5e60ff151d74"), "d57dbb2f-1177-4077-af16-31b1565e8476", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(375), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "土族", "土族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(376), true, "土族" },
                    { new Guid("0984b1ac-2d95-4086-b72a-fe7c9774b91d"), "b00ac42f-d7db-48d8-96ba-d78c0e8f5255", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(285), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "黎族", "黎族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(285), true, "黎族" },
                    { new Guid("0b1f4f90-f8fb-4bd0-8795-11d0892a74ef"), "b8c3060f-adba-498d-b01c-49a3d8e34dc6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(513), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "Web端", "Web", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(513), true, "Web" },
                    { new Guid("0c1884bd-139b-42a9-9d42-f82ddc1b95da"), "c6f2f53c-b7b8-44f9-b61a-6de6702fd8cc", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(476), new Guid("27e279dd-f075-44b9-83d9-e8333b8f2a2e"), null, "外部字典", "Public", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(476), true, "Public" },
                    { new Guid("0f001002-b7b2-4419-9208-9735b7d2bb26"), "b46cdc0f-66e6-4876-87cf-f0066380fc48", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(97), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "白族", "白族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(97), true, "白族" },
                    { new Guid("16243662-f085-4de2-8032-01f7890d3774"), "76d1d0c1-8d14-448d-87dc-66b3d6d2b292", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(106), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "布朗族", "布朗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(106), true, "布朗族" },
                    { new Guid("16939956-a211-4c5e-b599-288d06953190"), "5e33dfb5-93ef-43c1-9010-c57258da3ce9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(196), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "高山族", "高山族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(197), true, "高山族" },
                    { new Guid("19815146-eb27-44d4-bc3e-8f8eebddfe8b"), "11ee081d-682f-4ff3-a368-1a27b254ca7d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(367), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "塔塔尔族", "塔塔尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(367), true, "塔塔尔族" },
                    { new Guid("1cba3312-ffb3-4a48-999a-6e5ffd0c2dfd"), "69977ea4-cb57-42c6-afc5-a339bf88a143", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9962), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "地址凭据", "Address", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9963), true, "Address" },
                    { new Guid("1d5b94ff-1f16-43dc-a4bc-6b4c2ecedb06"), "d7b661ec-b9a4-479b-b031-e11d86752723", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(152), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "朝鲜族", "朝鲜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(153), true, "朝鲜族" },
                    { new Guid("1e7b81da-858a-4e2c-a7e1-f6a985785b4f"), "113c011d-7cf2-4aee-ac94-d4ede50621f5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(388), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "乌孜别克族", "乌孜别克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(388), true, "乌孜别克族" },
                    { new Guid("203a5d92-3b7d-43d6-b619-e3f202722626"), "3d4a921e-e327-4837-b5f4-2b9319e8ab71", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(649), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "街道级", "Street", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(650), true, "Street" },
                    { new Guid("21e97763-d729-42b8-bb37-b42f56837d59"), "04c27762-9804-41ba-bbd3-dacab00b718e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(433), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "藏族", "藏族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(433), true, "藏族" },
                    { new Guid("2367aba4-e64d-4f14-979b-01bf25e6fe15"), "6a281ba3-1d1b-4507-8bcc-27032263f46e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9904), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "路由凭据", "RoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9905), true, "RoutePath" },
                    { new Guid("26790e61-cdd9-4167-bbbc-8f5c5d931aae"), "c855434b-90dd-44c2-8baf-d50ae4830867", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(161), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "傣族", "傣族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(162), true, "傣族" },
                    { new Guid("29b4a12c-d009-45c8-bc42-87ae4ce36c1a"), "a4fafdeb-9c11-4104-b188-2167d402105e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(200), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "仡佬族", "仡佬族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(200), true, "仡佬族" },
                    { new Guid("2b3c29ef-41c4-4853-be9d-194770afac25"), "6fdbd544-332e-47eb-8741-b2a37a6f10e8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9870), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "端类型凭据", "EndType", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9871), true, "EndType" },
                    { new Guid("2d999a3e-fc11-49c7-9d7f-bf8c8f866d49"), "1c777ae8-96f0-40a7-b629-dd22b1ecc61e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(400), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "彝族", "彝族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(401), true, "彝族" },
                    { new Guid("2f18152d-2c71-4598-9246-3a9b10779239"), "f06f2525-dfe9-496c-adeb-ef2a384dc81f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(371), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "土家族", "土家族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(372), true, "土家族" },
                    { new Guid("32560027-b9bb-4c66-b4a8-b17f574a42cd"), "17900813-74ac-491d-b1b0-e7c69de4dcb1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(294), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "毛南族", "毛南族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(294), true, "毛南族" },
                    { new Guid("3486c696-2c79-4d91-874f-537e10b9a1e0"), "b35196ac-13e2-4b4d-b5a2-99a33e775bd3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(60), new Guid("54d206dd-0989-47d4-ab39-7a543889e4c0"), null, "路由路径策略", "RoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(61), true, "RoutePath" },
                    { new Guid("34aa47b4-49ee-417a-b1fc-b740762fee18"), "bca21714-2e74-48d9-b7b7-02c9432db18f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9922), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "签名凭据", "Signature", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9923), true, "Signature" },
                    { new Guid("34c4a99b-4642-4340-bd22-5530d9a41b01"), "123fa0a5-63cd-4d98-b64e-768e37710336", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(51), new Guid("54d206dd-0989-47d4-ab39-7a543889e4c0"), null, "管理员策略", "Admin", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(52), true, "Admin" },
                    { new Guid("366a13cd-2b3f-4ff0-930a-96a6d3beab8c"), "e996a203-5884-477b-9d17-3fbc18c6943a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(179), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "独龙族", "独龙族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(179), true, "独龙族" },
                    { new Guid("37fc747e-5bfd-4c40-b2c1-d22d24724836"), "8e400155-497d-4df9-8c98-b9edabf678b5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(9), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "截止日期凭据", "Expiration", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(10), true, "Expiration" },
                    { new Guid("398cd6bc-ba9f-4fa2-a57f-ebb2001dcf21"), "f6f91816-1659-40b9-beef-d5535c9990c9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9958), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "移动电话凭据", "Phone", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9959), true, "Phone" },
                    { new Guid("3c7c1f97-aef8-42a2-86f4-462736d9c051"), "f90a47ca-aad1-45c5-b79f-0620ef99c35a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(363), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "塔吉克族", "塔吉克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(363), true, "塔吉克族" },
                    { new Guid("3d593898-f274-4708-a5d0-70ea626d7ae8"), "e72d86f0-63b5-4896-af0d-fc378739fd3e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(310), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "仫佬族", "仫佬族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(310), true, "仫佬族" },
                    { new Guid("3e145938-fbbe-401a-9054-8bbd800cab1c"), "4de21a8b-5f7b-4a7c-a518-007d390e325c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(289), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "满族", "满族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(289), true, "满族" },
                    { new Guid("4079b3ae-d202-46eb-b9b5-dd7623ab7762"), "760a3cd1-7e1b-4f0b-a0dc-f753ab79fa67", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(209), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "哈尼族", "哈尼族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(209), true, "哈尼族" },
                    { new Guid("43a526e2-84e7-4019-9a3e-dea80a932694"), "2878fe81-9749-4f0b-a55b-c898b80fcc24", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(204), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "汉族", "汉族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(204), true, "汉族" },
                    { new Guid("45cdb1ff-c5d4-4f5e-9b69-3b274bb3d875"), "0fbfdeec-05a1-4bda-a2c0-6cc7119098f5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(187), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "俄罗斯族", "俄罗斯族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(188), true, "俄罗斯族" },
                    { new Guid("4a627cc2-a145-4179-969e-43aa465a1e77"), "791cfa7e-d10e-4ac0-9d84-be16ab091c68", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(165), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "德昂族", "德昂族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(166), true, "德昂族" },
                    { new Guid("4b412243-473c-495d-8d8e-b30c9ea456c7"), "37f12987-2f64-4380-a0c5-fde28f6cc15d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9926), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "Ip地址凭据", "IpAddress", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9926), true, "IpAddress" },
                    { new Guid("4ee9819e-ec04-4640-8a12-f7389008c238"), "a920f22a-6d36-421b-835e-36b86abbb1f7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(93), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "阿昌族", "阿昌族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(93), true, "阿昌族" },
                    { new Guid("5041b726-4363-4600-be78-b1422bb69911"), "23441229-48ce-435a-a51f-de56e8839442", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(47), new Guid("54d206dd-0989-47d4-ab39-7a543889e4c0"), null, "令牌策略", "Token", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(48), true, "Token" },
                    { new Guid("509ef7ca-60f3-4ded-b3b5-60f09a2eb12e"), "c96e5b02-3fb7-4dfe-83ed-02ba5ce697d0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(338), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "怒族", "怒族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(338), true, "怒族" },
                    { new Guid("57c9930c-a0bd-4611-8e71-2b2ab1899c96"), "0af79bb5-3c3a-42f6-a807-e00ede8266b4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(380), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "佤族", "佤族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(381), true, "佤族" },
                    { new Guid("585883b7-a0a4-4557-af73-2f34ea23fb22"), "1fe4e62d-70b0-43f3-a20f-97ec50b3e772", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(526), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "微信端", "WeChat", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(526), true, "WeChat" },
                    { new Guid("58712146-e48c-4520-9074-9824057d0095"), "41d6f763-ba4d-4edc-85e8-945d3f4f4f86", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(384), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "维吾尔族", "维吾尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(385), true, "维吾尔族" },
                    { new Guid("5c371f0f-587c-4d62-ba2c-f0d09494c518"), "4625e706-459e-465c-ac07-6953988d5490", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9843), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "用户标识凭据", "UserId", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9843), true, "UserId" },
                    { new Guid("5c9ddb95-f901-450a-b491-74aaef724732"), "33718b10-6ccc-47d9-9137-37b37ff1611a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "生日凭据", "Birthday", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(1), true, "Birthday" },
                    { new Guid("5d9cd383-7bde-48a6-91d9-7bde88ae9b57"), "76ffcb2c-6989-4e6f-b055-9cbb882c5ee9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(597), new Guid("c90ae04a-d987-48ba-b5b3-be14daad39c2"), null, "女性", "Female", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(597), true, "Female" },
                    { new Guid("5e4100c8-d842-4c31-b477-27cec0a7cc1f"), "738b1a31-1b70-46a2-b12f-0e359af4ec18", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(88), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "未知", "未知", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(89), true, "未知" },
                    { new Guid("5e477a89-a070-4a6f-bc70-ed188c4d7d1f"), "8cf8b990-464d-486e-b86c-fbf8a43097bf", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(332), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "纳西族", "纳西族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(332), true, "纳西族" },
                    { new Guid("5f0f1d9d-c436-43c2-bbd1-7f41c158dde8"), "d4318eca-3eb1-47a0-8772-24178f62a214", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(587), new Guid("c90ae04a-d987-48ba-b5b3-be14daad39c2"), null, "未知性别", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(587), true, "Unknown" },
                    { new Guid("62dab184-6beb-46de-b25d-02b9de132efb"), "eb27f303-c8e9-4c44-b56b-0fc13fd4cff6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(147), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "布依族", "布依族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(147), true, "布依族" },
                    { new Guid("63b4e827-7085-46c1-a482-f976abe58cee"), "bc816445-7c8b-4403-9f01-369b019b5c48", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(428), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "裕固族", "裕固族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(428), true, "裕固族" },
                    { new Guid("69892a01-c902-4c98-8f39-ea3e3cc4aaf8"), "0bda2fc1-3aa6-4b8e-94ce-d423771169ce", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(276), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "珞巴族", "珞巴族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(276), true, "珞巴族" },
                    { new Guid("6c645ec2-ba2f-4134-9d53-522ec6fc5cc8"), "b2c31dd5-8b56-4db9-ad28-30825d8f09d7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(622), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "未知等级", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(623), true, "Unknown" },
                    { new Guid("6c793c15-3f56-41fb-8d38-06c633d48807"), "ea6292f8-4b5f-4cc0-8f5b-938adbfcc791", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(397), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "瑶族", "瑶族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(397), true, "瑶族" },
                    { new Guid("6fe897aa-8c33-4acc-872c-373c9f640c96"), "fb5a59ad-43ba-47f1-bf87-5f8adbde9f53", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(517), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "IOS端", "IOS", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(518), true, "IOS" },
                    { new Guid("7367351b-cf7f-44df-85c7-7efd82675fc1"), "7c98028f-c13f-46ba-884e-8055f8aabd37", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(174), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "侗族", "侗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(175), true, "侗族" },
                    { new Guid("73822339-d0d5-4986-8c18-5e0a3b7410c4"), "4a09fde2-4023-4c56-8635-bd64e1f13ae1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(591), new Guid("c90ae04a-d987-48ba-b5b3-be14daad39c2"), null, "男性", "Male", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(592), true, "Male" },
                    { new Guid("751ba4c6-10f5-489c-96c8-3407b4143267"), "fa775080-8f78-493f-a2ad-81c8be336c89", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(438), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "壮族", "壮族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(439), true, "壮族" },
                    { new Guid("78f11bb8-1cc7-4d90-a52f-36e5f930f33c"), "f7a59026-3893-47fe-9ca2-4adc8bc34d8f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(251), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "回族", "回族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(251), true, "回族" },
                    { new Guid("7946a7e0-578b-42d0-865c-e331474b4021"), "ab623cc9-5162-4380-85a2-c02226ab0731", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(301), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "蒙古族", "蒙古族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(302), true, "蒙古族" },
                    { new Guid("80842f83-7140-4b8a-9179-bc72ac363d8d"), "090987eb-ee12-4fff-bf3d-70777e19b92e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(530), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "微信小程序端", "WxApp", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(530), true, "WxApp" },
                    { new Guid("81643f37-347f-46ae-9172-e40da421d32e"), "21e22a2d-8b6d-49bc-96a5-58fd9999552d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9935), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "Mac地址凭据", "MacAddress", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9936), true, "MacAddress" },
                    { new Guid("86f6073f-638e-4418-8125-c1ef8da8a7c4"), "73e6965d-d969-449f-ab31-64289a857fa1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(627), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "国家级", "State", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(628), true, "State" },
                    { new Guid("878bc14c-8cff-41a6-88f6-87360c754e63"), "d4766885-912f-48d9-85ee-aa21e03e3399", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(280), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "僳僳族", "僳僳族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(280), true, "僳僳族" },
                    { new Guid("88a68fe7-022a-4e2f-85be-19f792227f50"), "85b331d5-99af-499b-9643-c99d3abb524d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(640), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "县级", "County", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(641), true, "County" },
                    { new Guid("89ff7b0c-0e87-490f-9d0e-8e6a8fef75b2"), "fbb2f07e-52e9-4628-ae98-86f8d5871243", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(259), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "景颇族", "景颇族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(260), true, "景颇族" },
                    { new Guid("8c1f5501-6ff4-473a-bbb6-07af9610ebd7"), "f10288c7-0655-443e-8b7c-8d1dde4b2b22", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(354), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "畲族", "畲族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(355), true, "畲族" },
                    { new Guid("8d9a4478-1dcc-439d-9580-f033e5e586fb"), "7dc1ccd2-c315-4b52-a575-ac3c41e0276e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(635), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "市级", "Prefecture", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(636), true, "Prefecture" },
                    { new Guid("8f62c004-2e83-4684-8766-d43ce90c4abb"), "a9d0ea2b-0dfd-4ac7-8520-4fb0097a3dc9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(156), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "达斡尔族", "达斡尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(157), true, "达斡尔族" },
                    { new Guid("902749d6-6c50-4a2b-b9e5-96e236374e36"), "d62431f7-4007-432a-95ab-c5321906b6a2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(191), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "鄂温克族", "鄂温克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(191), true, "鄂温克族" },
                    { new Guid("908ac5de-e0cd-4c56-b15f-1e5802f69433"), "21787037-6540-4bee-9db2-29caab60a687", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(169), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "东乡族", "东乡族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(170), true, "东乡族" },
                    { new Guid("9a5d3bbc-b769-40a7-97e5-6c0a99682d79"), "5977a450-ead9-43a6-a931-4f9fe9f3db47", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(4), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "过期时间凭据", "Expired", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(4), true, "Expired" },
                    { new Guid("9da2e73f-e8a9-4d5f-9045-87a450be95ba"), "81ab350f-1dcd-42d0-a154-8331258d36a6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(213), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "哈萨克族", "哈萨克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(214), true, "哈萨克族" },
                    { new Guid("9e86e6d4-fab5-49c7-889f-8c640b76b7fb"), "ec7d9e68-a6ff-4ea3-9b57-37bdbc1bbdea", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9899), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "认证令牌凭据", "Authorization", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9899), true, "Authorization" },
                    { new Guid("a0a13a9a-d5bd-40b6-b24e-a6fe3a985664"), "1bc56abe-ec63-4288-99d0-574ea3e7b96c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(644), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "乡级", "Township", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(645), true, "Township" },
                    { new Guid("b81ecc08-5ab9-45ae-9c69-ba5380545818"), "d3fc0dc8-005d-49d3-805d-bc83afc0e44a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(263), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "京族", "京族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(264), true, "京族" },
                    { new Guid("bada0eb7-e170-4353-bd57-9e7c7c765725"), "bc2997a4-1ae7-4f59-a2be-9868099f62c4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(306), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "苗族", "苗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(306), true, "苗族" },
                    { new Guid("bb0419c9-bb39-48f4-964e-9a72e82df472"), "57998e93-8f06-46f1-942f-3ff0011362b7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9866), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "用户名凭据", "UserName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9866), true, "UserName" },
                    { new Guid("bd9cb8a4-1d0a-42d1-b2ef-cede11e7bbe8"), "ddc4a4ba-8206-4c6a-95ce-3b03923a6fe2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(245), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "赫哲族", "赫哲族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(246), true, "赫哲族" },
                    { new Guid("c0193d04-7efc-41f4-a116-e2ab0ac40a48"), "1da0a6fc-a4da-43f2-86be-7134c8b0db40", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9994), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "性别凭据", "Gender", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9994), true, "Gender" },
                    { new Guid("c622338a-9efe-4407-b0ee-fc22e4324958"), "b0b8fe38-790a-4ae9-8478-e02a712c6707", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9939), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "设备标识凭据", "DeviceId", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9940), true, "DeviceId" },
                    { new Guid("d2291475-293c-4534-b5e1-e421ff6a2493"), "90241c48-bc62-4d2e-b68e-874358e88d74", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(466), new Guid("27e279dd-f075-44b9-83d9-e8333b8f2a2e"), null, "未知类型", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(467), true, "Unknown" },
                    { new Guid("d47d94ae-9707-477e-9311-4aef4f1ff5e4"), "d1214176-4cdc-4dbf-82fd-6ba7ec65a287", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9930), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "Dns凭据", "Dns", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9931), true, "Dns" },
                    { new Guid("d4b7c23d-e10a-45f7-8603-fc1bf791bca0"), "7715d056-acc1-414f-82b4-5d5edae4f8ec", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(350), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "撒拉族", "撒拉族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(351), true, "撒拉族" },
                    { new Guid("d79d15ac-09a4-462e-a78e-d79cac238135"), "c72bc62c-25d0-4d11-8a4d-47b7d8f38e82", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(504), new Guid("e0102897-437c-4ea0-bc1c-8af64c47d06c"), null, "签名初始化", "SignInitial", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(504), true, "SignInitial" },
                    { new Guid("db2d6263-abfe-46d2-b111-6b10be4bee41"), "07ab0a4c-5209-4ccd-bf29-7391cf7aeeec", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(102), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "保安族", "保安族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(102), true, "保安族" },
                    { new Guid("dbb10719-ea72-43d2-838b-ee4e4f7febea"), "cfd781b3-a554-4753-af90-d6e366cefc39", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(347), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "羌族", "羌族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(347), true, "羌族" },
                    { new Guid("dcb31b1a-e27b-43b8-8c25-6a66d8119723"), "ee33530c-9c4c-4c42-976c-685e9aa9facb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(393), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "锡伯族", "锡伯族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(393), true, "锡伯族" },
                    { new Guid("dd3407fa-f487-4aba-918e-e5f40f3c50d0"), "eeb47c1b-bcdd-48e8-84e7-a17db7d3b47e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(42), new Guid("54d206dd-0989-47d4-ab39-7a543889e4c0"), null, "匿名策略", "Anonymous", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(42), true, "Anonymous" },
                    { new Guid("e1b12e5f-9d04-442c-9775-a2973a7bbfe7"), "0df66322-b64e-4b27-b06a-a04d165703b9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(255), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "基诺族", "基诺族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(255), true, "基诺族" },
                    { new Guid("e23af56d-dcc9-47bc-a141-b4022c68b615"), "f22cf987-4091-44ff-8767-ac0dcb07fff1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9952), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "邮箱凭据", "Email", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9953), true, "Email" },
                    { new Guid("e8242f29-eb38-4b85-a358-511537803f27"), "5d222721-ca9c-4cc7-8808-fe6bf634491a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9913), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "操作名凭据", "ActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9914), true, "ActionName" },
                    { new Guid("e8f5a1b9-8e8e-4243-85e9-e5a38bcb24a1"), "004bf220-a2b2-449c-9d25-e5eb4d288763", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(182), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "鄂伦春族", "鄂伦春族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(183), true, "鄂伦春族" },
                    { new Guid("ea76b472-1fdd-4a03-9f84-854582f56499"), "b33e7c90-6eeb-40f2-9a05-d4ec4afc8cb6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(55), new Guid("54d206dd-0989-47d4-ab39-7a543889e4c0"), null, "操作名策略", "ActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(55), true, "ActionName" },
                    { new Guid("ea925fd3-f892-4ee9-8df2-6db1b3ef93ed"), "e935f77f-6c3b-402a-9a23-649c782cd206", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(342), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "普米族", "普米族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(342), true, "普米族" },
                    { new Guid("eab58dd7-9736-4fab-84f2-b7fa9ced6030"), "3c7f3573-5c5a-4b39-b6bf-9614a35d2b0a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9948), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "设备类型凭据", "DeviceType", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9949), true, "DeviceType" },
                    { new Guid("eaca7c10-d833-40b9-ab52-2e6ef5ba0f1f"), "fcd05f76-9cd1-4cbb-8e98-b80219bfd197", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(267), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "柯尔克孜族", "柯尔克孜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(268), true, "柯尔克孜族" },
                    { new Guid("eaf155db-cec5-4f36-95da-87e662394014"), "02f4b4b0-2776-4678-867b-220ddc198678", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9917), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "元操作名凭据", "MateActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9917), true, "MateActionName" },
                    { new Guid("edd0df11-e968-4d24-8628-b5768943753a"), "d99b4375-e8c2-4008-af62-b933a3cdf6e7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9892), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "角色标识凭据", "Role", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9892), true, "Role" },
                    { new Guid("eea0ac89-556e-4092-afa1-62877ed49089"), "fb8dda88-bdc8-4310-bd55-f32b32a4c0d6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(359), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "水族", "水族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(359), true, "水族" },
                    { new Guid("f56dc779-e86c-46e5-9e76-94cee6ab4add"), "c67d0378-8008-4c09-bcda-29450f10790f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(297), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "门巴族", "门巴族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(298), true, "门巴族" },
                    { new Guid("f760b91b-6b39-4d69-834e-809777cf0497"), "51dc2fea-96f2-4d3d-9fc5-e47c65e75955", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(272), new Guid("9f72fe9c-ca7e-4507-9205-c1d6f48ba355"), null, "拉祜族", "拉祜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(272), true, "拉祜族" },
                    { new Guid("f97eec4e-3d78-4c94-8325-4bda25a7909c"), "d1b17567-c905-407e-8518-24588a74260e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9944), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "设备名称凭据", "DeviceName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9945), true, "DeviceName" },
                    { new Guid("fd27b8fd-a284-4566-babe-6f7a3dd28085"), "33942ea6-c1d8-4a34-869a-963a0d059e54", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(631), new Guid("fcdf50c5-0bcc-4505-889c-e25392bafbcb"), null, "省级", "Province", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 382, DateTimeKind.Local).AddTicks(632), true, "Province" },
                    { new Guid("fdd96cdc-963e-4707-9b76-2329b5cd2061"), "11591181-74a0-4591-bc23-aa5c8d75f8de", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9909), new Guid("4e9155b2-5253-4565-b5f3-48ad3cd73fab"), null, "元路由凭据", "MateRoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 25, 16, 56, 57, 381, DateTimeKind.Local).AddTicks(9910), true, "MateRoutePath" }
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
                name: "IX_ArtemisDivision_CountyCode",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "CountyCode");

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
                name: "IX_ArtemisDivision_PrefectureCode",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "PrefectureCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_ProvinceCode",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_RemoveBy",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisDivision_TownshipCode",
                schema: "Resource",
                table: "ArtemisDivision",
                column: "TownshipCode");

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
