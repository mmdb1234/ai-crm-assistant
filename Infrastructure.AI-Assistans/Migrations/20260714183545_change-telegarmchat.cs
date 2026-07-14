using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.AI_Assistans.Migrations
{
    /// <inheritdoc />
    public partial class changetelegarmchat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatConnections_Conversations_ActiveConversationId",
                table: "ChatConnections");

            migrationBuilder.DropIndex(
                name: "IX_ChatConnections_ActiveConversationId",
                table: "ChatConnections");

            migrationBuilder.DropIndex(
                name: "IX_ChatConnections_ExternalChatId_Platform",
                table: "ChatConnections");

            migrationBuilder.DropIndex(
                name: "IX_ChatConnections_WebhookToken",
                table: "ChatConnections");

            migrationBuilder.DropColumn(
                name: "ActiveConversationId",
                table: "ChatConnections");

            migrationBuilder.DropColumn(
                name: "ExternalChatId",
                table: "ChatConnections");

            migrationBuilder.DropColumn(
                name: "ExternalUsername",
                table: "ChatConnections");

            migrationBuilder.RenameColumn(
                name: "WebhookToken",
                table: "ChatConnections",
                newName: "BotUsername");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "ChatConnections",
                newName: "BusinessPhone");

            migrationBuilder.AddColumn<int>(
                name: "ExternalPlatform",
                table: "Conversations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalSenderId",
                table: "Conversations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BotToken",
                table: "ChatConnections",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberId",
                table: "ChatConnections",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalPlatform",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ExternalSenderId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "BotToken",
                table: "ChatConnections");

            migrationBuilder.DropColumn(
                name: "PhoneNumberId",
                table: "ChatConnections");

            migrationBuilder.RenameColumn(
                name: "BusinessPhone",
                table: "ChatConnections",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "BotUsername",
                table: "ChatConnections",
                newName: "WebhookToken");

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveConversationId",
                table: "ChatConnections",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalChatId",
                table: "ChatConnections",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExternalUsername",
                table: "ChatConnections",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

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
                name: "IX_ChatConnections_WebhookToken",
                table: "ChatConnections",
                column: "WebhookToken",
                unique: true,
                filter: "[WebhookToken] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatConnections_Conversations_ActiveConversationId",
                table: "ChatConnections",
                column: "ActiveConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
