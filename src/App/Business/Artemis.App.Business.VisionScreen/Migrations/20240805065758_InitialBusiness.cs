using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class InitialBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Business");

            migrationBuilder.CreateTable(
                name: "ArtemisVisionScreenRecord",
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
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: true),
                    TaskCode = table.Column<string>(type: "text", nullable: true),
                    TaskUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskUnitName = table.Column<string>(type: "text", nullable: true),
                    TaskUnitCode = table.Column<string>(type: "text", nullable: true),
                    TaskUnitTargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskUnitTargetCode = table.Column<string>(type: "text", nullable: true),
                    TaskAgentId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskAgentName = table.Column<string>(type: "text", nullable: true),
                    TaskAgentCode = table.Column<string>(type: "text", nullable: true),
                    TaskAgentType = table.Column<string>(type: "text", nullable: true),
                    DoctorName = table.Column<string>(type: "text", nullable: true),
                    VisualStandardId = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorAdvice = table.Column<string>(type: "text", nullable: true),
                    PrescribedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolName = table.Column<string>(type: "text", nullable: false),
                    SchoolCode = table.Column<string>(type: "text", nullable: true),
                    SchoolType = table.Column<string>(type: "text", nullable: true),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionName = table.Column<string>(type: "text", nullable: true),
                    DivisionCode = table.Column<string>(type: "text", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationName = table.Column<string>(type: "text", nullable: false),
                    OrganizationCode = table.Column<string>(type: "text", nullable: true),
                    OrganizationDesignCode = table.Column<string>(type: "text", nullable: false),
                    ClassId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClassName = table.Column<string>(type: "text", nullable: true),
                    ClassCode = table.Column<string>(type: "text", nullable: true),
                    GradeName = table.Column<string>(type: "text", nullable: true),
                    ClassSerialNumber = table.Column<int>(type: "integer", nullable: true),
                    StudyPhase = table.Column<string>(type: "text", nullable: true),
                    SchoolLength = table.Column<string>(type: "text", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentName = table.Column<string>(type: "text", nullable: true),
                    StudentCode = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    LeftChartDistance = table.Column<double>(type: "double precision", nullable: true),
                    RightChartDistance = table.Column<double>(type: "double precision", nullable: true),
                    LeftNakedEyeVision = table.Column<double>(type: "double precision", nullable: true),
                    RightNakedEyeVision = table.Column<double>(type: "double precision", nullable: true),
                    IsWareGlasses = table.Column<bool>(type: "boolean", nullable: false),
                    LeftCorrectedVision = table.Column<double>(type: "double precision", nullable: true),
                    RightCorrectedVision = table.Column<double>(type: "double precision", nullable: true),
                    LeftEyeHyperopiaType = table.Column<double>(type: "double precision", nullable: true),
                    RightEyeHyperopiaType = table.Column<double>(type: "double precision", nullable: true),
                    IsWareLeftOkLenses = table.Column<bool>(type: "boolean", nullable: true),
                    IsWareRightOkLenses = table.Column<bool>(type: "boolean", nullable: true),
                    ChartScreeningStuffName = table.Column<string>(type: "text", nullable: true),
                    ChartOperationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsChartChecked = table.Column<bool>(type: "boolean", nullable: false),
                    ChartCheckedTimes = table.Column<int>(type: "integer", nullable: false),
                    PupilDistance = table.Column<double>(type: "double precision", nullable: true),
                    LeftSphere = table.Column<double>(type: "double precision", nullable: true),
                    LeftEquivalentSphere = table.Column<int>(type: "integer", nullable: true),
                    RightSphere = table.Column<double>(type: "double precision", nullable: true),
                    RightEquivalentSphere = table.Column<int>(type: "integer", nullable: true),
                    LeftCylinder = table.Column<double>(type: "double precision", nullable: true),
                    RightCylinder = table.Column<double>(type: "double precision", nullable: true),
                    LeftAxis = table.Column<double>(type: "double precision", nullable: true),
                    RightAxis = table.Column<double>(type: "double precision", nullable: true),
                    LeftAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    RightAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureR1 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureR1 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureD1 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureD1 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAngle1 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAngle1 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureR2 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureR2 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureD2 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureD2 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAngle2 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAngle2 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAverage = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAverage = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAverageD = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAverageD = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    OptometerScreeningStuffName = table.Column<string>(type: "text", nullable: true),
                    OptometerOperationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsOptometerChecked = table.Column<bool>(type: "boolean", nullable: false),
                    OptometerCheckedTimes = table.Column<int>(type: "integer", nullable: false),
                    CheckTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExceptionReason = table.Column<string>(type: "text", nullable: true),
                    ReportSender = table.Column<string>(type: "text", nullable: true),
                    ReportSendTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportReceiver = table.Column<string>(type: "text", nullable: true),
                    ReportReceiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisVisionScreenRecord", x => x.Id);
                },
                comment: "视力档案数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisOptometer",
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
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    PupilDistance = table.Column<double>(type: "double precision", nullable: true),
                    LeftSphere = table.Column<double>(type: "double precision", nullable: true),
                    LeftEquivalentSphere = table.Column<int>(type: "integer", nullable: true),
                    RightSphere = table.Column<double>(type: "double precision", nullable: true),
                    RightEquivalentSphere = table.Column<int>(type: "integer", nullable: true),
                    LeftCylinder = table.Column<double>(type: "double precision", nullable: true),
                    RightCylinder = table.Column<double>(type: "double precision", nullable: true),
                    LeftAxis = table.Column<double>(type: "double precision", nullable: true),
                    RightAxis = table.Column<double>(type: "double precision", nullable: true),
                    LeftAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    RightAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureR1 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureR1 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureD1 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureD1 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAngle1 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAngle1 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureR2 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureR2 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureD2 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureD2 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAngle2 = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAngle2 = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAverage = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAverage = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAverageD = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAverageD = table.Column<double>(type: "double precision", nullable: true),
                    LeftCornealCurvatureAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    RightCornealCurvatureAstigmatism = table.Column<double>(type: "double precision", nullable: true),
                    OptometerScreeningStuffName = table.Column<string>(type: "text", nullable: true),
                    OptometerOperationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisOptometer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisOptometer_ArtemisVisionScreenRecord",
                        column: x => x.RecordId,
                        principalSchema: "Business",
                        principalTable: "ArtemisVisionScreenRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "验光仪数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisVisualChart",
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
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeftChartDistance = table.Column<double>(type: "double precision", nullable: true),
                    RightChartDistance = table.Column<double>(type: "double precision", nullable: true),
                    LeftNakedEyeVision = table.Column<double>(type: "double precision", nullable: true),
                    RightNakedEyeVision = table.Column<double>(type: "double precision", nullable: true),
                    IsWareGlasses = table.Column<bool>(type: "boolean", nullable: false),
                    LeftCorrectedVision = table.Column<double>(type: "double precision", nullable: true),
                    RightCorrectedVision = table.Column<double>(type: "double precision", nullable: true),
                    LeftEyeHyperopiaType = table.Column<double>(type: "double precision", nullable: true),
                    RightEyeHyperopiaType = table.Column<double>(type: "double precision", nullable: true),
                    IsWareLeftOkLenses = table.Column<bool>(type: "boolean", nullable: true),
                    IsWareRightOkLenses = table.Column<bool>(type: "boolean", nullable: true),
                    ChartScreeningStuffName = table.Column<string>(type: "text", nullable: true),
                    ChartOperationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsChartChecked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisVisualChart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisVisualChart_ArtemisVisionScreenRecord",
                        column: x => x.RecordId,
                        principalSchema: "Business",
                        principalTable: "ArtemisVisionScreenRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "视力表数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_CreateBy",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_CreatedAt",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_DeletedAt",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_ModifyBy",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_Partition",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_RecordId",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_RemoveBy",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_UpdatedAt",
                schema: "Business",
                table: "ArtemisOptometer",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_CreateBy",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_CreatedAt",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_DeletedAt",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_ModifyBy",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_Partition",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_RemoveBy",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisionScreenRecord_UpdatedAt",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_CreateBy",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_CreatedAt",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_DeletedAt",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_ModifyBy",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_Partition",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_RecordId",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_RemoveBy",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_UpdatedAt",
                schema: "Business",
                table: "ArtemisVisualChart",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisOptometer",
                schema: "Business");

            migrationBuilder.DropTable(
                name: "ArtemisVisualChart",
                schema: "Business");

            migrationBuilder.DropTable(
                name: "ArtemisVisionScreenRecord",
                schema: "Business");
        }
    }
}
