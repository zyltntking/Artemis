using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AddNation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nation",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "民族");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nation",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");
        }
    }
}
