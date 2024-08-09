using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Business.VisionScreen.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelFlag",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "text",
                nullable: true,
                comment: "取消筛查标识");

            migrationBuilder.AddColumn<double>(
                name: "LeftHorizontalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼水平方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "LeftPupilRadius",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼瞳孔半径");

            migrationBuilder.AddColumn<double>(
                name: "LeftRedReflect",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼红光反射");

            migrationBuilder.AddColumn<double>(
                name: "LeftVerticalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "左眼垂直方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "RightHorizontalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼水平方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "RightPupilRadius",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼瞳孔半径");

            migrationBuilder.AddColumn<double>(
                name: "RightRedReflect",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼红光反射");

            migrationBuilder.AddColumn<double>(
                name: "RightVerticalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord",
                type: "double precision",
                nullable: true,
                comment: "右眼垂直方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "LeftHorizontalAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼水平方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "LeftPupilRadius",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼瞳孔半径");

            migrationBuilder.AddColumn<double>(
                name: "LeftRedReflect",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼红光反射");

            migrationBuilder.AddColumn<double>(
                name: "LeftVerticalAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "左眼垂直方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "RightHorizontalAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼水平方向斜视度数");

            migrationBuilder.AddColumn<double>(
                name: "RightPupilRadius",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼瞳孔半径");

            migrationBuilder.AddColumn<double>(
                name: "RightRedReflect",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼红光反射");

            migrationBuilder.AddColumn<double>(
                name: "RightVerticalAxis",
                schema: "Business",
                table: "ArtemisOptometer",
                type: "double precision",
                nullable: true,
                comment: "右眼垂直方向斜视度数");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelFlag",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "LeftHorizontalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "LeftPupilRadius",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "LeftRedReflect",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "LeftVerticalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "RightHorizontalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "RightPupilRadius",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "RightRedReflect",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "RightVerticalAxis",
                schema: "Business",
                table: "ArtemisVisionScreenRecord");

            migrationBuilder.DropColumn(
                name: "LeftHorizontalAxis",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "LeftPupilRadius",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "LeftRedReflect",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "LeftVerticalAxis",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "RightHorizontalAxis",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "RightPupilRadius",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "RightRedReflect",
                schema: "Business",
                table: "ArtemisOptometer");

            migrationBuilder.DropColumn(
                name: "RightVerticalAxis",
                schema: "Business",
                table: "ArtemisOptometer");
        }
    }
}
