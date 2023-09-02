using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Artemis.App.IdentityApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialArtemisIdentityContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityRole",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityUser",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityRoleClaim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisIdentityRoleClaim_ArtemisIdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "ArtemisIdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityUserClaim",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtemisIdentityUserClaim_ArtemisIdentityUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisIdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityUserLogin",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ArtemisIdentityUserLogin_ArtemisIdentityUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisIdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityUserRole",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ArtemisIdentityUserRole_ArtemisIdentityRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "ArtemisIdentityRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtemisIdentityUserRole_ArtemisIdentityUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisIdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtemisIdentityUserToken",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtemisIdentityUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_ArtemisIdentityUserToken_ArtemisIdentityUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "ArtemisIdentityUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "ArtemisIdentityRole",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisIdentityRoleClaim_RoleId",
                schema: "identity",
                table: "ArtemisIdentityRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "ArtemisIdentityUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "ArtemisIdentityUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisIdentityUserClaim_UserId",
                schema: "identity",
                table: "ArtemisIdentityUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisIdentityUserLogin_UserId",
                schema: "identity",
                table: "ArtemisIdentityUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtemisIdentityUserRole_RoleId",
                schema: "identity",
                table: "ArtemisIdentityUserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtemisIdentityRoleClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisIdentityUserClaim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisIdentityUserLogin",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisIdentityUserRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisIdentityUserToken",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisIdentityRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "ArtemisIdentityUser",
                schema: "identity");
        }
    }
}
