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
                name: "School");

            migrationBuilder.CreateTable(
                name: "ArtemisSchool",
                schema: "School",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学校名称"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学校编码"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学校类型"),
                    OrganizationCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "组织机构代码"),
                    DivisionCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学校所在地行政区划代码"),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "学校地址"),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校邮箱"),
                    WebSite = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校网站"),
                    ContactNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学校联系电话"),
                    EstablishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "学校建立时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchool", x => x.Id);
                },
                comment: "学校数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisStudent",
                schema: "School",
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
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学生名称"),
                    Gender = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false, comment: "学生性别"),
                    Major = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "专业"),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false, comment: "学生生日"),
                    Nation = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "民族"),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学生编码"),
                    StudentNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学籍号"),
                    Cert = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "证件号码"),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "入学时间"),
                    DivisionCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "住址区划代码"),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "住址")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudent", x => x.Id);
                },
                comment: "学生数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacher",
                schema: "School",
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
                name: "ArtemisSchoolStudent",
                schema: "School",
                columns: table => new
                {
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学生标识"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchoolStudent", x => new { x.SchoolId, x.StudentId, x.MoveIn });
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolStudent_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolStudent_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "School",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学校学生映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClass",
                schema: "School",
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
                    HeadTeacherId = table.Column<Guid>(type: "uuid", nullable: false, comment: "班主任标识"),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "班级名称"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "班级类型"),
                    Major = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "所学专业"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "班级编码"),
                    StudyPhase = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学段"),
                    SchoolLength = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学制"),
                    Length = table.Column<int>(type: "integer", nullable: false, comment: "学制长度"),
                    SerialNumber = table.Column<int>(type: "integer", nullable: false, comment: "班级序号"),
                    EstablishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "班级创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisClass_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacher_ArtemisClass",
                        column: x => x.HeadTeacherId,
                        principalSchema: "School",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "班级数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisSchoolTeacher",
                schema: "School",
                columns: table => new
                {
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识"),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false, comment: "教师标识"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchoolTeacher", x => new { x.SchoolId, x.TeacherId, x.MoveIn });
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolTeacher_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisSchoolTeacher_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学校教师映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClassStudent",
                schema: "School",
                columns: table => new
                {
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false, comment: "班级标识"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学生标识"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClassStudent", x => new { x.ClassId, x.StudentId, x.MoveIn });
                    table.ForeignKey(
                        name: "FK_ArtemisClassStudent_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "School",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisClassStudent_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "School",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "班级学生映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClassTeacher",
                schema: "School",
                columns: table => new
                {
                    MoveIn = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "转入时间"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false, comment: "班级标识"),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false, comment: "教师标识"),
                    MoveOut = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "转出时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisClassTeacher", x => new { x.ClassId, x.TeacherId, x.MoveIn });
                    table.ForeignKey(
                        name: "FK_ArtemisClassTeacher_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "School",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisClassTeacher_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "班级教师映射数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisStudentCurrentAffiliation",
                schema: "School",
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
                        principalSchema: "School",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentCurrentAffiliation_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisStudentCurrentAffiliation_ArtemisStudent",
                        column: x => x.StudentId,
                        principalSchema: "School",
                        principalTable: "ArtemisStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "学生当前所属关系数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacherCurrentAffiliation",
                schema: "School",
                columns: table => new
                {
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: false, comment: "班级标识"),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false, comment: "教师标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacherCurrentAffiliation", x => new { x.SchoolId, x.ClassId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherCurrentAffiliation_ArtemisClass",
                        column: x => x.ClassId,
                        principalSchema: "School",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherCurrentAffiliation_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacherCurrentAffiliation_ArtemisTeacher",
                        column: x => x.TeacherId,
                        principalSchema: "School",
                        principalTable: "ArtemisTeacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "教师当前所属关系数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_CreateBy",
                schema: "School",
                table: "ArtemisClass",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_CreatedAt",
                schema: "School",
                table: "ArtemisClass",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_DeletedAt",
                schema: "School",
                table: "ArtemisClass",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_HeadTeacherId",
                schema: "School",
                table: "ArtemisClass",
                column: "HeadTeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_ModifyBy",
                schema: "School",
                table: "ArtemisClass",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_Partition",
                schema: "School",
                table: "ArtemisClass",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_RemoveBy",
                schema: "School",
                table: "ArtemisClass",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_SchoolId",
                schema: "School",
                table: "ArtemisClass",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_UpdatedAt",
                schema: "School",
                table: "ArtemisClass",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassStudent_MoveIn",
                schema: "School",
                table: "ArtemisClassStudent",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassStudent_MoveOut",
                schema: "School",
                table: "ArtemisClassStudent",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassStudent_StudentId",
                schema: "School",
                table: "ArtemisClassStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassTeacher_MoveIn",
                schema: "School",
                table: "ArtemisClassTeacher",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassTeacher_MoveOut",
                schema: "School",
                table: "ArtemisClassTeacher",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClassTeacher_TeacherId",
                schema: "School",
                table: "ArtemisClassTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_CreateBy",
                schema: "School",
                table: "ArtemisSchool",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_CreatedAt",
                schema: "School",
                table: "ArtemisSchool",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_DeletedAt",
                schema: "School",
                table: "ArtemisSchool",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_ModifyBy",
                schema: "School",
                table: "ArtemisSchool",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_Partition",
                schema: "School",
                table: "ArtemisSchool",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_RemoveBy",
                schema: "School",
                table: "ArtemisSchool",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_UpdatedAt",
                schema: "School",
                table: "ArtemisSchool",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolStudent_MoveIn",
                schema: "School",
                table: "ArtemisSchoolStudent",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolStudent_MoveOut",
                schema: "School",
                table: "ArtemisSchoolStudent",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolStudent_StudentId",
                schema: "School",
                table: "ArtemisSchoolStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolTeacher_MoveIn",
                schema: "School",
                table: "ArtemisSchoolTeacher",
                column: "MoveIn");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolTeacher_MoveOut",
                schema: "School",
                table: "ArtemisSchoolTeacher",
                column: "MoveOut");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchoolTeacher_TeacherId",
                schema: "School",
                table: "ArtemisSchoolTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_CreateBy",
                schema: "School",
                table: "ArtemisStudent",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_CreatedAt",
                schema: "School",
                table: "ArtemisStudent",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_DeletedAt",
                schema: "School",
                table: "ArtemisStudent",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_ModifyBy",
                schema: "School",
                table: "ArtemisStudent",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_Partition",
                schema: "School",
                table: "ArtemisStudent",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_RemoveBy",
                schema: "School",
                table: "ArtemisStudent",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_UpdatedAt",
                schema: "School",
                table: "ArtemisStudent",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentCurrentAffiliation_ClassId",
                schema: "School",
                table: "ArtemisStudentCurrentAffiliation",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentCurrentAffiliation_SchoolId",
                schema: "School",
                table: "ArtemisStudentCurrentAffiliation",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_CreateBy",
                schema: "School",
                table: "ArtemisTeacher",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_CreatedAt",
                schema: "School",
                table: "ArtemisTeacher",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_DeletedAt",
                schema: "School",
                table: "ArtemisTeacher",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_ModifyBy",
                schema: "School",
                table: "ArtemisTeacher",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_Partition",
                schema: "School",
                table: "ArtemisTeacher",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_RemoveBy",
                schema: "School",
                table: "ArtemisTeacher",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_UpdatedAt",
                schema: "School",
                table: "ArtemisTeacher",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherCurrentAffiliation_ClassId",
                schema: "School",
                table: "ArtemisTeacherCurrentAffiliation",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacherCurrentAffiliation_TeacherId",
                schema: "School",
                table: "ArtemisTeacherCurrentAffiliation",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisClassStudent",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisClassTeacher",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisSchoolStudent",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisSchoolTeacher",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisStudentCurrentAffiliation",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisTeacherCurrentAffiliation",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisStudent",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisClass",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisSchool",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisTeacher",
                schema: "School");
        }
    }
}
