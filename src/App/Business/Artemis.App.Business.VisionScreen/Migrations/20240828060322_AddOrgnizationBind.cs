using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgnizationBind : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtemisOrganizationUsersBinding",
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
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisOrganizationUsersBinding", x => x.Id);
                },
                comment: "机构用户关系绑定数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherUserBinding_TeacherId",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "TeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserId",
                schema: "Business",
                table: "ArtemisTeacherUserBinding",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_UserId",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "StudentId",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_CreateBy",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_CreatedAt",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_DeletedAt",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_ModifyBy",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_Partition",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_RemoveBy",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_UpdatedAt",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOrganizationUsersBinding_UserId",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "OrganizationId",
                schema: "Business",
                table: "ArtemisOrganizationUsersBinding",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisOrganizationUsersBinding",
                schema: "Business");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisTeacherUserBinding_TeacherId",
                schema: "Business",
                table: "ArtemisTeacherUserBinding");

            migrationBuilder.DropIndex(
                name: "UserId",
                schema: "Business",
                table: "ArtemisTeacherUserBinding");

            migrationBuilder.DropIndex(
                name: "IX_ArtemisStudentRelationBinding_UserId",
                schema: "Business",
                table: "ArtemisStudentRelationBinding");

            migrationBuilder.DropIndex(
                name: "StudentId",
                schema: "Business",
                table: "ArtemisStudentRelationBinding");
        }
    }
}
