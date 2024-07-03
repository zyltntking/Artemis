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
                name: "task");

            migrationBuilder.CreateTable(
                name: "ArtemisAgent",
                schema: "task",
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
                schema: "task",
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
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "任务结束时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTask", x => x.Id);
                },
                comment: "任务数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskUnit",
                schema: "task",
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
                        principalSchema: "task",
                        principalTable: "ArtemisTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务单元数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskAgent",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifyBy = table.Column<Guid>(type: "uuid", nullable: false),
                    RemoveBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Partition = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识"),
                    TaskUnitId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务单元标识"),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false, comment: "代理标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisTaskAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskAgent_ArtemisAgent",
                        column: x => x.AgentId,
                        principalSchema: "task",
                        principalTable: "ArtemisAgent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskAgent_ArtemisTask",
                        column: x => x.TaskId,
                        principalSchema: "task",
                        principalTable: "ArtemisTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskAgent_ArtemisTaskUnit",
                        column: x => x.TaskUnitId,
                        principalSchema: "task",
                        principalTable: "ArtemisTaskUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务代理配置");

            migrationBuilder.CreateTable(
                name: "ArtemisTaskTarget",
                schema: "task",
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
                        name: "FK_ArtemisTaskTarget_ArtemisTask",
                        column: x => x.TaskId,
                        principalSchema: "task",
                        principalTable: "ArtemisTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisTaskTarget_ArtemisTaskUnit",
                        column: x => x.TaskUnitId,
                        principalSchema: "task",
                        principalTable: "ArtemisTaskUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "任务目标数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_CreateBy",
                schema: "task",
                table: "ArtemisAgent",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_CreatedAt",
                schema: "task",
                table: "ArtemisAgent",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_DeletedAt",
                schema: "task",
                table: "ArtemisAgent",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_ModifyBy",
                schema: "task",
                table: "ArtemisAgent",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_Partition",
                schema: "task",
                table: "ArtemisAgent",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_RemoveBy",
                schema: "task",
                table: "ArtemisAgent",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisAgent_UpdatedAt",
                schema: "task",
                table: "ArtemisAgent",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_CreateBy",
                schema: "task",
                table: "ArtemisTask",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_CreatedAt",
                schema: "task",
                table: "ArtemisTask",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_DeletedAt",
                schema: "task",
                table: "ArtemisTask",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_ModifyBy",
                schema: "task",
                table: "ArtemisTask",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_Partition",
                schema: "task",
                table: "ArtemisTask",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_RemoveBy",
                schema: "task",
                table: "ArtemisTask",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTask_UpdatedAt",
                schema: "task",
                table: "ArtemisTask",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskAgent_AgentId",
                schema: "task",
                table: "ArtemisTaskAgent",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskAgent_TaskId",
                schema: "task",
                table: "ArtemisTaskAgent",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskAgent_TaskUnitId",
                schema: "task",
                table: "ArtemisTaskAgent",
                column: "TaskUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_CreateBy",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_CreatedAt",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_DeletedAt",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_ModifyBy",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_Partition",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_RemoveBy",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_TaskId",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_TaskUnitId",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "TaskUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskTarget_UpdatedAt",
                schema: "task",
                table: "ArtemisTaskTarget",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_CreateBy",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_CreatedAt",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_DeletedAt",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_ModifyBy",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_Partition",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_RemoveBy",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_TaskId",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisTaskUnit_UpdatedAt",
                schema: "task",
                table: "ArtemisTaskUnit",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisTaskAgent",
                schema: "task");

            migrationBuilder.DropTable(
                name: "ArtemisTaskTarget",
                schema: "task");

            migrationBuilder.DropTable(
                name: "ArtemisAgent",
                schema: "task");

            migrationBuilder.DropTable(
                name: "ArtemisTaskUnit",
                schema: "task");

            migrationBuilder.DropTable(
                name: "ArtemisTask",
                schema: "task");
        }
    }
}
