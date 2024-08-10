using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.School.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtemisStudent_ArtemisSchool",
                schema: "School",
                table: "ArtemisStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtemisTeacher_ArtemisSchool",
                schema: "School",
                table: "ArtemisTeacher");

            migrationBuilder.CreateTable(
                name: "ArtemisStudentChangeLog",
                schema: "School",
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
                    ChangeType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "变动类型"),
                    ChangeTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "变动时间"),
                    ChangeReason = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "变动原因"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "变动学校标识"),
                    SchoolName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "变动学校名称"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: true, comment: "变动班级标识"),
                    ClassName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "变动班级名称"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学生标识"),
                    StudentName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学生姓名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudentChangeLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentChangeLog_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "School",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentChangeLog_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentChangeLog_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "School",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学生变动记录数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacherChangeLog",
                schema: "School",
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
                    ChangeType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "变动类型"),
                    ChangeTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "变动时间"),
                    ChangeReason = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "变动原因"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "变动学校标识"),
                    SchoolName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "变动学校名称"),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false, comment: "教师标识"),
                    TeacherName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "教师姓名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacherChangeLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherChangeLog_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherChangeLog_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "教师变动记录数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_ClassId",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_CreateBy",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_CreatedAt",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_DeletedAt",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_ModifyBy",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_RemoveBy",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_SchoolId",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_StudentId",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentChangeLog_UpdatedAt",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_CreateBy",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_CreatedAt",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_DeletedAt",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_ModifyBy",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_RemoveBy",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_SchoolId",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_TeacherId",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherChangeLog_UpdatedAt",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                column: "UpdatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtemisStudent_ArtemisSchool",
                schema: "School",
                table: "ArtemisStudent",
                column: "SchoolId",
                principalSchema: "School",
                principalTable: "ArtemisSchool",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtemisTeacher_ArtemisSchool",
                schema: "School",
                table: "ArtemisTeacher",
                column: "SchoolId",
                principalSchema: "School",
                principalTable: "ArtemisSchool",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtemisStudent_ArtemisSchool",
                schema: "School",
                table: "ArtemisStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtemisTeacher_ArtemisSchool",
                schema: "School",
                table: "ArtemisTeacher");

            migrationBuilder.DropTable(
                name: "ArtemisStudentChangeLog",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisTeacherChangeLog",
                schema: "School");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtemisStudent_ArtemisSchool",
                schema: "School",
                table: "ArtemisStudent",
                column: "SchoolId",
                principalSchema: "School",
                principalTable: "ArtemisSchool",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtemisTeacher_ArtemisSchool",
                schema: "School",
                table: "ArtemisTeacher",
                column: "SchoolId",
                principalSchema: "School",
                principalTable: "ArtemisSchool",
                principalColumn: "Id");
        }
    }
}
