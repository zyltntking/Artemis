using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.School.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentAffiliations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "school",
                table: "ArtemisTeacherStudent",
                type: "uuid",
                nullable: false,
                comment: "学生标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "school",
                table: "ArtemisTeacherStudent",
                type: "uuid",
                nullable: false,
                comment: "教师标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                type: "uuid",
                nullable: false,
                comment: "教师标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                type: "uuid",
                nullable: false,
                comment: "学校标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "school",
                table: "ArtemisSchoolStudent",
                type: "uuid",
                nullable: false,
                comment: "学生标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                schema: "school",
                table: "ArtemisSchoolStudent",
                type: "uuid",
                nullable: false,
                comment: "学校标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "school",
                table: "ArtemisClassTeacher",
                type: "uuid",
                nullable: false,
                comment: "教师标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                schema: "school",
                table: "ArtemisClassTeacher",
                type: "uuid",
                nullable: false,
                comment: "班级标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "school",
                table: "ArtemisClassStudent",
                type: "uuid",
                nullable: false,
                comment: "学生标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                schema: "school",
                table: "ArtemisClassStudent",
                type: "uuid",
                nullable: false,
                comment: "班级标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "ArtemisStudentCurrentAffiliation",
                schema: "school",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学生标识"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false, comment: "班级标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudentCurrentAffiliation", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentCurrentAffiliation_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "school",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentCurrentAffiliation_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "school",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentCurrentAffiliation_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "school",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学生当前所属关系数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacherCurrentAffiliation",
                schema: "school",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false, comment: "教师标识"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacherCurrentAffiliation", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherCurrentAffiliation_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "school",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherCurrentAffiliation_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "school",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "教师当前所属关系数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentCurrentAffiliation_ClassId",
                schema: "school",
                table: "ArtemisStudentCurrentAffiliation",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentCurrentAffiliation_SchoolId",
                schema: "school",
                table: "ArtemisStudentCurrentAffiliation",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherCurrentAffiliation_SchoolId",
                schema: "school",
                table: "ArtemisTeacherCurrentAffiliation",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisStudentCurrentAffiliation",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisTeacherCurrentAffiliation",
                schema: "school");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "school",
                table: "ArtemisTeacherStudent",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学生标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "school",
                table: "ArtemisTeacherStudent",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "教师标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "教师标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学校标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "school",
                table: "ArtemisSchoolStudent",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学生标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                schema: "school",
                table: "ArtemisSchoolStudent",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学校标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "school",
                table: "ArtemisClassTeacher",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "教师标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                schema: "school",
                table: "ArtemisClassTeacher",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "班级标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "school",
                table: "ArtemisClassStudent",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学生标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                schema: "school",
                table: "ArtemisClassStudent",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "班级标识");
        }
    }
}
