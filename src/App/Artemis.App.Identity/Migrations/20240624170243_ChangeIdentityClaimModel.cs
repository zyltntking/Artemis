using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdentityClaimModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "identity",
                table: "IdentityClaim",
                type: "uuid",
                nullable: false,
                comment: "标识",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "标识")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "CreateBy",
                schema: "identity",
                table: "IdentityClaim",
                type: "UUID",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "创建者标识");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "identity",
                table: "IdentityClaim",
                type: "TIMESTAMP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "创建时间");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "identity",
                table: "IdentityClaim",
                type: "TIMESTAMP",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifyBy",
                schema: "identity",
                table: "IdentityClaim",
                type: "UUID",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "更新者标识");

            migrationBuilder.AddColumn<Guid>(
                name: "RemoveBy",
                schema: "identity",
                table: "IdentityClaim",
                type: "UUID",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "删除者标识");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "identity",
                table: "IdentityClaim",
                type: "TIMESTAMP",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "更新时间");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_CreateBy",
                schema: "identity",
                table: "IdentityClaim",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_CreatedAt",
                schema: "identity",
                table: "IdentityClaim",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_DeletedAt",
                schema: "identity",
                table: "IdentityClaim",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_ModifyBy",
                schema: "identity",
                table: "IdentityClaim",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_RemoveBy",
                schema: "identity",
                table: "IdentityClaim",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityClaim_UpdatedAt",
                schema: "identity",
                table: "IdentityClaim",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityClaim_CreateBy",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityClaim_CreatedAt",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityClaim_DeletedAt",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityClaim_ModifyBy",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityClaim_RemoveBy",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityClaim_UpdatedAt",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropColumn(
                name: "RemoveBy",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "identity",
                table: "IdentityClaim");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "identity",
                table: "IdentityClaim",
                type: "integer",
                nullable: false,
                comment: "标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "标识")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
