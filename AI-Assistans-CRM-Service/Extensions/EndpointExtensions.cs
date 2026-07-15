using Features.AI_Assistans.Conversations.AnalyzeConversation;
using Features.AI_Assistans.Conversations.CreateConversation;
using Features.AI_Assistans.Conversations.GetConversation;
using Features.AI_Assistans.Conversations.GetConversatiosbyUserID;
using Features.AI_Assistans.Conversations.GetLatestConversationAnalysis;
using Features.AI_Assistans.Messages.GetConversationMessages;
using Features.AI_Assistans.Users.CreateUser;
using Features.AI_Assistans.Users.GetUsers;
using AI_Assistans_CRM_Service.Companies.GetCompanyConversations;
using AI_Assistans_CRM_Service.Companies.RegisterCompany;
using AI_Assistans_CRM_Service.Conversations.LoginCompany;
using AI_Assistans_CRM_Service.Conversations.RefreshToken;
using AI_Assistans_CRM_Service.Users.ConnectTelegramBot;
using AI_Assistans_CRM_Service.Webhooks.Telegram;
using AI_Assistans_CRM_Service.Webhooks.WhatsApp;

namespace AI_Assistans_CRM_Service.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapConversationEndpoints(
            this IEndpointRouteBuilder app)
        {
            app.MapCreateConversationEndpoint();
            app.MapGetConversationEndpoint();
            app.MapGetLatestConversationAnalysisEndpoint();
            app.MapCreateAnalyzeConversationEndpoint();
            app.MapGetConversationsByUserIDEndpoint();
            return app;
        }

        public static IEndpointRouteBuilder MapMessageEndpoints(
           this IEndpointRouteBuilder app)
        {
            app.MapCreateMessageEndpoint();
            app.MapGetConversationMessagesEndpoint();
            return app;
        }

        public static IEndpointRouteBuilder MapUsersEndpoints(
            this IEndpointRouteBuilder app)
        {
            app.MapCreateUserEndpoint();
            app.MapGetUserEndpoint();
            return app;
        }

        public static IEndpointRouteBuilder MapCompaniesEndpoints(
           this IEndpointRouteBuilder app)
        {
            app.MapLoginCompanyEndpoint();
            app.MapRegisterCompanyEndpoint();
            app.MapRefreshTokenEndpoint();
            app.MapGetCompanyConversationsEndpoint();
            return app;
        }

        public static IEndpointRouteBuilder MapWebhookEndpoints(
           this IEndpointRouteBuilder app)
        {
            app.MapTelegramWebhookEndpoints();
            app.MapWhatsAppWebhookEndpoints();
            return app;
        }

        public static IEndpointRouteBuilder MapUserBotEndpoints(
           this IEndpointRouteBuilder app)
        {
            app.MapConnectTelegramBotEndpoint();
            return app;
        }
    }
}
