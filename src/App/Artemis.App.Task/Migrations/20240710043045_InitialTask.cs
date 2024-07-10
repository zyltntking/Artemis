using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Task.Migrations
{
    /// <inheritdoc />
    public partial class InitialTask : Migration
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
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
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
                    CreateBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<Guid>(type: "UUID", nullable: false, comment: "删除者标识"),
                    Partition = table.Column<int>(type: "integer", nullable: false, comment: "分区标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    TaskName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "任务名称"),
                    TaskStatus = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务状态"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "任务描述"),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "任务开始时间"),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "任务结束时间"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "父任务标识")
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
                name: "ArtemisTaskUnit",
                schema: "Task",
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
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识"),
                    UnitName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, comment: "任务单元名称"),
                    TaskStatus = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务状态"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "任务描述"),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "任务开始时间"),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "任务结束时间")
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
                name: "ArtemisTaskAgent",
                schema: "Task",
                columns: table => new
                {
                    TaskUnitId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务单元标识"),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "代理标识"),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTaskAgent", x => new { x.TaskUnitId, x.AgentId });
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
                    table.ForeignKey(
                        name: "FK_ArtemisTaskAgent_ArtemisTaskUnit",
                        column: x => x.TaskUnitId,
                        principalSchema: "Task",
                        principalTable: "ArtemisTaskUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务代理配置");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskTarget",
                schema: "Task",
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
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识"),
                    TaskUnitId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务单元标识"),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false, comment: "目标标识"),
                    TaskStatus = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "任务状态"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "任务描述"),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "任务开始时间"),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "任务结束时间")
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
                name: "IX_ArtemisTaskAgent_TaskId",
                schema: "Task",
                table: "ArtemisTaskAgent",
                column: "TaskId");

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
                name: "IX_ArtemisTaskUnit_TaskId",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_UpdatedAt",
                schema: "Task",
                table: "ArtemisTaskUnit",
                column: "UpdatedAt");
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
