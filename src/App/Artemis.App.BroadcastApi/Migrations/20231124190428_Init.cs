using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.BroadcastApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    License = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, comment: "车牌号"),
                    Count = table.Column<int>(type: "integer", nullable: false, comment: "用餐人数"),
                    Price = table.Column<double>(type: "double precision", nullable: false, comment: "餐价"),
                    MealTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "用餐时间"),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "状态"),
                    Remark = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "备注"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                },
                comment: "订单数据集");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    UserName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "用户名"),
                    NormalizedUserName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "规范化用户名"),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "凭据值"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                },
                comment: "用户数据集");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedAt",
                table: "Order",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeletedAt",
                table: "Order",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MealTime",
                table: "Order",
                column: "MealTime");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status",
                table: "Order",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UpdatedAt",
                table: "Order",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedAt",
                table: "User",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedAt",
                table: "User",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedAt",
                table: "User",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "NormalizedUserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
