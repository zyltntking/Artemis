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
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "上级行政区划标识")
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
                    { new Guid("16f15e66-0725-4049-a64f-c0d2d50457fe"), "DictionaryType", "5e2b3a46-9ed3-4b42-ad14-32144ca172e4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5940), null, "字典类型", "00000000-0000-0000-0000-000000000000", "DictionaryType", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5941), true },
                    { new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), "EndType", "a29293fa-a5ee-4ac6-a89b-aee0b4a48020", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6018), null, "端类型", "00000000-0000-0000-0000-000000000000", "EndType", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6019), true },
                    { new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), "ClaimTypes", "42e588ee-b574-401c-a895-53bcf70a6590", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4775), null, "凭据类型", "00000000-0000-0000-0000-000000000000", "ClaimTypes", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4790), true },
                    { new Guid("36def1d1-c875-426c-af43-26fb47e64709"), "Gender", "0b2c49af-3f58-44f0-87d0-1dda26707c45", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6080), null, "性别类型", "00000000-0000-0000-0000-000000000000", "Gender", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6081), true },
                    { new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), "RegionLevel", "8dbafe57-4128-4e53-bbdb-984bf57506ff", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6119), null, "行政区划等级", "00000000-0000-0000-0000-000000000000", "RegionLevel", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6119), true },
                    { new Guid("8ea46004-df76-4ef5-abca-2c2a3d533756"), "IdentityPolicy", "77aa2226-1d82-4f22-ab72-56c25e349550", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5074), null, "认证策略", "00000000-0000-0000-0000-000000000000", "IdentityPolicy", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5075), true },
                    { new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), "ChineseNation", "48c04824-fb40-408d-b91d-579f500a07d1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5128), null, "民族类型", "00000000-0000-0000-0000-000000000000", "ChineseNation", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5128), true },
                    { new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), "ChineseNationEn", "df9c2fdd-ce94-43c3-9646-45dfa40b798c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5537), null, "民族类型(英文标识)", "00000000-0000-0000-0000-000000000000", "ChineseNationEn", null, "Public", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5537), false }
                });

            migrationBuilder.InsertData(
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                columns: new[] { "Id", "ConcurrencyStamp", "CreateBy", "CreatedAt", "DataDictionaryId", "DeletedAt", "Description", "Key", "ModifyBy", "RemoveBy", "UpdatedAt", "Valid", "Value" },
                values: new object[,]
                {
                    { new Guid("010ce571-90da-44f9-ba64-09a90c52c788"), "78796bd3-08d4-42ad-b313-94634ee67b1f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5243), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "俄罗斯族", "俄罗斯族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5243), true, "俄罗斯族" },
                    { new Guid("01adf7aa-e43a-49f7-a457-63c00f825516"), "dba5f95f-c9db-4877-a87c-d5f215184b64", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6069), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "微信端", "WeChat", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6070), true, "WeChat" },
                    { new Guid("025cba27-3c10-49f8-adc5-1bcafcd4a2a3"), "3fb716cd-e1ed-46f7-8eaa-c16312061452", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5796), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "羌族", "Qiangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5797), true, "Qiangzu" },
                    { new Guid("02ba04e8-fd2c-4ad4-bced-c670a5cca758"), "e997708b-dfa9-432a-b8cf-a743d79da601", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6144), new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), null, "未知等级", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6144), true, "Unknown" },
                    { new Guid("038af7ac-cc0e-4997-98be-3be5c9b7973e"), "93b7d5c4-a4d3-4eeb-84c3-20f6bb86271f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5335), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "僳僳族", "僳僳族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5336), true, "僳僳族" },
                    { new Guid("041be48d-7b18-4d28-a287-831eff499da9"), "2e2bb5b9-9701-49db-b7d9-5e774b20a284", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6065), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "Android端", "Android", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6065), true, "Android" },
                    { new Guid("048f697c-b41f-42af-ac8f-b48e375b5587"), "3699cebe-76d3-468b-a9c0-bc654f8745b2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5654), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "鄂温克族", "Ewenkezu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5654), true, "Ewenkezu" },
                    { new Guid("04a74174-a7bb-4e36-a91d-cf2f449f63a6"), "31f99da5-9886-44d6-b04b-185d81823a34", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5792), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "普米族", "Pumizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5792), true, "Pumizu" },
                    { new Guid("05675136-d673-460c-aafc-2b7575b308a1"), "a0d7ccbd-1c31-4c41-9a5c-08e3d0537d14", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5112), new Guid("8ea46004-df76-4ef5-abca-2c2a3d533756"), null, "管理员策略", "Admin", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5112), true, "Admin" },
                    { new Guid("06a62b0b-52b4-4235-9f43-22a3a5367766"), "31f7462b-fb1d-4510-add4-1b93cd30565c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4900), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "用户标识凭据", "UserId", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4901), true, "UserId" },
                    { new Guid("0796bf9e-cfd9-49d1-9898-fa3e0e107eeb"), "16bfbf06-5dcf-4e84-a4f7-27b30ed0537b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5648), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "俄罗斯族", "Eluosizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5648), true, "Eluosizu" },
                    { new Guid("08418d74-5cb4-480b-9c79-cb162d9804fa"), "3044572c-9bfb-4b6b-b44d-507eafd1a833", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5895), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "锡伯族", "Xibozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5895), true, "Xibozu" },
                    { new Guid("08c71749-0862-46c8-a261-badeaac3d78f"), "c18c1d21-95e4-428b-8d00-8090fc791472", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5683), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "赫哲族", "Hezhezu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5684), true, "Hezhezu" },
                    { new Guid("0ae9eb1a-3e36-4dd0-849f-2fb3de58103c"), "cea309e1-3728-4025-9f88-d774f4a81377", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4999), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "设备标识凭据", "DeviceId", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4999), true, "DeviceId" },
                    { new Guid("0c26bc23-c2d1-43b1-8e72-ff6f1d71f7dd"), "f4bd4e9e-e4d8-4450-848a-a981827bc971", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5203), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "朝鲜族", "朝鲜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5204), true, "朝鲜族" },
                    { new Guid("0c71d23f-f647-4e16-ba27-970e08149c3b"), "7c8cb47d-2048-4aff-9a11-995024680faf", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5663), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "仡佬族", "Gelaozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5663), true, "Gelaozu" },
                    { new Guid("0d05e4af-730e-47cd-baac-11d52fb8ee1c"), "9c125988-9d72-491d-9e63-2d70c6007e9c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5346), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "满族", "满族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5347), true, "满族" },
                    { new Guid("0d3bd4b7-1d15-46e1-9a35-f68e960d7aa1"), "3fa67c41-dcf7-49ad-8fd1-2c3551c99078", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5752), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "满族", "Manzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5752), true, "Manzu" },
                    { new Guid("138029e6-dae1-415c-81a0-ada0dcc27a43"), "5d9323f2-ce66-454b-b802-3088244acae2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5454), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "土族", "土族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5454), true, "土族" },
                    { new Guid("1a1e4e92-7eed-403e-99d4-0c5a76ff7260"), "c11b8d05-ef43-497f-9662-31de1af5be7e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5473), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "锡伯族", "锡伯族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5473), true, "锡伯族" },
                    { new Guid("1cbcaf55-1daf-46bf-8877-8c705b12362a"), "5f1e41d0-c385-4710-86f4-63b12f770cc7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5604), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "德昂族", "Deangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5604), true, "Deangzu" },
                    { new Guid("1f2a5dc9-500c-4144-836a-cfb7c6f3dc5d"), "d21b67d3-f3cd-4634-892e-8346bc7b8d8b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5429), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "畲族", "畲族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5429), true, "畲族" },
                    { new Guid("1fe3b62f-a482-4778-8981-bb1931ed582a"), "e7af263d-12dc-4034-aac2-d5ac7e5cfcea", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5003), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "设备名称凭据", "DeviceName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5003), true, "DeviceName" },
                    { new Guid("2176d574-1a1e-4914-9ad6-85c04ad4c183"), "c842adf6-2b4a-47b2-b9ca-ddaa4a5275d5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6175), new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), null, "省级", "Province", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6175), true, "Province" },
                    { new Guid("2265f42b-4ff0-4d6a-89c5-6ec590f21367"), "27f16aa0-4dba-4aac-84e1-af06a138b22d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4911), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "用户名凭据", "UserName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4912), true, "UserName" },
                    { new Guid("23c66508-e85b-46a3-bcf5-adb944335a2b"), "36280daa-9b5f-4cf9-8d64-37c6d80b096e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5687), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "回族", "Huizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5688), true, "Huizu" },
                    { new Guid("252d6910-8db3-4b90-a8fd-f808e0ef975c"), "6923e822-0c00-4489-b16b-4b6b134bb2b9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4973), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "元操作名凭据", "MateActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4973), true, "MateActionName" },
                    { new Guid("292bd809-5c1a-4b29-a5b8-0be34588c913"), "de07902c-2879-4a0b-9a0f-fe85e677f283", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5259), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "仡佬族", "仡佬族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5259), true, "仡佬族" },
                    { new Guid("2b3e36db-a542-425e-96e7-36eb565cc8ea"), "90c9d7f5-803f-42fc-8a5a-835c91083ec8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6059), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "IOS端", "IOS", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6059), true, "IOS" },
                    { new Guid("2bbb914b-7fcf-42c3-b2ec-b592e039c593"), "e4d22f92-69d6-4e3c-867d-e632ff3f3727", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5151), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "未知", "未知", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5151), true, "未知" },
                    { new Guid("2db68cd4-733c-4c76-b971-62c417bc2c2b"), "882b9164-e92c-4f52-abd5-c0e503f663bc", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4946), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "角色标识凭据", "Role", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4946), true, "Role" },
                    { new Guid("30450604-cc20-4c68-a488-9989029f1191"), "045a0caa-7464-4b5d-b4e5-9b9eacae3c01", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5063), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "截止日期凭据", "Expiration", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5063), true, "Expiration" },
                    { new Guid("30492967-62f2-4d77-9a88-44f2ac649f6d"), "1803ba4b-e046-4fdb-8496-d9b52c792d94", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5776), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "仫佬族", "Mulaozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5776), true, "Mulaozu" },
                    { new Guid("31b06be0-7a30-43d7-9cd8-fbd7c9458517"), "e29ab987-7584-4db0-9cdb-ae374d19c35c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5013), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "邮箱凭据", "Email", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5013), true, "Email" },
                    { new Guid("334934ed-031c-4a50-809d-9819b62a4b49"), "7d0b7d13-0729-42a1-905f-d53033f4173d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5643), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "鄂伦春族", "Elunchunzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5643), true, "Elunchunzu" },
                    { new Guid("335b9fca-4e6c-4221-b4bc-43ebc02eb8b4"), "b3addda1-6089-49f9-8258-1f91eedfcf77", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5757), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "毛南族", "Maonanzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5757), true, "Maonanzu" },
                    { new Guid("34d569c1-cb3b-4930-aaea-50f3b73a4912"), "8c991c62-941d-43ca-b420-d6b4767ecbab", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5525), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "藏族", "藏族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5525), true, "藏族" },
                    { new Guid("35f639a7-282c-42ab-bf47-d4be91d06cc0"), "8cdcd565-5128-411c-8e7d-6921e6d05b11", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4978), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "签名凭据", "Signature", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4978), true, "Signature" },
                    { new Guid("35feafcf-b6ca-4f61-bb32-2ec0c7c00859"), "bd228c3e-c854-422e-8885-4a2e31557659", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5575), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "保安族", "Baoanzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5575), true, "Baoanzu" },
                    { new Guid("361c591d-c234-4bb6-b0f6-2659160501f3"), "7218caa3-54e6-44ed-b073-d89c23e6f65d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5253), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "高山族", "高山族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5253), true, "高山族" },
                    { new Guid("37c9acf3-5406-403a-94d0-189bbf36de56"), "72e36a38-02d8-426d-8a82-8d1264df284c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5579), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "布朗族", "Bulangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5580), true, "Bulangzu" },
                    { new Guid("393dd64a-d37a-4a65-972e-209de331dde1"), "a7f8df7b-de90-46ad-83a8-4faea9f97e31", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5589), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "朝鲜族", "Chaoxianzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5590), true, "Chaoxianzu" },
                    { new Guid("3a05aea6-d4f8-4868-a893-ae2e2d808ae1"), "ac0f2606-1175-4f1c-8864-e83bf526712d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6114), new Guid("36def1d1-c875-426c-af43-26fb47e64709"), null, "女性", "Female", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6114), true, "Female" },
                    { new Guid("3fbafa9f-4e0f-4c0c-942b-90e208bc8c85"), "958d1586-d166-4c80-ad1f-2b09646681eb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5857), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "土家族", "Tujiazu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5857), true, "Tujiazu" },
                    { new Guid("43d74db5-07d3-4e5d-aa53-642d23ffc051"), "48b211e3-770e-422a-8873-ffd6a4b7eeb5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5424), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "撒拉族", "撒拉族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5424), true, "撒拉族" },
                    { new Guid("46beb292-9794-4e48-95fd-8b97ef7da4a4"), "5537c244-dd0b-4742-8dee-4354b265855e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5224), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "东乡族", "东乡族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5224), true, "东乡族" },
                    { new Guid("46cd41b6-b4a7-4683-988a-86b6dace6aa9"), "d077bffc-5dac-49c8-9ebe-88ef54f134b8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5357), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "门巴族", "门巴族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5357), true, "门巴族" },
                    { new Guid("476f2d6b-e913-426e-8024-32f7c128839d"), "fa8d3fa2-cc5e-4351-8a84-6a212c51ed16", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5816), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "塔吉克族", "Tajikezu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5817), true, "Tajikezu" },
                    { new Guid("47ade2c6-53b1-4b3c-b144-da0ae67ff217"), "719cde85-8343-4c63-9745-cfbc163bea18", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5618), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "独龙族", "Dulongzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5619), true, "Dulongzu" },
                    { new Guid("48c35e3e-4dd7-4325-8ab9-135afda15c97"), "2f1cb059-86da-4f24-b56f-d540d0ba3769", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5273), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "哈萨克族", "哈萨克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5274), true, "哈萨克族" },
                    { new Guid("51a0eeea-04cb-444d-abf9-dc9fcbbb9cca"), "95b928a2-52f4-422d-b2c9-a4978ab979c7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5981), new Guid("16f15e66-0725-4049-a64f-c0d2d50457fe"), null, "内部字典", "Internal", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5982), true, "Internal" },
                    { new Guid("527b8d87-3db5-4658-9fa7-97c26ef92b4b"), "09358ad6-6085-4dc9-a181-b010c86bc794", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5209), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "达斡尔族", "达斡尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5210), true, "达斡尔族" },
                    { new Guid("5316d811-676c-4648-b8f0-7a3b0bf5ee5c"), "b793dc92-6849-4d32-ab92-4f1a32bf7623", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5786), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "怒族", "Nuzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5786), true, "Nuzu" },
                    { new Guid("533a302c-0497-44c1-bbe6-c13ce1dcffcc"), "bbd69659-1a29-4dd0-ada7-b465119fa6e9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5872), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "佤族", "Wazu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5872), true, "Wazu" },
                    { new Guid("5870cb21-bd4a-4e6b-b480-5e1a0503a2b2"), "9098730b-3adf-4c40-a52f-34ce2d31934f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6185), new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), null, "县级", "County", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6186), true, "County" },
                    { new Guid("5a2320c7-403c-44e8-8ebc-2ca5beae0ae0"), "082a6ca2-9f3b-4ea1-b15f-b9687912ee24", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5023), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "地址凭据", "Address", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5024), true, "Address" },
                    { new Guid("5c1a11f8-a112-4836-a3f8-a21a2c1732ee"), "2a9a3f9d-d309-459f-b89c-bbd64dcd0638", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5722), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "僳僳族", "Lisuzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5723), true, "Lisuzu" },
                    { new Guid("5e65302e-7f99-4421-a1d5-9e98d0435b0a"), "61082d71-b79e-4131-ae3e-080b237ae800", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5570), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "白族", "Baizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5571), true, "Baizu" },
                    { new Guid("5ee8caf8-b39c-4d72-aff9-25e1a120f271"), "0ed7e182-8ae1-4aa7-a233-80ea8f3a1c1e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5117), new Guid("8ea46004-df76-4ef5-abca-2c2a3d533756"), null, "操作名策略", "ActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5117), true, "ActionName" },
                    { new Guid("62cc4576-3a42-43a9-bb66-6dd182e7af2b"), "703384bc-1139-482e-917f-19441aa990f0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5801), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "撒拉族", "Salazu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5801), true, "Salazu" },
                    { new Guid("636f9587-ea4f-4304-88d6-4d68698135e7"), "b769706c-936b-4d44-bfc7-a84841cd0e42", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5239), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "鄂伦春族", "鄂伦春族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5239), true, "鄂伦春族" },
                    { new Guid("640fd394-f1ac-48d6-bbf6-847ba112c192"), "54bee601-6ed3-4143-b94e-c41860890b5a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5610), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "东乡族", "Dongxiangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5610), true, "Dongxiangzu" },
                    { new Guid("65ad772b-044b-4647-b54d-889be478d445"), "429d5bee-edc1-40cb-8523-feb32730b6a1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5712), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "拉祜族", "Lahuzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5713), true, "Lahuzu" },
                    { new Guid("660ee2b6-1598-401d-a946-761555ff8665"), "5947b6fb-8dfe-44de-a00f-4d382a2f3c63", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5669), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "汉族", "Hanzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5669), true, "Hanzu" },
                    { new Guid("67647e1e-9e9c-4874-adeb-e0128c9971ad"), "d4372a14-6096-4dd4-9887-c1187a74afbc", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5708), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "柯尔克孜族", "Keerkezizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5708), true, "Keerkezizu" },
                    { new Guid("6bc60718-a5eb-423c-b25d-1b6e0748aaa8"), "a47fe112-5554-47e0-8ac0-2f5719dd6142", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5018), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "移动电话凭据", "Phone", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5018), true, "Phone" },
                    { new Guid("6be426d6-460c-47c4-99a6-f15412adbf26"), "39965ebd-3021-487e-821c-363cd6152efd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5172), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "布朗族", "布朗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5172), true, "布朗族" },
                    { new Guid("6c791c55-b378-4ed6-ad0b-3406fc50c707"), "38af6e9d-ac38-4bfc-88e6-4991742c7a19", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5249), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "鄂温克族", "鄂温克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5249), true, "鄂温克族" },
                    { new Guid("6e040a17-e437-4891-8310-705940c66d4d"), "a5a6edad-2a22-42b4-a7a3-4bdcbf5b912e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5008), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "设备类型凭据", "DeviceType", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5009), true, "DeviceType" },
                    { new Guid("6e38b552-48f8-45cc-8bde-c7adf7c95051"), "d8e2e514-309f-4d18-97d8-dc417a580dda", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5408), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "怒族", "怒族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5409), true, "怒族" },
                    { new Guid("6ed74a52-ed68-40cb-b6af-06586d950bae"), "5d50d609-1df5-4ffd-94da-f4c88052a05b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4984), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "Ip地址凭据", "IpAddress", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4984), true, "IpAddress" },
                    { new Guid("723ad20d-1472-4069-8522-0f9693d80092"), "9a3e9a90-40b2-4194-b58d-c78637bb723c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5659), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "高山族", "Gaoshanzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5659), true, "Gaoshanzu" },
                    { new Guid("72c08260-def6-4cbe-8547-0615af945ddd"), "643a8a44-b44c-4db8-a15c-eef239de0413", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5614), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "侗族", "Dongzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5614), true, "Dongzu" },
                    { new Guid("73c3a79d-2449-4d18-ae9e-96efa0514756"), "b1d6f29d-2b7b-464b-be9a-bd11e8424ee7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5047), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "性别凭据", "Gender", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5047), true, "Gender" },
                    { new Guid("765bf9d1-5d58-44ef-a30e-4f708187e5ae"), "0fc13788-8d23-4712-8b83-a7ff2b707a5f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4957), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "路由凭据", "RoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4958), true, "RoutePath" },
                    { new Guid("77a745df-f52b-4942-910e-fa5f5831cf5a"), "702e69d3-4e1f-4214-a604-4121e9965e77", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5600), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "傣族", "Daizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5600), true, "Daizu" },
                    { new Guid("7b101c8a-f9c7-44a1-8d4c-fd5c5fd4f6da"), "70616c3e-ec61-48ae-9290-ff0e688a79e2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5101), new Guid("8ea46004-df76-4ef5-abca-2c2a3d533756"), null, "匿名策略", "Anonymous", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5101), true, "Anonymous" },
                    { new Guid("7d93f737-3c56-4917-bcc5-29a30d66765c"), "cce5d480-40b7-4d1e-b917-4c98883d138f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5325), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "拉祜族", "拉祜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5325), true, "拉祜族" },
                    { new Guid("8049b793-97d6-4c90-8b5a-5ee0464595b9"), "6fe0a56a-41cc-4904-85eb-27d5ac3576b0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5320), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "柯尔克孜族", "柯尔克孜族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5321), true, "柯尔克孜族" },
                    { new Guid("81b31c87-527c-40f3-b4de-ce47278bf81a"), "8acfd8fa-af9a-4029-acbf-ea495cd97fcb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4968), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "操作名凭据", "ActionName", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4968), true, "ActionName" },
                    { new Guid("845ee62f-7b4b-40c9-90cf-4803a25dde62"), "9dd598b9-68c0-428f-9b72-f80d0845a5d6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5458), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "佤族", "佤族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5459), true, "佤族" },
                    { new Guid("8483f3ee-cfa1-47bb-a5df-2fef0d26b65b"), "25545798-3414-4b61-b8ae-e0459e43d976", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5488), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "裕固族", "裕固族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5488), true, "裕固族" },
                    { new Guid("85b29d03-58cf-4990-8267-0402025dd2cf"), "eb258bfa-2e1d-41e4-8be5-ad6d947023f1", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5585), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "布依族", "Buyizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5585), true, "Buyizu" },
                    { new Guid("88108a6a-968f-420e-81b9-a2ab70984c01"), "dd2bcdab-0408-45a7-a6d3-5d83c96f9a80", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5693), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "基诺族", "Jinuozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5694), true, "Jinuozu" },
                    { new Guid("884f1b47-d8fb-48a0-819e-cb45f29f914e"), "a9409e54-47d2-4898-8c62-83e4e38055e5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5677), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "哈萨克族", "Hasakezu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5678), true, "Hasakezu" },
                    { new Guid("89478d3d-475c-433d-a731-3314131f830b"), "3972b925-b311-44be-bdc9-80956dcd10d4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4950), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "认证令牌凭据", "Authorization", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4950), true, "Authorization" },
                    { new Guid("8c134407-237f-40a3-a0da-192395e338a9"), "184c6604-792c-43f1-897b-0db81e3ee100", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5439), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "塔吉克族", "塔吉克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5439), true, "塔吉克族" },
                    { new Guid("8d8274af-bb53-48fc-bf2a-3376cb12e7c7"), "83da70a1-fc65-4bc6-83b1-956fdd4230e8", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6073), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "微信小程序端", "WxApp", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6074), true, "WxApp" },
                    { new Guid("904b33d3-3d84-4717-922e-4ccf6c30bf5b"), "7dcbfd61-182a-4730-8d3d-c5b84cddc3e6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5305), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "基诺族", "基诺族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5306), true, "基诺族" },
                    { new Guid("91ef3db0-bd54-4808-b13f-f2daa02fc53e"), "2aa7bcaa-b112-486a-a07e-3b1ee1706ada", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5443), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "塔塔尔族", "塔塔尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5444), true, "塔塔尔族" },
                    { new Guid("9401b1ed-26ad-4d4c-90ac-29ecaa87853e"), "7036379b-d5e7-4f60-93d7-7e0a6eccc012", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6050), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "签入端", "SignUpEnd", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6050), true, "SignUpEnd" },
                    { new Guid("95270922-aa15-40ad-bba8-a3378bc68f92"), "b90fadf7-d40a-4f86-b266-c240b4924c89", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5464), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "维吾尔族", "维吾尔族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5464), true, "维吾尔族" },
                    { new Guid("95808677-3895-4a63-8815-b61563662c7c"), "e6e9ef88-c08a-4fc3-aa25-a0581af578a6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6191), new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), null, "乡级", "Township", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6191), true, "Township" },
                    { new Guid("96dbb1d3-d3ae-4928-b61e-7d162f1b29f3"), "21cf474e-16dd-4e57-9da6-818a1caf25ff", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5161), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "白族", "白族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5162), true, "白族" },
                    { new Guid("96e002e8-313c-49b8-a6dd-41858c2c4ec7"), "a8506b5c-5903-46d6-b6d9-0fdbe8287e54", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5361), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "蒙古族", "蒙古族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5361), true, "蒙古族" },
                    { new Guid("9827c10c-52cf-44fb-a36c-1ebe208833a4"), "a25a7ae4-7788-45e8-bb3e-e8d7eeea0c43", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6109), new Guid("36def1d1-c875-426c-af43-26fb47e64709"), null, "男性", "Male", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6109), true, "Male" },
                    { new Guid("9a76fecd-1e85-47ae-b1d6-1d4f8eee65de"), "c59d7a16-c2bb-4a99-b7e6-fad31b548f83", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5595), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "达斡尔族", "Dawoerzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5595), true, "Dawoerzu" },
                    { new Guid("9dde7f7a-1c91-413a-8aa2-d763f2494afa"), "c1b6886b-67bb-4d51-8a6e-364f980b5213", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5052), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "生日凭据", "Birthday", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5053), true, "Birthday" },
                    { new Guid("9ee0678d-6780-4a46-95b5-3228f59d7930"), "bf3c6837-b394-495a-9b76-8deb8b742d1b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5530), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "壮族", "壮族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5530), true, "壮族" },
                    { new Guid("a57049eb-acea-4fdb-9fdf-e05e7819c545"), "d15654d9-9705-4c39-a185-f08ffa9f328a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5902), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "瑶族", "Yaozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5902), true, "Yaozu" },
                    { new Guid("a61345b2-b99b-4eba-b4cf-9a175f6bb7b0"), "6a4270d2-f698-4a63-886c-5d16b73b0922", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5560), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "未知", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5560), true, "Unknown" },
                    { new Guid("a6b5a2b1-688e-42d8-9037-c21be201a535"), "aeb23da3-0d74-41ef-b2f6-3d990581ddfb", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5371), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "仫佬族", "仫佬族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5372), true, "仫佬族" },
                    { new Guid("a6faa750-c8be-43c6-bda9-e7541241bced"), "22ce6a0b-fa36-47f6-93a7-7b68939b348a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5433), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "水族", "水族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5434), true, "水族" },
                    { new Guid("a91b9d7e-28f8-4692-aee8-7398e8e4010e"), "6f07ba84-0e74-4a29-9cca-c6d63a4c73e2", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5887), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "乌孜别克族", "Wuzibiekezu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5887), true, "Wuzibiekezu" },
                    { new Guid("aac5e553-37f3-4a9a-9b95-3793b3038cca"), "91adf265-5e49-4af6-b915-3a9e0d7970d6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5157), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "阿昌族", "阿昌族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5157), true, "阿昌族" },
                    { new Guid("ab26169a-a0dc-42f0-8e4b-0b37e8bbe1b4"), "6f12483b-b077-46a6-8109-dfd050093dab", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5811), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "水族", "Shuizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5811), true, "Shuizu" },
                    { new Guid("ac9a8869-7de1-4d00-9a75-0e084c4d52fa"), "54d33de6-6d05-4f13-b9fc-be8df6140e9b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5199), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "布依族", "布依族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5199), true, "布依族" },
                    { new Guid("aecdd2b7-b036-40cc-b702-c36eb8afab18"), "08e9ce57-dbef-41b9-a3de-ccd7eed7b683", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5403), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "纳西族", "纳西族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5404), true, "纳西族" },
                    { new Guid("af8d9e29-cd6f-4040-a0f9-027efc24d818"), "8bab8f2f-fa42-4bb9-8f91-8b5afc0ef01a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6054), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "Web端", "Web", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6055), true, "Web" },
                    { new Guid("b0273e61-97af-459c-b45b-4f49ff5872dc"), "765226d4-d7de-49f2-abbe-9ad0bf784c55", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5316), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "京族", "京族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5316), true, "京族" },
                    { new Guid("b0fbe026-585f-496e-9413-662fcf8bbe86"), "2c457976-b1a1-4dc0-b6e1-219325f3d3df", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5278), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "赫哲族", "赫哲族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5278), true, "赫哲族" },
                    { new Guid("b26137d1-b5e3-4d71-b09f-324543fd9d64"), "1d1472a0-17f7-4f59-b089-06166f921772", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5866), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "土族", "Tuzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5866), true, "Tuzu" },
                    { new Guid("b2e50156-3f84-4eaa-bdeb-c6db04ef6113"), "de4f50bb-009f-44dd-8dea-22e5e5a48646", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6148), new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), null, "国家级", "State", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6149), true, "State" },
                    { new Guid("b34516be-3974-4fcd-a77f-ac8c54f969c5"), "6133af28-8204-4612-945a-37ec72bee1c3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5850), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "塔塔尔族", "Tataerzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5850), true, "Tataerzu" },
                    { new Guid("b59a1b7f-3293-48e2-9409-d04dd6c5e440"), "58a47711-36c0-46b4-a0d1-1183f15dc3f7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5263), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "汉族", "汉族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5264), true, "汉族" },
                    { new Guid("b957da81-d611-4ede-bde7-7b7e96b3bbae"), "6188872b-819a-4a6c-ae7d-c0e66fe75af4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6103), new Guid("36def1d1-c875-426c-af43-26fb47e64709"), null, "未知性别", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6103), true, "Unknown" },
                    { new Guid("b9ff8f71-58c4-4ccf-a046-4a8874e4a274"), "0ea570c3-52d3-47f2-bcb1-2ea6471ffcdf", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5926), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "藏族", "Zangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5926), true, "Zangzu" },
                    { new Guid("bc33b96c-e7e1-470e-9e77-61cca0022a51"), "a66dd6c6-526b-4762-95d4-bb013d02312b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5166), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "保安族", "保安族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5166), true, "保安族" },
                    { new Guid("be19d149-85b8-4b40-bfaf-3852c4d2e15c"), "dc4e714a-0ebe-4a0d-b459-aad9a7d0ccc7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5767), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "蒙古族", "Mengguzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5768), true, "Mengguzu" },
                    { new Guid("bf7d23cf-4e3e-489b-87c2-6e53f69e5f14"), "77c02818-c613-414a-964b-0498471e293d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5268), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "哈尼族", "哈尼族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5268), true, "哈尼族" },
                    { new Guid("c084deeb-fe7f-4198-ac1f-99bf2ec95a31"), "72c98c37-3447-4206-80c0-2a9352a38a31", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5716), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "珞巴族", "Luobazu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5717), true, "Luobazu" },
                    { new Guid("c129af25-8440-45db-85e1-ef9adc02dd2f"), "5d690f27-7bae-41eb-acd3-0eb01d1a1f0c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5932), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "壮族", "Zhuangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5933), true, "Zhuangzu" },
                    { new Guid("c134308c-ca8d-4817-a1c8-9269548e8a74"), "2f8492ff-acad-466a-9b11-7f0a126f30cd", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5339), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "黎族", "黎族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5340), true, "黎族" },
                    { new Guid("c1ebdb82-b4fa-4976-a324-502fc0b1160d"), "5247fea1-c0ef-4491-ae90-d76ff7b6e71b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5479), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "瑶族", "瑶族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5479), true, "瑶族" },
                    { new Guid("c26f13a8-815d-48a5-988c-b9e1c8603d5e"), "e85e1356-cc3c-4170-ba51-ddff0a2b372d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5910), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "彝族", "Yizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5911), true, "Yizu" },
                    { new Guid("c2e64383-0cb9-48b8-9761-609b1e204a1e"), "f5ff86a1-c390-4713-938f-5318770c6be6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4940), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "端类型凭据", "EndType", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4940), true, "EndType" },
                    { new Guid("c40cf68d-332c-433d-b16a-e55f587d9bac"), "7be593c9-ffe3-4198-8b08-c620cc298d14", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5414), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "普米族", "普米族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5414), true, "普米族" },
                    { new Guid("c547dbcc-89e5-42b0-9269-0074331ba2c2"), "cb666011-9ed4-4c67-9871-c770d4e93b1a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5058), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "过期时间凭据", "Expired", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5058), true, "Expired" },
                    { new Guid("c59b8514-eace-42bd-93c2-7a5c547c54cb"), "412317d2-0275-4cf6-b6f8-ac9de94e1219", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5469), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "乌孜别克族", "乌孜别克族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5469), true, "乌孜别克族" },
                    { new Guid("c83f3ae4-b49d-45dd-9452-e7ad420c0676"), "7d2c3b7e-fe61-439a-bcbc-40dca55e9fea", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5299), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "回族", "回族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5299), true, "回族" },
                    { new Guid("c84ce07b-62a6-4856-8843-c099cb631deb"), "00636c32-1465-4b2d-ba74-e32e14b071fe", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5365), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "苗族", "苗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5366), true, "苗族" },
                    { new Guid("ca385168-1d59-4af5-bf39-3da967c0a7bb"), "869e75cd-4c2f-401d-80cb-9f331cf7b223", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4989), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "Dns凭据", "Dns", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4989), true, "Dns" },
                    { new Guid("cc737ddb-8a92-41b4-a406-6750e334f0d1"), "f262a4f9-5bdc-4e44-89e3-b5d2f8b89a35", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6181), new Guid("39d7b6e1-4bd1-455f-bd27-6291d98d875f"), null, "市级", "Prefecture", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6181), true, "Prefecture" },
                    { new Guid("d0235aa8-8b8e-4967-8d96-01f41c3554f9"), "dd02d0be-e219-4ed8-88db-78d47712428d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5234), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "独龙族", "独龙族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5234), true, "独龙族" },
                    { new Guid("d0d94995-0e18-4f0a-b4cd-f08b7eb25eea"), "9327cdbe-1ae4-4a69-a56d-3494c07909d3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5879), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "维吾尔族", "Weiwuerzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5879), true, "Weiwuerzu" },
                    { new Guid("d132b8d1-0c37-4a75-9479-da6df7ae449c"), "a23a7d5c-7eed-4bcc-ac66-485fd8b718b4", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5484), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "彝族", "彝族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5484), true, "彝族" },
                    { new Guid("d424bca3-cd8d-4751-a552-13faf3a105e9"), "f35ca66b-dd74-4569-ac21-e6a5faf57761", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4962), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "元路由凭据", "MateRoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4962), true, "MateRoutePath" },
                    { new Guid("d8835c2d-6ef5-4934-8bda-1f05440b70f7"), "58aa24e4-245b-49fe-a151-7657e860ffa6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5228), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "侗族", "侗族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5228), true, "侗族" },
                    { new Guid("da07ad55-5f95-4ba4-a9e8-28e4f3be66fb"), "c30e9352-5a08-477c-abe0-e9b486f740d7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5418), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "羌族", "羌族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5419), true, "羌族" },
                    { new Guid("db56bc7e-0343-409c-8403-1fd43a19bec0"), "df86d0bf-ade3-4eb3-bd17-1f20db8fdd6e", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5772), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "苗族", "Miaozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5772), true, "Miaozu" },
                    { new Guid("dc54a530-31c5-42ce-a02d-b6d83676f964"), "9b370bb2-8257-4bb0-8819-b20fa3a93661", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4993), new Guid("33b126b4-ffb8-4c8c-a900-1671bc9903c0"), null, "Mac地址凭据", "MacAddress", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(4993), true, "MacAddress" },
                    { new Guid("dd9b8965-8bf8-4723-8828-237b44c44559"), "87842abc-5ff5-4e94-9525-cda951535b33", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5782), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "纳西族", "Naxizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5782), true, "Naxizu" },
                    { new Guid("de31ed04-1d51-44d5-a59a-686700b3225e"), "6584b561-04cd-4e7f-a390-f0dae6ec57f5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5806), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "畲族", "Shezu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5807), true, "Shezu" },
                    { new Guid("de5b23db-1516-495e-8bfc-c5ad5f3ae902"), "9065b53c-9437-44a9-803d-dff46925f2e5", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5761), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "门巴族", "Menbazu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5761), true, "Menbazu" },
                    { new Guid("ded4ec3b-69d5-4f59-8a38-d81983c2ecdc"), "699405c5-b08d-438d-a8b1-3724319ac6b6", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5213), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "傣族", "傣族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5214), true, "傣族" },
                    { new Guid("e53b66b5-d3ab-4d7b-9859-61ab4565c242"), "94f87f71-7443-4c47-ac20-11356b23a21d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5702), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "京族", "Jingzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5702), true, "Jingzu" },
                    { new Guid("e5ea9692-55fe-4042-bfe5-1efe64bf6659"), "d2ee4540-110f-4ec3-9e13-af8e28930666", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6044), new Guid("2c259847-ea07-463a-8c27-be0a5281e782"), null, "签名初始化", "SignInitial", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(6044), true, "SignInitial" },
                    { new Guid("e6282902-caa1-4fce-89ec-9372db1fb705"), "5ba49009-e704-45aa-a8a7-4c6a6e1c9d8d", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5351), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "毛南族", "毛南族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5351), true, "毛南族" },
                    { new Guid("e6bbbe65-1fa5-4146-9f2a-f188538a9f77"), "8dcb9b54-b176-4707-804e-a8e13a7f35e7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5121), new Guid("8ea46004-df76-4ef5-abca-2c2a3d533756"), null, "路由路径策略", "RoutePath", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5122), true, "RoutePath" },
                    { new Guid("e77290f1-0f4c-4d42-90b9-eaf33de581fb"), "f1534615-6ed4-4dba-ba1d-e97de2074536", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5448), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "土家族", "土家族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5448), true, "土家族" },
                    { new Guid("ef349ff2-e967-4c73-8d67-b55341f216be"), "49ecbffd-67c8-4252-9fb4-920b84ed24f9", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5727), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "黎族", "Lizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5727), true, "Lizu" },
                    { new Guid("f1e9f961-f122-4822-9a23-250ee942ea34"), "9f8efc76-627a-4541-8a54-e76ca87a2a82", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5698), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "景颇族", "Jingpozu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5698), true, "Jingpozu" },
                    { new Guid("f397acf8-c432-4367-8918-b3f7b3d9e160"), "160e1d44-cfd5-4090-bf55-a7363caafa17", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5219), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "德昂族", "德昂族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5219), true, "德昂族" },
                    { new Guid("f58991c0-5fb8-4131-8b34-f60c9cb0d288"), "7a3b27a0-04cc-48de-ac98-6463a02bb98b", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5976), new Guid("16f15e66-0725-4049-a64f-c0d2d50457fe"), null, "未知类型", "Unknown", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5977), true, "Unknown" },
                    { new Guid("f6f1f8ca-0283-4d42-b5b2-9deefe18a38e"), "9b4286e5-b0f1-47b0-af1e-194f2fa5e934", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5564), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "阿昌族", "Achangzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5564), true, "Achangzu" },
                    { new Guid("f91134da-0db2-4f70-a8b9-9d037eee39a7"), "456518c0-1ca2-40d8-863f-d5286c586abf", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5310), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "景颇族", "景颇族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5310), true, "景颇族" },
                    { new Guid("fa3902d3-63fa-4b27-8268-3878a06e2770"), "cfa2ba78-2e4a-42f1-a2ef-6bd68aa8776a", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5331), new Guid("e7ef82bc-7497-449a-9047-93ba3abd4ecc"), null, "珞巴族", "珞巴族", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5331), true, "珞巴族" },
                    { new Guid("fbd9ac96-17da-4cb0-ab4a-ba4766f71495"), "5b262f37-99c3-42e3-a8f4-fe53de0a6753", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5986), new Guid("16f15e66-0725-4049-a64f-c0d2d50457fe"), null, "外部字典", "Public", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5986), true, "Public" },
                    { new Guid("fc2edfca-95e6-4c65-b842-238158294210"), "1156e977-e4a7-4967-9b1e-cdf7c3b11de7", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5106), new Guid("8ea46004-df76-4ef5-abca-2c2a3d533756"), null, "令牌策略", "Token", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5106), true, "Token" },
                    { new Guid("fd884b91-e33e-49a3-b819-840b7b1fd7d5"), "d1898eb2-4cc9-4904-8b7f-bf366184362f", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5917), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "裕固族", "Yuguzu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5917), true, "Yuguzu" },
                    { new Guid("ff873b23-2cef-4805-a340-6c42c31e5178"), "4247b40e-5b2a-45d5-b89e-ed71e3b83bc3", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5673), new Guid("e9076eec-2fee-49d2-95ad-10d20243b6df"), null, "哈尼族", "Hanizu", "00000000-0000-0000-0000-000000000000", null, new DateTime(2024, 7, 15, 16, 36, 2, 350, DateTimeKind.Local).AddTicks(5674), true, "Hanizu" }
                });

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
                name: "IX_ArtemisDataDictionaryItem_DataDictionaryId",
                schema: "Resource",
                table: "ArtemisDataDictionaryItem",
                column: "DataDictionaryId");

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
