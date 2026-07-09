using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.AI_Assistans.Migrations
{
    /// <inheritdoc />
    public partial class AddtelegramAndWhatsup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalMessageId",
                table: "Messages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourcePlatform",
                table: "Messages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatConnections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Platform = table.Column<int>(type: "integer", nullable: false),
                    ExternalChatId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExternalUsername = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    WebhookToken = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActiveConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatConnections_Conversations_ActiveConversationId",
                        column: x => x.ActiveConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ChatConnections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatConnections_ActiveConversationId",
                table: "ChatConnections",
                column: "ActiveConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConnections_ExternalChatId_Platform",
                table: "ChatConnections",
                columns: new[] { "ExternalChatId", "Platform" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatConnections_UserId",
                table: "ChatConnections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConnections_WebhookToken",
                table: "ChatConnections",
                column: "WebhookToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatConnections");

            migrationBuilder.DropColumn(
                name: "ExternalMessageId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SourcePlatform",
                table: "Messages");
        }
    }
}
