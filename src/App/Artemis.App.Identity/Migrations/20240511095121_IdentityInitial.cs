using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    /// <inheritdoc />
    public partial class IdentityInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "ArtemisClaim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效"),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据值"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClaim", x => x.Id);
                },
                comment: "认证凭据数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisRole",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效"),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "角色名"),
                    NormalizedName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "规范化角色名"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "角色描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisRole", x => x.Id);
                },
                comment: "认证角色数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisUser",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效"),
                    UserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "用户名"),
                    NormalizedUserName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "规范化用户名"),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "邮箱地址"),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "规范化邮箱地址"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "是否确认邮箱地址"),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "密码哈希"),
                    SecurityStamp = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "加密锁"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "电话号码"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false, comment: "是否确认电话号码"),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false, comment: "是否允许双步认证"),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "用户锁定到期时间标记"),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false, comment: "是否允许锁定用户"),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false, comment: "尝试错误数量"),
                    NormalizedPhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "规范化电话号码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisUser", x => x.Id);
                },
                comment: "认证用户数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisRoleClaim",
                schema: "identity",
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
                    table.PrimaryKey("PK_ArtemisRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisUserClaim_ArtemisRole_Id",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "ArtemisRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证角色凭据数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisUserClaim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "标识")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    ClaimType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据类型"),
                    ClaimValue = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "凭据类型"),
                    CheckStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "校验戳"),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "凭据描述")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisUserClaim_ArtemisUser_Id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisUserLogin",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "认证提供程序"),
                    ProviderKey = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "认证提供程序提供的第三方标识"),
                    ProviderDisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "认证提供程序显示的用户名"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ArtemisUserLogin_ArtemisUser_Id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户登录数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisUserProfile",
                schema: "identity",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "用户信息键"),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "用户信息值"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisUserProfile", x => new { x.UserId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_ArtemisUserProfile_ArtemisUser_Id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户信息数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisUserRole",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, comment: "角色标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ArtemisUserRole_ArtemisRole_Id",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "ArtemisRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisUserRole_ArtemisUser_Id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户角色映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisUserToken",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    LoginProvider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "认证提供程序"),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "认证令牌名"),
                    Value = table.Column<string>(type: "text", nullable: true, comment: "认证令牌")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_ArtemisUserToken_ArtemisUser_Id",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "认证用户令牌数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClaim_CheckStamp",
                schema: "identity",
                table: "ArtemisClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClaim_ClaimType_ClaimValue",
                schema: "identity",
                table: "ArtemisClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClaim_CreatedAt",
                schema: "identity",
                table: "ArtemisClaim",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClaim_DeletedAt",
                schema: "identity",
                table: "ArtemisClaim",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClaim_UpdatedAt",
                schema: "identity",
                table: "ArtemisClaim",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRole_CreatedAt",
                schema: "identity",
                table: "ArtemisRole",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRole_DeletedAt",
                schema: "identity",
                table: "ArtemisRole",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRole_Name",
                schema: "identity",
                table: "ArtemisRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRole_UpdatedAt",
                schema: "identity",
                table: "ArtemisRole",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRoleClaim_CheckStamp",
                schema: "identity",
                table: "ArtemisRoleClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRoleClaim_ClaimType_ClaimValue",
                schema: "identity",
                table: "ArtemisRoleClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRoleClaim_RoleId",
                schema: "identity",
                table: "ArtemisRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUser_CreatedAt",
                schema: "identity",
                table: "ArtemisUser",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUser_DeletedAt",
                schema: "identity",
                table: "ArtemisUser",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUser_Email",
                schema: "identity",
                table: "ArtemisUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUser_PhoneNumber",
                schema: "identity",
                table: "ArtemisUser",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUser_UpdatedAt",
                schema: "identity",
                table: "ArtemisUser",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUser_UserName",
                schema: "identity",
                table: "ArtemisUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUserClaim_CheckStamp",
                schema: "identity",
                table: "ArtemisUserClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUserClaim_ClaimType_ClaimValue",
                schema: "identity",
                table: "ArtemisUserClaim",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUserClaim_UserId",
                schema: "identity",
                table: "ArtemisUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUserLogin_UserId",
                schema: "identity",
                table: "ArtemisUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUserProfile_Key_Value",
                schema: "identity",
                table: "ArtemisUserProfile",
                columns: new[] { "Key", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisUserRole_RoleId",
                schema: "identity",
                table: "ArtemisUserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisRoleClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisUserClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisUserLogin",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisUserProfile",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisUserRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisUserToken",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisUser",
                schema: "identity");
        }
    }
}
