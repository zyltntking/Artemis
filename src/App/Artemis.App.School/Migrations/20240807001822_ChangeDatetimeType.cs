using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.School.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatetimeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EstablishTime",
                schema: "School",
                table: "ArtemisClass",
                type: "TIMESTAMP",
                nullable: true,
                comment: "班级创建时间",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "班级创建时间");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EstablishTime",
                schema: "School",
                table: "ArtemisClass",
                type: "timestamp with time zone",
                nullable: true,
                comment: "班级创建时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "班级创建时间");
        }
    }
}
