using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportSendTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportReceiveTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PrescribedTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OptometerOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChartOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HeadTeacherId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadTeacherName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolLengthValue",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "HeadTeacherId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "HeadTeacherName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "SchoolLengthValue",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.AlterColumn<double>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportSendTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportReceiveTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PrescribedTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OptometerOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChartOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Birthday",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);
        }
    }
}
