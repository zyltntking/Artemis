using System;
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
                name: "identity");

            migrationBuilder.CreateTable(
                name: "IdentityClaim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "标识")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
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
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    UserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "用户名"),
                    NormalizedUserName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "标准化用户名"),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "电子邮件"),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "标准化电子邮件"),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "电话号码"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "电子邮件确认戳"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "电话号码确认戳"),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "密码哈希"),
                    SecurityStamp = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "密码锁"),
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
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "标识")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据值"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "角色标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityRoleClaim_IdentityRole",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证角色凭据数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "标识")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据值"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户凭据数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin",
                schema: "identity",
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
                        principalSchema: "identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色登录数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserRole",
                schema: "identity",
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
                        principalSchema: "identity",
                        principalTable: "IdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentityUserRole_IdentityUser",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色映射数据集");

            migrationBuilder.CreateTable(
                name: "IdentityUserToken",
                schema: "identity",
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
                        principalSchema: "identity",
                        principalTable: "IdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户令牌数据集");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_CheckStamp",
                schema: "identity",
                table: "IdentityClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_ClaimType_ClaimValue",
                schema: "identity",
                table: "IdentityClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_CreateBy",
                schema: "identity",
                table: "IdentityRole",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_CreatedAt",
                schema: "identity",
                table: "IdentityRole",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_DeletedAt",
                schema: "identity",
                table: "IdentityRole",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_ModifyBy",
                schema: "identity",
                table: "IdentityRole",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_Name",
                schema: "identity",
                table: "IdentityRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_RemoveBy",
                schema: "identity",
                table: "IdentityRole",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_UpdatedAt",
                schema: "identity",
                table: "IdentityRole",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_CheckStamp",
                schema: "identity",
                table: "IdentityRoleClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_ClaimType_ClaimValue",
                schema: "identity",
                table: "IdentityRoleClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_RoleId",
                schema: "identity",
                table: "IdentityRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_CreateBy",
                schema: "identity",
                table: "IdentityUser",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_CreatedAt",
                schema: "identity",
                table: "IdentityUser",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_DeletedAt",
                schema: "identity",
                table: "IdentityUser",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_Email",
                schema: "identity",
                table: "IdentityUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_ModifyBy",
                schema: "identity",
                table: "IdentityUser",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_PhoneNumber",
                schema: "identity",
                table: "IdentityUser",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_RemoveBy",
                schema: "identity",
                table: "IdentityUser",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_UpdatedAt",
                schema: "identity",
                table: "IdentityUser",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_UserName",
                schema: "identity",
                table: "IdentityUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_CheckStamp",
                schema: "identity",
                table: "IdentityUserClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_ClaimType_ClaimValue",
                schema: "identity",
                table: "IdentityUserClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_UserId",
                schema: "identity",
                table: "IdentityUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogin_UserId",
                schema: "identity",
                table: "IdentityUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRole_RoleId",
                schema: "identity",
                table: "IdentityUserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityRoleClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityUserLogin",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityUserRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityUserToken",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "IdentityUser",
                schema: "identity");
        }
    }
}
