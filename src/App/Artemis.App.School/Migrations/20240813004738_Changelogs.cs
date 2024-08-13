using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.School.Migrations
{
    /// <inheritdoc />
    public partial class Changelogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                schema: "School",
                table: "ArtemisTeacherChangeLog",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "学校所在行政区划编码");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                type: "TIMESTAMP",
                nullable: true,
                comment: "学生生日");

            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "学校所在行政区划编码");

            migrationBuilder.AddColumn<string>(
                name: "GradeName",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "年级名称");

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                schema: "School",
                table: "ArtemisStudentChangeLog",
                type: "integer",
                nullable: true,
                comment: "班级序列");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionCode",
                schema: "School",
                table: "ArtemisTeacherChangeLog");

            migrationBuilder.DropColumn(
                name: "Birthday",
                schema: "School",
                table: "ArtemisStudentChangeLog");

            migrationBuilder.DropColumn(
                name: "DivisionCode",
                schema: "School",
                table: "ArtemisStudentChangeLog");

            migrationBuilder.DropColumn(
                name: "GradeName",
                schema: "School",
                table: "ArtemisStudentChangeLog");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                schema: "School",
                table: "ArtemisStudentChangeLog");
        }
    }
}
