using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtemisNotificationMessage",
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
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false, comment: "任务标识"),
                    EndType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "端类型"),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, comment: "是否已读"),
                    ReadTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "消息读取时间"),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "通知标题"),
                    Content = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "通知内容"),
                    BindingTag = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "绑定标记")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisNotificationMessage", x => x.Id);
                },
                comment: "通知消息数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_CreateBy",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_CreatedAt",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_DeletedAt",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_ModifyBy",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_Partition",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_RemoveBy",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisNotificationMessage_UpdatedAt",
                schema: "Business",
                table: "ArtemisNotificationMessage",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisNotificationMessage",
                schema: "Business");
        }
    }
}
