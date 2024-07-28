﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "IdentityClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "删除者标识"),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据值"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityClaim", x => x.Id);
                },
                comment: "认证凭据数据集");

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                schema: "Identity",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "角色名"),
                    NormalizedName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "标准化角色名"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "角色描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                },
                comment: "认证角色数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                schema: "Identity",
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
                    UserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "用户名"),
                    NormalizedUserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "标准化用户名"),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "电子邮件"),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "标准化电子邮件"),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "电话号码"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "电子邮件确认戳"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "电话号码确认戳"),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "密码哈希"),
                    SecurityStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "密码锁"),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false, comment: "是否启用双因子认证"),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "用户锁定到期时间标记"),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false, comment: "是否启用锁定"),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false, comment: "失败尝试次数")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                },
                comment: "认证用户数据集");

            migrationBuilder.CreateTable(
                name: "IdentityRoleClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "标识")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "角色标识"),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据值"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityRoleClaim_IdentityRole",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证角色凭据数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "标识")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据值"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户凭据数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "登录提供程序"),
                    ProviderKey = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "提供程序密钥"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    ProviderDisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "提供程序显示名称")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_IdentityUserLogin_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色登录数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserProfile",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "用户档案数据键"),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "用户档案数据值")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserProfile", x => new { x.UserId, x.Key });
                    table.ForeignKey(
                        name: "FK_IdentityUserProfile_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色档案数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserRole",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "角色标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_IdentityUserRole_IdentityRole",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityUserRole_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色映射数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserToken",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_IdentityUserToken_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户令牌数据集");

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "CreateBy", "CreatedAt", "DeletedAt", "Description", "ModifyBy", "Name", "NormalizedName", "RemoveBy", "UpdatedAt" },
                values: new object[] { new Guid("47610070-84d0-4638-a5d1-b40e19cf9d5a"), "d6b238c5-f0a3-4254-8928-d4fee3f68b4c", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 9, 7, 2, 384, DateTimeKind.Local).AddTicks(1443), null, "默认管理员", "00000000-0000-0000-0000-000000000000", "Admin", "Admin", null, new DateTime(2024, 7, 25, 9, 7, 2, 384, DateTimeKind.Local).AddTicks(1463) });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "IdentityUser",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreateBy", "CreatedAt", "DeletedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "ModifyBy", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RemoveBy", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("58510b95-1bf6-4d24-9b2b-e71bdf550a73"), 0, "e8121291-cd1e-4063-933b-d74e793246a0", "00000000-0000-0000-0000-000000000000", new DateTime(2024, 7, 25, 9, 7, 2, 384, DateTimeKind.Local).AddTicks(1682), null, null, false, false, null, "00000000-0000-0000-0000-000000000000", null, "admin", "AQAAAAQABAeCAAAAEAWMpbNE093jCa58ROOANUCRejUrt//+t+4aBJJ5OpDLYU3gcS5XhbA4WTtmt4p80YchOq2O", null, false, null, "AMEVUIDINGYGJRPIT4K6I43DOCMD7HUK", false, new DateTime(2024, 7, 25, 9, 7, 2, 384, DateTimeKind.Local).AddTicks(1682), "admin" });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "IdentityUserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("47610070-84d0-4638-a5d1-b40e19cf9d5a"), new Guid("58510b95-1bf6-4d24-9b2b-e71bdf550a73") });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_CheckStamp",
                schema: "Identity",
                table: "IdentityClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_ClaimType_ClaimValue",
                schema: "Identity",
                table: "IdentityClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_CreateBy",
                schema: "Identity",
                table: "IdentityClaim",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_CreatedAt",
                schema: "Identity",
                table: "IdentityClaim",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_DeletedAt",
                schema: "Identity",
                table: "IdentityClaim",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_ModifyBy",
                schema: "Identity",
                table: "IdentityClaim",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_RemoveBy",
                schema: "Identity",
                table: "IdentityClaim",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_UpdatedAt",
                schema: "Identity",
                table: "IdentityClaim",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_CreateBy",
                schema: "Identity",
                table: "IdentityRole",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_CreatedAt",
                schema: "Identity",
                table: "IdentityRole",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_DeletedAt",
                schema: "Identity",
                table: "IdentityRole",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_ModifyBy",
                schema: "Identity",
                table: "IdentityRole",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_Name",
                schema: "Identity",
                table: "IdentityRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_RemoveBy",
                schema: "Identity",
                table: "IdentityRole",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_UpdatedAt",
                schema: "Identity",
                table: "IdentityRole",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_ClaimType_ClaimValue",
                schema: "Identity",
                table: "IdentityRoleClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_RoleId_CheckStamp",
                schema: "Identity",
                table: "IdentityRoleClaim",
                columns: new[] { "RoleId", "CheckStamp" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_CreateBy",
                schema: "Identity",
                table: "IdentityUser",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_CreatedAt",
                schema: "Identity",
                table: "IdentityUser",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_DeletedAt",
                schema: "Identity",
                table: "IdentityUser",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_Email",
                schema: "Identity",
                table: "IdentityUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_ModifyBy",
                schema: "Identity",
                table: "IdentityUser",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_PhoneNumber",
                schema: "Identity",
                table: "IdentityUser",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_RemoveBy",
                schema: "Identity",
                table: "IdentityUser",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_UpdatedAt",
                schema: "Identity",
                table: "IdentityUser",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_UserName",
                schema: "Identity",
                table: "IdentityUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_ClaimType_ClaimValue",
                schema: "Identity",
                table: "IdentityUserClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_UserId_CheckStamp",
                schema: "Identity",
                table: "IdentityUserClaim",
                columns: new[] { "UserId", "CheckStamp" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogin_UserId",
                schema: "Identity",
                table: "IdentityUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRole_RoleId",
                schema: "Identity",
                table: "IdentityUserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityRoleClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityUserLogin",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityUserProfile",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityUserRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityUserToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityUser",
                schema: "Identity");
        }
    }
}