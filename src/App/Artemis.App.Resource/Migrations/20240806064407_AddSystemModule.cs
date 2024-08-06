using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Resource.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtemisSystemModule",
                schema: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间"),
                    CreateBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "创建者标识"),
                    ModifyBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "更新者标识"),
                    RemoveBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "删除者标识"),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁"),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "模块类型"),
                    Order = table.Column<int>(type: "integer", nullable: false, comment: "模块序列"),
                    Path = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "路由地址"),
                    Parameters = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "路由参数"),
                    Component = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "组件路径"),
                    Icon = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "模块图标"),
                    IsFrame = table.Column<bool>(type: "boolean", nullable: false, comment: "是否外链"),
                    Visible = table.Column<bool>(type: "boolean", nullable: false, comment: "是否显示"),
                    Status = table.Column<bool>(type: "boolean", nullable: false, comment: "模块状态"),
                    ClaimStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "凭据戳"),
                    Remark = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "备注"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true, comment: "上级模块标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisSystemModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisSystemModule_ArtemisSystemModule",
                        column: x => x.ParentId,
                        principalSchema: "Resource",
                        principalTable: "ArtemisSystemModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "系统模块数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_CreateBy",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_CreatedAt",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_DeletedAt",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_ModifyBy",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_Name",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_ParentId",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_RemoveBy",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisSystemModule_UpdatedAt",
                schema: "Resource",
                table: "ArtemisSystemModule",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisSystemModule",
                schema: "Resource");
        }
    }
}
