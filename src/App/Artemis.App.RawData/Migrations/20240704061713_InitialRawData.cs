using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.RawData.Migrations
{
    /// <inheritdoc />
    public partial class InitialRawData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RawData");

            migrationBuilder.CreateTable(
                name: "ArtemisOptometer",
                schema: "RawData",
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
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisOptometer", x => x.Id);
                },
                comment: "验光仪数据集");

            migrationBuilder.CreateTable(
                name: "ArtemisVisualChart",
                schema: "RawData",
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
                    ConcurrencyStamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "并发锁")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisVisualChart", x => x.Id);
                },
                comment: "视力表数据集");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_CreateBy",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_CreatedAt",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_DeletedAt",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_ModifyBy",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_Partition",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_RemoveBy",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisOptometer_UpdatedAt",
                schema: "RawData",
                table: "ArtemisOptometer",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_CreateBy",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_CreatedAt",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_DeletedAt",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_ModifyBy",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "ModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_Partition",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "Partition");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_RemoveBy",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "RemoveBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisVisualChart_UpdatedAt",
                schema: "RawData",
                table: "ArtemisVisualChart",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisOptometer",
                schema: "RawData");

            migrationBuilder.DropTable(
                name: "ArtemisVisualChart",
                schema: "RawData");
        }
    }
}
