using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.School.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "school");

            migrationBuilder.CreateTable(
                name: "ArtemisSchool",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
                    Partition = table.Column<int>(type: "integer", nullable: false, comment: "分区标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    SchoolName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学校名称"),
                    SchoolCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学校编码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchool", x => x.Id);
                },
                comment: "学校数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisStudent",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
                    Partition = table.Column<int>(type: "integer", nullable: false, comment: "分区标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    StudentName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学生名称"),
                    StudentCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学生编码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudent", x => x.Id);
                },
                comment: "学生数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacher",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
                    Partition = table.Column<int>(type: "integer", nullable: false, comment: "分区标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    TeacherName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "教师名称"),
                    TeacherCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "教师编码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacher", x => x.Id);
                },
                comment: "教师数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClass",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
                    Partition = table.Column<int>(type: "integer", nullable: false, comment: "分区标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识"),
                    ClassName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "班级名称"),
                    ClassCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "班级编码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisClass_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "school",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "班级数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisSchoolStudent",
                schema: "school",
                columns: table => new
                {
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchoolStudent", x => new { x.SchoolId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolStudent_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "school",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolStudent_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "school",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学校学生映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisSchoolTeacher",
                schema: "school",
                columns: table => new
                {
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchoolTeacher", x => new { x.SchoolId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolTeacher_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "school",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolTeacher_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "school",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学校教师映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacherStudent",
                schema: "school",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacherStudent", x => new { x.TeacherId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherStudent_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "school",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherStudent_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "school",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "教师学生映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClassStudent",
                schema: "school",
                columns: table => new
                {
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClassStudent", x => new { x.ClassId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ArtemisClassStudent_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "school",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisClassStudent_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "school",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "班级学生映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClassTeacher",
                schema: "school",
                columns: table => new
                {
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClassTeacher", x => new { x.ClassId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ArtemisClassTeacher_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "school",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisClassTeacher_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "school",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "班级教师映射数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_CreateBy",
                schema: "school",
                table: "ArtemisClass",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_CreatedAt",
                schema: "school",
                table: "ArtemisClass",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_DeletedAt",
                schema: "school",
                table: "ArtemisClass",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_ModifyBy",
                schema: "school",
                table: "ArtemisClass",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_Partition",
                schema: "school",
                table: "ArtemisClass",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_RemoveBy",
                schema: "school",
                table: "ArtemisClass",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_SchoolId",
                schema: "school",
                table: "ArtemisClass",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_UpdatedAt",
                schema: "school",
                table: "ArtemisClass",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassStudent_MoveIn",
                schema: "school",
                table: "ArtemisClassStudent",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassStudent_MoveOut",
                schema: "school",
                table: "ArtemisClassStudent",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassStudent_StudentId",
                schema: "school",
                table: "ArtemisClassStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassTeacher_MoveIn",
                schema: "school",
                table: "ArtemisClassTeacher",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassTeacher_MoveOut",
                schema: "school",
                table: "ArtemisClassTeacher",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassTeacher_TeacherId",
                schema: "school",
                table: "ArtemisClassTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_CreateBy",
                schema: "school",
                table: "ArtemisSchool",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_CreatedAt",
                schema: "school",
                table: "ArtemisSchool",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_DeletedAt",
                schema: "school",
                table: "ArtemisSchool",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_ModifyBy",
                schema: "school",
                table: "ArtemisSchool",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_Partition",
                schema: "school",
                table: "ArtemisSchool",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_RemoveBy",
                schema: "school",
                table: "ArtemisSchool",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_UpdatedAt",
                schema: "school",
                table: "ArtemisSchool",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolStudent_MoveIn",
                schema: "school",
                table: "ArtemisSchoolStudent",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolStudent_MoveOut",
                schema: "school",
                table: "ArtemisSchoolStudent",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolStudent_StudentId",
                schema: "school",
                table: "ArtemisSchoolStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolTeacher_MoveIn",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolTeacher_MoveOut",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolTeacher_TeacherId",
                schema: "school",
                table: "ArtemisSchoolTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_CreateBy",
                schema: "school",
                table: "ArtemisStudent",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_CreatedAt",
                schema: "school",
                table: "ArtemisStudent",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_DeletedAt",
                schema: "school",
                table: "ArtemisStudent",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_ModifyBy",
                schema: "school",
                table: "ArtemisStudent",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_Partition",
                schema: "school",
                table: "ArtemisStudent",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_RemoveBy",
                schema: "school",
                table: "ArtemisStudent",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_UpdatedAt",
                schema: "school",
                table: "ArtemisStudent",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_CreateBy",
                schema: "school",
                table: "ArtemisTeacher",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_CreatedAt",
                schema: "school",
                table: "ArtemisTeacher",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_DeletedAt",
                schema: "school",
                table: "ArtemisTeacher",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_ModifyBy",
                schema: "school",
                table: "ArtemisTeacher",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_Partition",
                schema: "school",
                table: "ArtemisTeacher",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_RemoveBy",
                schema: "school",
                table: "ArtemisTeacher",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_UpdatedAt",
                schema: "school",
                table: "ArtemisTeacher",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherStudent_MoveIn",
                schema: "school",
                table: "ArtemisTeacherStudent",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherStudent_MoveOut",
                schema: "school",
                table: "ArtemisTeacherStudent",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherStudent_StudentId",
                schema: "school",
                table: "ArtemisTeacherStudent",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisClassStudent",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisClassTeacher",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisSchoolStudent",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisSchoolTeacher",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisTeacherStudent",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisClass",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisStudent",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisTeacher",
                schema: "school");

            migrationBuilder.DropTable(
                name: "ArtemisSchool",
                schema: "school");
        }
    }
}
