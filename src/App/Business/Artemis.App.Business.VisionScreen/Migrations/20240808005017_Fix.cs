using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecordFeedBack",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "筛查报告反馈");

            migrationBuilder.CreateTable(
                name: "ArtemisStudentEyePhoto",
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
                    LeftEyePhoto = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "左眼照片"),
                    RightEyePhoto = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "右眼照片"),
                    BothEyePhoto = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "双眼照片"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudentEyePhoto", x => x.Id);
                },
                comment: "学生眼部照片数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_CreateBy",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_CreatedAt",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_DeletedAt",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_ModifyBy",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_Partition",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_RemoveBy",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentEyePhoto_UpdatedAt",
                schema: "Business",
                table: "ArtemisStudentEyePhoto",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisStudentEyePhoto",
                schema: "Business");

            migrationBuilder.DropColumn(
                name: "RecordFeedBack",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");
        }
    }
}
