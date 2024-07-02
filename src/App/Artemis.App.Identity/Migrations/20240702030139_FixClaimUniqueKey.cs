using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artemis.App.Identity.Migrations
{
    /// <inheritdoc />
    public partial class FixClaimUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUserClaim_CheckStamp",
                schema: "identity",
                table: "IdentityUserClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserClaim_UserId",
                schema: "identity",
                table: "IdentityUserClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityRoleClaim_CheckStamp",
                schema: "identity",
                table: "IdentityRoleClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityRoleClaim_RoleId",
                schema: "identity",
                table: "IdentityRoleClaim");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_UserId_CheckStamp",
                schema: "identity",
                table: "IdentityUserClaim",
                columns: new[] { "UserId", "CheckStamp" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_RoleId_CheckStamp",
                schema: "identity",
                table: "IdentityRoleClaim",
                columns: new[] { "RoleId", "CheckStamp" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUserClaim_UserId_CheckStamp",
                schema: "identity",
                table: "IdentityUserClaim");

            migrationBuilder.DropIndex(
                name: "IX_IdentityRoleClaim_RoleId_CheckStamp",
                schema: "identity",
                table: "IdentityRoleClaim");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_CheckStamp",
                schema: "identity",
                table: "IdentityUserClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_UserId",
                schema: "identity",
                table: "IdentityUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_CheckStamp",
                schema: "identity",
                table: "IdentityRoleClaim",
                column: "CheckStamp",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRoleClaim_RoleId",
                schema: "identity",
                table: "IdentityRoleClaim",
                column: "RoleId");
        }
    }
}
