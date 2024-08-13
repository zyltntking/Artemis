using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AuthCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityAuthCode",
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
                    Sign = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "签名"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "验证码"),
                    SendTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "发送时间"),
                    ExpireTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "过期时间"),
                    Used = table.Column<bool>(type: "boolean", nullable: false, comment: "是否使用")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityAuthCode", x => x.Id);
                },
                comment: "认证验证码数据集");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_Code",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_CreateBy",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_CreatedAt",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_DeletedAt",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_ModifyBy",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_RemoveBy",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_SendTime",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "SendTime");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_Sign",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "Sign");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_UpdatedAt",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityAuthCode_Used",
                schema: "Identity",
                table: "IdentityAuthCode",
                column: "Used");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityAuthCode",
                schema: "Identity");
        }
    }
}
