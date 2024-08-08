using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordFeedBack",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeedBack",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否反馈");

            migrationBuilder.CreateTable(
                name: "ArtemisRecordFeedback",
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "反馈用户标识"),
                    IsCheck = table.Column<bool>(type: "boolean", nullable: false, comment: "是否已处理"),
                    CheckDate = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "处理日期"),
                    Content = table.Column<string>(type: "text", nullable: true, comment: "反馈内容"),
                    FeedBackTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "反馈时间"),
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false, comment: "记录标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisRecordFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisRecordFeedback_ArtemisVisionScreenRecord",
                        column: x => x.RecordId,
                        principalSchema: "Business",
                        principalTable: "ArtemisVisionScreenRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "记录反馈数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_CreateBy",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_CreatedAt",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_DeletedAt",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_ModifyBy",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_Partition",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_RecordId",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_RemoveBy",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisRecordFeedback_UpdatedAt",
                schema: "Business",
                table: "ArtemisRecordFeedback",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisRecordFeedback",
                schema: "Business");

            migrationBuilder.DropColumn(
                name: "IsFeedBack",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.AddColumn<string>(
                name: "RecordFeedBack",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "筛查报告反馈");
        }
    }
}
