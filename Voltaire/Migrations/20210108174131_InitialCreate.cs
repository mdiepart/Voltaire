using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Voltaire.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiscordId = table.Column<string>(nullable: true),
                    AllowedRole = table.Column<string>(nullable: true),
                    AdminRole = table.Column<string>(nullable: true),
                    AllowDirectMessage = table.Column<bool>(nullable: false),
                    UseUserIdentifiers = table.Column<bool>(nullable: false),
                    UseEmbed = table.Column<bool>(nullable: false),
                    UserIdentifierSeed = table.Column<int>(nullable: false),
                    SubscriptionId = table.Column<string>(nullable: true),
                    MessagesSentThisMonth = table.Column<int>(nullable: false),
                    TrackingMonth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BannedIdentifiers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<string>(nullable: true),
                    GuildID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedIdentifiers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BannedIdentifiers_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannedIdentifiers_GuildID",
                table: "BannedIdentifiers",
                column: "GuildID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedIdentifiers");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
