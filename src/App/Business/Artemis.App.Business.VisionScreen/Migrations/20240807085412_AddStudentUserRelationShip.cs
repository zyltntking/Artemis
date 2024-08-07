using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentUserRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChartChecked",
                schema: "Business",
                table: "ArtemisVisualChart");

            migrationBuilder.AlterColumn<double>(
                name: "RightNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                comment: "右眼裸眼视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "右眼远视类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCorrectedVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                comment: "右眼矫正视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightChartDistance",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                comment: "右眼与视力表的距离",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RecordId",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "uuid",
                nullable: false,
                comment: "档案标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<double>(
                name: "LeftNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                comment: "左眼裸眼视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "左眼远视类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCorrectedVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                comment: "左眼矫正视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftChartDistance",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                comment: "左眼与视力表的距离",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareRightOkLenses",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: true,
                comment: "右眼是否佩戴角膜塑形镜",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareLeftOkLenses",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: true,
                comment: "左眼是否佩戴角膜塑形镜",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareGlasses",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: false,
                comment: "是否佩戴眼镜",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "ChartScreeningStuffName",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "筛查工作人员姓名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChartOperationTime",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "TIMESTAMP",
                nullable: true,
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "VisualStandardId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "应用的视力标准标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskUnitTargetId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "任务目标标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "TaskUnitTargetCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务目标编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskUnitName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务单元名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskUnitId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "任务单元标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "TaskUnitCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务单元编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "任务标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "TaskCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskAgentType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务代理类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskAgentName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务代理名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskAgentId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true,
                comment: "任务代理标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskAgentCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "任务代理编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudyPhase",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "学段",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "学生名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "学生标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "学生编号",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SchoolType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "学校类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SchoolName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "学校名称",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "SchoolLengthValue",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                comment: "学制值",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SchoolLength",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "学制",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "学校标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "学校编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼球镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼裸眼视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "右眼远视类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RightEquivalentSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                comment: "右眼等效球径度数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCylinder",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼柱镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCorrectedVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼矫正视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r2(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r1(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r2(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r1(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率平均值(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率平均值(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率散光度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r2角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r1角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightChartDistance",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼与视力表的距离",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼轴位",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼散光度数",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportSender",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "报告发送人",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportSendTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "报告发送时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportReceiver",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "报告签收人",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportReceiveTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "报告签收时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PupilDistance",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "瞳距",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PrescribedTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "医嘱时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "组织机构名称",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "组织机构标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationDesignCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "组织机构设计编码",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "组织机构编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OptometerScreeningStuffName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "筛查工作人员姓名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OptometerOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OptometerCheckedTimes",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: false,
                comment: "验光仪筛查次数",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "LeftSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼球镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼裸眼视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "左眼远视类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LeftEquivalentSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                comment: "左眼等效球镜度数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCylinder",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼柱镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCorrectedVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼矫正视力",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r2(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r1(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r2(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r1(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率平均值(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率平均值(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率散光度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r2角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r1角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftChartDistance",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼与视力表的距离",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼轴位",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼散光度数",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareRightOkLenses",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: true,
                comment: "右眼是否佩戴角膜塑形镜",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareLeftOkLenses",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: true,
                comment: "左眼是否佩戴角膜塑形镜",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareGlasses",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                comment: "是否佩戴眼镜",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOptometerChecked",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                comment: "是否已经过验光仪筛查",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsChartChecked",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                comment: "是否已经过电子视力表筛查",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "HeadTeacherName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "班主任名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "HeadTeacherId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true,
                comment: "班主任标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GradeName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "年级名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "性别",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExceptionReason",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "筛查结果",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "医师姓名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorAdvice",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "医嘱",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DivisionName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "行政区划名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DivisionId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                comment: "行政区划标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "行政区划编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassSerialNumber",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                maxLength: 128,
                nullable: true,
                comment: "班级序列号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "班级名称",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true,
                comment: "班级标识",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClassCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "班级编码",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "筛查时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChartScreeningStuffName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "筛查工作人员姓名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChartOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChartCheckedTimes",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: false,
                comment: "电子视力表筛查次数",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                comment: "生日",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                comment: "年龄",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼球镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RightEquivalentSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "integer",
                nullable: true,
                comment: "右眼等效球径度数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCylinder",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼柱镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r2(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r1(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r2(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r1(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率平均值(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率平均值(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率散光度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r2角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼角膜曲率r1角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼轴位",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RightAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼散光度数",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RecordId",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "uuid",
                nullable: false,
                comment: "档案标识",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<double>(
                name: "PupilDistance",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "瞳距",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OptometerScreeningStuffName",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "text",
                nullable: true,
                comment: "筛查工作人员姓名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OptometerOperationTime",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "TIMESTAMP",
                nullable: true,
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼球镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LeftEquivalentSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "integer",
                nullable: true,
                comment: "左眼等效球镜度数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCylinder",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼柱镜",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r2(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r1(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r2(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r1(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率平均值(d)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率平均值(mm)",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率散光度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r2角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼角膜曲率r1角度",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼轴位",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LeftAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼散光度数",
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ArtemisStudentRelationBinding",
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "用户标识"),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "学生标识"),
                    Relation = table.Column<string>(type: "text", nullable: false, comment: "关系")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisStudentRelationBinding", x => x.Id);
                },
                comment: "用户学生亲属关系绑定数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_CreateBy",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_CreatedAt",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_DeletedAt",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_ModifyBy",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_Partition",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_RemoveBy",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisStudentRelationBinding_UpdatedAt",
                schema: "Business",
                table: "ArtemisStudentRelationBinding",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisStudentRelationBinding",
                schema: "Business");

            migrationBuilder.AlterColumn<double>(
                name: "RightNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼裸眼视力");

            migrationBuilder.AlterColumn<string>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "右眼远视类型");

            migrationBuilder.AlterColumn<double>(
                name: "RightCorrectedVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼矫正视力");

            migrationBuilder.AlterColumn<double>(
                name: "RightChartDistance",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼与视力表的距离");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecordId",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "档案标识");

            migrationBuilder.AlterColumn<double>(
                name: "LeftNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼裸眼视力");

            migrationBuilder.AlterColumn<string>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "左眼远视类型");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCorrectedVision",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼矫正视力");

            migrationBuilder.AlterColumn<double>(
                name: "LeftChartDistance",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼与视力表的距离");

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareRightOkLenses",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "右眼是否佩戴角膜塑形镜");

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareLeftOkLenses",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "左眼是否佩戴角膜塑形镜");

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareGlasses",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否佩戴眼镜");

            migrationBuilder.AlterColumn<string>(
                name: "ChartScreeningStuffName",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "筛查工作人员姓名");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChartOperationTime",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "操作时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsChartChecked",
                schema: "Business",
                table: "ArtemisVisualChart",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "VisualStandardId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "应用的视力标准标识");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskUnitTargetId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "任务目标标识");

            migrationBuilder.AlterColumn<string>(
                name: "TaskUnitTargetCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务目标编码");

            migrationBuilder.AlterColumn<string>(
                name: "TaskUnitName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务单元名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskUnitId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "任务单元标识");

            migrationBuilder.AlterColumn<string>(
                name: "TaskUnitCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务单元编码");

            migrationBuilder.AlterColumn<string>(
                name: "TaskName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "任务标识");

            migrationBuilder.AlterColumn<string>(
                name: "TaskCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务编码");

            migrationBuilder.AlterColumn<string>(
                name: "TaskAgentType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务代理类型");

            migrationBuilder.AlterColumn<string>(
                name: "TaskAgentName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务代理名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskAgentId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "任务代理标识");

            migrationBuilder.AlterColumn<string>(
                name: "TaskAgentCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "任务代理编码");

            migrationBuilder.AlterColumn<string>(
                name: "StudyPhase",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "学段");

            migrationBuilder.AlterColumn<string>(
                name: "StudentName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "学生名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学生标识");

            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "学生编号");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "学校类型");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "学校名称");

            migrationBuilder.AlterColumn<int>(
                name: "SchoolLengthValue",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "学制值");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolLength",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "学制");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "学校标识");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "学校编码");

            migrationBuilder.AlterColumn<double>(
                name: "RightSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼球镜");

            migrationBuilder.AlterColumn<double>(
                name: "RightNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼裸眼视力");

            migrationBuilder.AlterColumn<string>(
                name: "RightEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "右眼远视类型");

            migrationBuilder.AlterColumn<int>(
                name: "RightEquivalentSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "右眼等效球径度数");

            migrationBuilder.AlterColumn<double>(
                name: "RightCylinder",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼柱镜");

            migrationBuilder.AlterColumn<double>(
                name: "RightCorrectedVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼矫正视力");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r2(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r1(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r2(d)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r1(d)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率平均值(d)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率平均值(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率散光度");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r2角度");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r1角度");

            migrationBuilder.AlterColumn<double>(
                name: "RightChartDistance",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼与视力表的距离");

            migrationBuilder.AlterColumn<double>(
                name: "RightAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼轴位");

            migrationBuilder.AlterColumn<double>(
                name: "RightAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼散光度数");

            migrationBuilder.AlterColumn<string>(
                name: "ReportSender",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "报告发送人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportSendTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "报告发送时间");

            migrationBuilder.AlterColumn<string>(
                name: "ReportReceiver",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "报告签收人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReportReceiveTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "报告签收时间");

            migrationBuilder.AlterColumn<double>(
                name: "PupilDistance",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "瞳距");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PrescribedTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "医嘱时间");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "组织机构名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "组织机构标识");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationDesignCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "组织机构设计编码");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "组织机构编码");

            migrationBuilder.AlterColumn<string>(
                name: "OptometerScreeningStuffName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "筛查工作人员姓名");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OptometerOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "操作时间");

            migrationBuilder.AlterColumn<int>(
                name: "OptometerCheckedTimes",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "验光仪筛查次数");

            migrationBuilder.AlterColumn<double>(
                name: "LeftSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼球镜");

            migrationBuilder.AlterColumn<double>(
                name: "LeftNakedEyeVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼裸眼视力");

            migrationBuilder.AlterColumn<string>(
                name: "LeftEyeHyperopiaType",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "左眼远视类型");

            migrationBuilder.AlterColumn<int>(
                name: "LeftEquivalentSphere",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "左眼等效球镜度数");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCylinder",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼柱镜");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCorrectedVision",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼矫正视力");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r2(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r1(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r2(d)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r1(d)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率平均值(d)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率平均值(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率散光度");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r2角度");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r1角度");

            migrationBuilder.AlterColumn<double>(
                name: "LeftChartDistance",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼与视力表的距离");

            migrationBuilder.AlterColumn<double>(
                name: "LeftAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼轴位");

            migrationBuilder.AlterColumn<double>(
                name: "LeftAstigmatism",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼散光度数");

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareRightOkLenses",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "右眼是否佩戴角膜塑形镜");

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareLeftOkLenses",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "左眼是否佩戴角膜塑形镜");

            migrationBuilder.AlterColumn<bool>(
                name: "IsWareGlasses",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否佩戴眼镜");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOptometerChecked",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否已经过验光仪筛查");

            migrationBuilder.AlterColumn<bool>(
                name: "IsChartChecked",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否已经过电子视力表筛查");

            migrationBuilder.AlterColumn<string>(
                name: "HeadTeacherName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "班主任名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "HeadTeacherId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "班主任标识");

            migrationBuilder.AlterColumn<string>(
                name: "GradeName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "年级名称");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "性别");

            migrationBuilder.AlterColumn<string>(
                name: "ExceptionReason",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "筛查结果");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "医师姓名");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorAdvice",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "医嘱");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "行政区划名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "DivisionId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "行政区划标识");

            migrationBuilder.AlterColumn<string>(
                name: "DivisionCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "行政区划编码");

            migrationBuilder.AlterColumn<int>(
                name: "ClassSerialNumber",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "班级序列号");

            migrationBuilder.AlterColumn<string>(
                name: "ClassName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "班级名称");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassId",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "班级标识");

            migrationBuilder.AlterColumn<string>(
                name: "ClassCode",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "班级编码");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "筛查时间");

            migrationBuilder.AlterColumn<string>(
                name: "ChartScreeningStuffName",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "筛查工作人员姓名");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChartOperationTime",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "操作时间");

            migrationBuilder.AlterColumn<int>(
                name: "ChartCheckedTimes",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "电子视力表筛查次数");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "生日");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "年龄");

            migrationBuilder.AlterColumn<double>(
                name: "RightSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼球镜");

            migrationBuilder.AlterColumn<int>(
                name: "RightEquivalentSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "右眼等效球径度数");

            migrationBuilder.AlterColumn<double>(
                name: "RightCylinder",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼柱镜");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r2(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r1(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r2(d)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r1(d)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率平均值(d)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率平均值(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率散光度");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r2角度");

            migrationBuilder.AlterColumn<double>(
                name: "RightCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼角膜曲率r1角度");

            migrationBuilder.AlterColumn<double>(
                name: "RightAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼轴位");

            migrationBuilder.AlterColumn<double>(
                name: "RightAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "右眼散光度数");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecordId",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "档案标识");

            migrationBuilder.AlterColumn<double>(
                name: "PupilDistance",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "瞳距");

            migrationBuilder.AlterColumn<string>(
                name: "OptometerScreeningStuffName",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "筛查工作人员姓名");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OptometerOperationTime",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "TIMESTAMP",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP",
                oldNullable: true,
                oldComment: "操作时间");

            migrationBuilder.AlterColumn<double>(
                name: "LeftSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼球镜");

            migrationBuilder.AlterColumn<int>(
                name: "LeftEquivalentSphere",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "左眼等效球镜度数");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCylinder",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼柱镜");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r2(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureR1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r1(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r2(d)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureD1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r1(d)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverageD",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率平均值(d)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAverage",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率平均值(mm)");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率散光度");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle2",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r2角度");

            migrationBuilder.AlterColumn<double>(
                name: "LeftCornealCurvatureAngle1",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼角膜曲率r1角度");

            migrationBuilder.AlterColumn<double>(
                name: "LeftAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼轴位");

            migrationBuilder.AlterColumn<double>(
                name: "LeftAstigmatism",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldComment: "左眼散光度数");
        }
    }
}
