using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Task.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Task");

            migrationBuilder.CreateTable(
                name: "ArtemisAgent",
                schema: "Task",
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
                    AgentName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "代理名称"),
                    AgentType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "代理类型"),
                    AgentCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "代理编码")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisAgent", x => x.Id);
                },
                comment: "代理数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTask",
                schema: "Task",
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
                    TaskName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务名称"),
                    TaskCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "任务编码"),
                    DesignCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "设计编码"),
                    NormalizedTaskName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务名称"),
                    TaskShip = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "任务归属"),
                    TaskMode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "任务模式"),
                    TaskState = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "任务状态"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "任务描述"),
                    StartTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "任务开始时间"),
                    EndTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "任务结束时间"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "父任务标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisTask_ArtemisTask",
                        column: x => x.ParentId,
                        principalSchema: "Task",
                        principalTable: "ArtemisTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskAgent",
                schema: "Task",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识"),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "代理标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTaskAgent", x => new { x.TaskId, x.AgentId });
                    table.ForeignKey(
                        name: "FK_ArtemisTaskAgent_ArtemisAgent",
                        column: x => x.AgentId,
                        principalSchema: "Task",
                        principalTable: "ArtemisAgent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskAgent_ArtemisTask",
                        column: x => x.TaskId,
                        principalSchema: "Task",
                        principalTable: "ArtemisTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务代理配置");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskUnit",
                schema: "Task",
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
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识"),
                    UnitName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务单元名称"),
                    NormalizedUnitName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "标准化单元名"),
                    UnitCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "单元编码"),
                    DesignCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "设计编码"),
                    TaskUnitState = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "任务状态"),
                    TaskUnitMode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "任务模式"),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "任务描述"),
                    StartTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "任务开始时间"),
                    EndTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "任务结束时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTaskUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskUnit_ArtemisTask",
                        column: x => x.TaskId,
                        principalSchema: "Task",
                        principalTable: "ArtemisTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务单元数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskTarget",
                schema: "Task",
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
                    TaskUnitId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务单元标识"),
                    TargetName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务目标名称"),
                    TargetCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "任务目标编码"),
                    DesignCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "设计编码"),
                    TargetType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "任务目标类型"),
                    TargetId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "任务目标外部标识"),
                    TargetState = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "任务描述"),
                    ExecuteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TaskStatus = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务状态")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTaskTarget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskTarget_ArtemisTaskUnit",
                        column: x => x.TaskUnitId,
                        principalSchema: "Task",
                        principalTable: "ArtemisTaskUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务目标数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskUnitAgent",
                schema: "Task",
                columns: table => new
                {
                    TaskUnitId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务单元标识"),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "代理标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTaskUnitAgent", x => new { x.TaskUnitId, x.AgentId });
                    table.ForeignKey(
                        name: "FK_ArtemisTaskUnitAgent_ArtemisAgent",
                        column: x => x.AgentId,
                        principalSchema: "Task",
                        principalTable: "ArtemisAgent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskUnitAgent_ArtemisTaskUnit",
                        column: x => x.TaskUnitId,
                        principalSchema: "Task",
                        principalTable: "ArtemisTaskUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务单元代理配置");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_CreateBy",
                schema: "Task",
                table: "ArtemisAgent",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_CreatedAt",
                schema: "Task",
                table: "ArtemisAgent",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_DeletedAt",
                schema: "Task",
                table: "ArtemisAgent",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_ModifyBy",
                schema: "Task",
                table: "ArtemisAgent",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_Partition",
                schema: "Task",
                table: "ArtemisAgent",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_RemoveBy",
                schema: "Task",
                table: "ArtemisAgent",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_UpdatedAt",
                schema: "Task",
                table: "ArtemisAgent",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_CreateBy",
                schema: "Task",
                table: "ArtemisTask",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_CreatedAt",
                schema: "Task",
                table: "ArtemisTask",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_DeletedAt",
                schema: "Task",
                table: "ArtemisTask",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_DesignCode",
                schema: "Task",
                table: "ArtemisTask",
                column: "DesignCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_EndTime",
                schema: "Task",
                table: "ArtemisTask",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_ModifyBy",
                schema: "Task",
                table: "ArtemisTask",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_ParentId",
                schema: "Task",
                table: "ArtemisTask",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_Partition",
                schema: "Task",
                table: "ArtemisTask",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_RemoveBy",
                schema: "Task",
                table: "ArtemisTask",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_StartTime",
                schema: "Task",
                table: "ArtemisTask",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_TaskCode",
                schema: "Task",
                table: "ArtemisTask",
                column: "TaskCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_TaskMode",
                schema: "Task",
                table: "ArtemisTask",
                column: "TaskMode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_TaskName",
                schema: "Task",
                table: "ArtemisTask",
                column: "NormalizedTaskName");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_TaskShip",
                schema: "Task",
                table: "ArtemisTask",
                column: "TaskShip");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_TaskState",
                schema: "Task",
                table: "ArtemisTask",
                column: "TaskState");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_UpdatedAt",
                schema: "Task",
                table: "ArtemisTask",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskAgent_AgentId",
                schema: "Task",
                table: "ArtemisTaskAgent",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_CreateBy",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_CreatedAt",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_DeletedAt",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_ModifyBy",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_Partition",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_RemoveBy",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_TaskUnitId",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "TaskUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_UpdatedAt",
                schema: "Task",
                table: "ArtemisTaskTarget",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_CreateBy",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_CreatedAt",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_DeletedAt",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_DesignCode",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "DesignCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_EndTime",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_ModifyBy",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_Partition",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_RemoveBy",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_StartTime",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_TaskId",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_TaskUnitMode",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "TaskUnitMode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_TaskUnitState",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "TaskUnitState");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_UnitCode",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "UnitCode");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_UnitName",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "NormalizedUnitName");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_UpdatedAt",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnitAgent_AgentId",
                schema: "Task",
                table: "ArtemisTaskUnitAgent",
                column: "AgentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisTaskAgent",
                schema: "Task");

            migrationBuilder.DropTable(
                name: "ArtemisTaskTarget",
                schema: "Task");

            migrationBuilder.DropTable(
                name: "ArtemisTaskUnitAgent",
                schema: "Task");

            migrationBuilder.DropTable(
                name: "ArtemisAgent",
                schema: "Task");

            migrationBuilder.DropTable(
                name: "ArtemisTaskUnit",
                schema: "Task");

            migrationBuilder.DropTable(
                name: "ArtemisTask",
                schema: "Task");
        }
    }
}
