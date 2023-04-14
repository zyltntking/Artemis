using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.MonitorService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetadataGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "数据键"),
                    Value = table.Column<string>(type: "text", nullable: false, comment: "数据值"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效"),
                    Partition = table.Column<int>(type: "INTEGER", nullable: false, comment: "分区标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetadataItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    MetadataGroupId = table.Column<Guid>(type: "uuid", nullable: false, comment: "元数据组标识"),
                    Key = table.Column<string>(type: "text", nullable: false, comment: "数据键"),
                    Value = table.Column<string>(type: "text", nullable: false, comment: "数据值"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效"),
                    Partition = table.Column<int>(type: "INTEGER", nullable: false, comment: "分区标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetadataItems_MetadataGroups_MetadataGroupId",
                        column: x => x.MetadataGroupId,
                        principalTable: "MetadataGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorHosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, comment: "标识"),
                    MetadataGroupId = table.Column<Guid>(type: "uuid", nullable: true, comment: "元数据组标识"),
                    HostName = table.Column<string>(type: "text", nullable: false, comment: "主机名"),
                    HostType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "主机类型"),
                    OsName = table.Column<string>(type: "text", nullable: false, comment: "系统名"),
                    OsVersion = table.Column<string>(type: "text", nullable: false, comment: "系统版本"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "创建时间,初始化后不再进行任何变更"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, comment: "更新时间,初始为创建时间"),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true, comment: "删除时间,启用软删除时生效"),
                    Partition = table.Column<int>(type: "INTEGER", nullable: false, comment: "分区标识")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorHosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorHosts_MetadataGroups_MetadataGroupId",
                        column: x => x.MetadataGroupId,
                        principalTable: "MetadataGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetadataItems_MetadataGroupId",
                table: "MetadataItems",
                column: "MetadataGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorHosts_MetadataGroupId",
                table: "MonitorHosts",
                column: "MetadataGroupId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetadataItems");

            migrationBuilder.DropTable(
                name: "MonitorHosts");

            migrationBuilder.DropTable(
                name: "MetadataGroups");
        }
    }
}
