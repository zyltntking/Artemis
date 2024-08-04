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
                    CreateBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "删除者标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "学校名称"),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校编码"),
                    BindingTag = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "绑定标记"),
                    Type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校类型"),
                    OrganizationCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "组织机构代码"),
                    DivisionCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学校所在地行政区划代码"),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校地址"),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校邮箱"),
                    WebSite = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "学校网站"),
                    ContactNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学校联系电话"),
                    EstablishTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "学校建立时间"),
                    Introduction = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "学校简介"),
                    Remark = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "备注")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSchool", x => x.Id);
                },
                comment: "学校数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTeacher",
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
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true, comment: "学校标识"),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "教师名称"),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "教师编码"),
                    EntryTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "入职时间"),
                    Gender = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "教师性别"),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "教师职称"),
                    Education = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "教师学历"),
                    IdCard = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "教师身份证号"),
                    NativePlace = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "教师籍贯"),
                    PoliticalStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "政治面貌"),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "家庭住址"),
                    Birthday = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "生日"),
                    Phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "联系电话"),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "邮箱"),
                    Remark = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "备注")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTeacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisTeacher_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id");
                },
                comment: "教师数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisClass",
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
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学校标识"),
                    HeadTeacherId = table.Column<Guid>(type: "uuid", nullable: true, comment: "班主任标识"),
                    HeadTeacherName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "班主任名称"),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "班级名称"),
                    GradeName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "年级名称"),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "班级类型"),
                    Major = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "所学专业"),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "班级编码"),
                    StudyPhase = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学段"),
                    SchoolLength = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学制"),
                    Length = table.Column<int>(type: "integer", nullable: false, comment: "学制长度"),
                    SerialNumber = table.Column<int>(type: "integer", nullable: false, comment: "班级序号"),
                    EstablishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "班级创建时间"),
                    Remark = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "备注")
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
                        principalColumn: "Id");
                },
                comment: "班级数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisStudent",
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
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: true, comment: "学校标识"),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: true, comment: "班级标识"),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学生名称"),
                    Gender = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学生性别"),
                    Birthday = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "学生生日"),
                    Nation = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "民族"),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "学生编码"),
                    StudentNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "学籍号"),
                    Cert = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "证件号码"),
                    EnrollmentDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "入学时间"),
                    DivisionCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "住址区划代码"),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "住址"),
                    Remark = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "备注")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisClass_ArtemisStudent",
                        column: x => x.ClassId,
                        principalSchema: "School",
                        principalTable: "ArtemisClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArtemisStudent_ArtemisSchool",
                        column: x => x.SchoolId,
                        principalSchema: "School",
                        principalTable: "ArtemisSchool",
                        principalColumn: "Id");
                },
                comment: "学生数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_Code",
                schema: "School",
                table: "ArtemisClass",
                column: "Code");

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
                name: "IX_ArtemisClass_GradeName",
                schema: "School",
                table: "ArtemisClass",
                column: "GradeName");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_HeadTeacherId",
                schema: "School",
                table: "ArtemisClass",
                column: "HeadTeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_Length",
                schema: "School",
                table: "ArtemisClass",
                column: "Length");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_ModifyBy",
                schema: "School",
                table: "ArtemisClass",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_Name",
                schema: "School",
                table: "ArtemisClass",
                column: "Name");

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
                name: "IX_ArtemisClass_SerialNumber",
                schema: "School",
                table: "ArtemisClass",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_StudyPhase",
                schema: "School",
                table: "ArtemisClass",
                column: "StudyPhase");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisClass_UpdatedAt",
                schema: "School",
                table: "ArtemisClass",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_BindingTag",
                schema: "School",
                table: "ArtemisSchool",
                column: "BindingTag");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_Code",
                schema: "School",
                table: "ArtemisSchool",
                column: "Code");

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
                name: "IX_ArtemisSchool_DivisionCode",
                schema: "School",
                table: "ArtemisSchool",
                column: "DivisionCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_ModifyBy",
                schema: "School",
                table: "ArtemisSchool",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_Name",
                schema: "School",
                table: "ArtemisSchool",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_OrganizationCode",
                schema: "School",
                table: "ArtemisSchool",
                column: "OrganizationCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_RemoveBy",
                schema: "School",
                table: "ArtemisSchool",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_Type",
                schema: "School",
                table: "ArtemisSchool",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSchool_UpdatedAt",
                schema: "School",
                table: "ArtemisSchool",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_ClassId",
                schema: "School",
                table: "ArtemisStudent",
                column: "ClassId");

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
                name: "IX_ArtemisStudent_Gender",
                schema: "School",
                table: "ArtemisStudent",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_ModifyBy",
                schema: "School",
                table: "ArtemisStudent",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_Name",
                schema: "School",
                table: "ArtemisStudent",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_Nation",
                schema: "School",
                table: "ArtemisStudent",
                column: "Nation");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_RemoveBy",
                schema: "School",
                table: "ArtemisStudent",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_SchoolId",
                schema: "School",
                table: "ArtemisStudent",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_StudentNumber",
                schema: "School",
                table: "ArtemisStudent",
                column: "StudentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudent_UpdatedAt",
                schema: "School",
                table: "ArtemisStudent",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_Code",
                schema: "School",
                table: "ArtemisTeacher",
                column: "Code");

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
                name: "IX_ArtemisTeacher_Name",
                schema: "School",
                table: "ArtemisTeacher",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_RemoveBy",
                schema: "School",
                table: "ArtemisTeacher",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_SchoolId",
                schema: "School",
                table: "ArtemisTeacher",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTeacher_UpdatedAt",
                schema: "School",
                table: "ArtemisTeacher",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisStudent",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisClass",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisTeacher",
                schema: "School");

            migrationBuilder.DropTable(
                name: "ArtemisSchool",
                schema: "School");
        }
    }
}
