using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentNumber",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "学籍号");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacherUserBinding",
                schema: "Business",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "删除者标识"),
                    Partition = table.Column<int>(type: "integer", nullable: false, comment: "分区标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacherUserBinding", x => x.Id);
                },
                comment: "用户教师绑定数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_CreateBy",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_CreatedAt",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_DeletedAt",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_ModifyBy",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_Partition",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_RemoveBy",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_UpdatedAt",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisTeacherUserBinding",
                schema: "Business");

            migrationBuilder.DropColumn(
                name: "StudentNumber",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");
        }
    }
}
