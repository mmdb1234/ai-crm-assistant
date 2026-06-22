using Domain.AI_Assistans.Enums;

namespace AI_Assistans_CRM_Service
{
    public static class AppSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Companies.AnyAsync())
                return;

            // =========================
            // 🏢 COMPANIES
            // =========================
            var ownerCompany = new Company
            {
                Name = "Nova AI CRM",
                Username = "nova-owner",
                Password = "123",
                CompanyRole = CompanyRole.Owner,
                Users = new List<User>(),
                Conversations = new List<Conversation>()
            };

            var supportCompany = new Company
            {
                Name = "SupportX",
                Username = "support-manager",
                Password = "123",
                CompanyRole = CompanyRole.Manager,
                Users = new List<User>(),
                Conversations = new List<Conversation>()
            };

            // =========================
            // 👤 USERS
            // =========================
            var userA = new User { Id = Guid.NewGuid(), Username = "ali.karimi", Company = ownerCompany };
            var userB = new User { Id = Guid.NewGuid(), Username = "sara.mohammadi", Company = ownerCompany };
            var userC = new User { Id = Guid.NewGuid(), Username = "john.doe", Company = supportCompany };

            ownerCompany.Users.Add(userA);
            ownerCompany.Users.Add(userB);
            supportCompany.Users.Add(userC);

            // =========================
            // 💬 CONVERSATIONS
            // =========================
            var hotLead = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Enterprise Pricing Request",
                Company = ownerCompany,
                User = userA,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var supportCase = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Payment Failed - Urgent",
                Company = supportCompany,
                User = userC,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var coldLead = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Demo Request",
                Company = ownerCompany,
                User = userB,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            ownerCompany.Conversations.Add(hotLead);
            ownerCompany.Conversations.Add(coldLead);
            supportCompany.Conversations.Add(supportCase);

            // =========================
            // 🔥 MESSAGES + ANALYSIS
            // =========================
            hotLead.Messages.Add(
            new Message
            {
                Role = MessageRole.Customer,
                Content = "We are 300+ employees, need enterprise plan",
                SentAt = DateTime.UtcNow.AddMinutes(-40)
            }
        );
            hotLead.Messages.Add(
            new Message
            {
                Role = MessageRole.SalesAgent,
                Content = "Great! I can prepare a custom proposal for you",
                SentAt = DateTime.UtcNow.AddMinutes(-39)
            }
        );

            hotLead.Analyses.Add(new ConversationAnalysis
            {
                Summary = "High intent enterprise lead",
                Sentiment = "Positive",
                LeadScore = 95,
                SuggestedReply = "Send proposal ASAP",
                SuggestedNextAction = "Sales call",
                ModelName = "seed-ai",
                Version = "v1"
            });

            supportCase.Messages.Add(
                new Message
                {
                    Role = MessageRole.Customer,
                    Content = "Payment failed 3 times",
                    SentAt = DateTime.UtcNow.AddHours(-2)
                }
        );

            supportCase.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Critical billing issue",
                Sentiment = "Negative",
                LeadScore = 20,
                SuggestedReply = "Escalate immediately",
                SuggestedNextAction = "Fix billing",
                ModelName = "seed-ai",
                Version = "v1"
            });

            coldLead.Messages.Add(new Message
            {
                Role = MessageRole.Customer,
                Content = "Can I see a demo?",
                SentAt = DateTime.UtcNow.AddDays(-1)
            });

            coldLead.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Cold lead",
                Sentiment = "Neutral",
                LeadScore = 55,
                SuggestedReply = "Send demo link",
                SuggestedNextAction = "Follow up",
                ModelName = "seed-ai",
                Version = "v1"
            });

            // =========================
            // 💾 SAVE ONLY ROOTS
            // =========================
            await context.Companies.AddRangeAsync(ownerCompany, supportCompany);
            await context.SaveChangesAsync();
        }
    }
}
