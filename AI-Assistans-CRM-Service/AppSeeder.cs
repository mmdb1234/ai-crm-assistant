using Domain.AI_Assistans.Enums;
using Features.AI_Assistans.Services;

namespace AI_Assistans_CRM_Service
{
    public static class AppSeeder
    {
        public static async Task SeedAsync(IAppDbContext context)
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

            var techCompany = new Company
            {
                Name = "TechStartup Hub",
                Username = "tech-ceo",
                Password = "123",
                CompanyRole = CompanyRole.Manager,
                Users = new List<User>(),
                Conversations = new List<Conversation>()
            };

            // =========================
            // 👤 USERS - Nova AI CRM
            // =========================
            var user1 = new User { Id = Guid.NewGuid(), Username = "ali.karimi", Company = ownerCompany };
            var user2 = new User { Id = Guid.NewGuid(), Username = "sara.mohammadi", Company = ownerCompany };
            var user3 = new User { Id = Guid.NewGuid(), Username = "mehdi.rezaei", Company = ownerCompany };
            var user4 = new User { Id = Guid.NewGuid(), Username = "zahra.nouri", Company = ownerCompany };
            var user5 = new User { Id = Guid.NewGuid(), Username = "reza.ahmadi", Company = ownerCompany };
            var user6 = new User { Id = Guid.NewGuid(), Username = "maryam.karimi", Company = ownerCompany };
            var user7 = new User { Id = Guid.NewGuid(), Username = "amir.hosseini", Company = ownerCompany };
            var user8 = new User { Id = Guid.NewGuid(), Username = "nazanin.mohseni", Company = ownerCompany };

            ownerCompany.Users.Add(user1);
            ownerCompany.Users.Add(user2);
            ownerCompany.Users.Add(user3);
            ownerCompany.Users.Add(user4);
            ownerCompany.Users.Add(user5);
            ownerCompany.Users.Add(user6);
            ownerCompany.Users.Add(user7);
            ownerCompany.Users.Add(user8);

            // =========================
            // 👤 USERS - SupportX
            // =========================
            var user9 = new User { Id = Guid.NewGuid(), Username = "john.doe", Company = supportCompany };
            var user10 = new User { Id = Guid.NewGuid(), Username = "jane.smith", Company = supportCompany };
            var user11 = new User { Id = Guid.NewGuid(), Username = "robert.brown", Company = supportCompany };
            var user12 = new User { Id = Guid.NewGuid(), Username = "emily.davis", Company = supportCompany };
            var user13 = new User { Id = Guid.NewGuid(), Username = "michael.wilson", Company = supportCompany };
            var user14 = new User { Id = Guid.NewGuid(), Username = "sarah.johnson", Company = supportCompany };

            supportCompany.Users.Add(user9);
            supportCompany.Users.Add(user10);
            supportCompany.Users.Add(user11);
            supportCompany.Users.Add(user12);
            supportCompany.Users.Add(user13);
            supportCompany.Users.Add(user14);

            // =========================
            // 👤 USERS - TechStartup Hub
            // =========================
            var user15 = new User { Id = Guid.NewGuid(), Username = "alex.chen", Company = techCompany };
            var user16 = new User { Id = Guid.NewGuid(), Username = "maria.garcia", Company = techCompany };
            var user17 = new User { Id = Guid.NewGuid(), Username = "david.kim", Company = techCompany };
            var user18 = new User { Id = Guid.NewGuid(), Username = "lisa.wang", Company = techCompany };
            var user19 = new User { Id = Guid.NewGuid(), Username = "james.anderson", Company = techCompany };
            var user20 = new User { Id = Guid.NewGuid(), Username = "emma.thompson", Company = techCompany };

            techCompany.Users.Add(user15); 
            techCompany.Users.Add(user16); 
            techCompany.Users.Add(user17); 
            techCompany.Users.Add(user18); 
            techCompany.Users.Add(user19); 
            techCompany.Users.Add(user20); ;

            // =========================
            // 💬 CONVERSATIONS - Nova AI CRM (Hot Leads - High Score)
            // =========================
            var conv1 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Enterprise Pricing Request",
                Company = ownerCompany,
                User = user1,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv2 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "AI Integration for E-Commerce",
                Company = ownerCompany,
                User = user3,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv3 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Multi-Agent System Demo",
                Company = ownerCompany,
                User = user5,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv4 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Custom AI Model Training",
                Company = ownerCompany,
                User = user7,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            // =========================
            // 💬 CONVERSATIONS - Nova AI CRM (Warm Leads - Medium Score)
            // =========================
            var conv5 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Demo Request",
                Company = ownerCompany,
                User = user2,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv6 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "API Documentation Question",
                Company = ownerCompany,
                User = user4,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv7 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Integration with Slack",
                Company = ownerCompany,
                User = user6,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv8 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Knowledge Base Setup",
                Company = ownerCompany,
                User = user8,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            // =========================
            // 💬 CONVERSATIONS - SupportX (Various Topics)
            // =========================
            var conv9 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Payment Failed - Urgent",
                Company = supportCompany,
                User = user9,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv10 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Account Suspension Inquiry",
                Company = supportCompany,
                User = user10,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv11 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Billing Dispute Resolution",
                Company = supportCompany,
                User = user11,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv12 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Feature Request: Export Data",
                Company = supportCompany,
                User = user12,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv13 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Subscription Upgrade Question",
                Company = supportCompany,
                User = user13,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv14 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Technical Support: Server Error",
                Company = supportCompany,
                User = user14,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            // =========================
            // 💬 CONVERSATIONS - TechStartup Hub (Startup & Tech)
            // =========================
            var conv15 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "YC Application Preparation",
                Company = techCompany,
                User = user15,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv16 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "MVP Development Timeline",
                Company = techCompany,
                User = user16,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv17 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Investor Pitch Deck Review",
                Company = techCompany,
                User = user17,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv18 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Product Strategy Session",
                Company = techCompany,
                User = user18,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv19 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Hiring Technical Talent",
                Company = techCompany,
                User = user19,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            var conv20 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Cloud Infrastructure Planning",
                Company = techCompany,
                User = user20,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };

            // =========================
            // 🔥 ADD CONVERSATIONS TO COMPANIES
            // =========================
            ownerCompany.Conversations.Add(conv1);
            ownerCompany.Conversations.Add(conv2);
            ownerCompany.Conversations.Add(conv3);
            ownerCompany.Conversations.Add(conv4);
            ownerCompany.Conversations.Add(conv5);
            ownerCompany.Conversations.Add(conv6);
            ownerCompany.Conversations.Add(conv7);
            ownerCompany.Conversations.Add(conv8);
            supportCompany.Conversations.Add(conv9);
            supportCompany.Conversations.Add(conv10);
            supportCompany.Conversations.Add(conv11);
            supportCompany.Conversations.Add(conv12); 
            supportCompany.Conversations.Add(conv13); 
            supportCompany.Conversations.Add(conv14);
            techCompany.Conversations.Add(conv15); 
            techCompany.Conversations.Add(conv16);
            techCompany.Conversations.Add(conv17);
            techCompany.Conversations.Add(conv18);
            techCompany.Conversations.Add(conv19);
            techCompany.Conversations.Add(conv20);

            // =========================
            // 📝 MESSAGES & ANALYSES - Hot Leads
            // =========================
            // Conv1: Enterprise Pricing Request (Hot - 95)

            conv1.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We are 300+ employees, need enterprise plan", SentAt = DateTime.UtcNow.AddMinutes(-40) });
            conv1.Messages.Add(new Message { Role = MessageRole.SalesAgent, Content = "Great! I can prepare a custom proposal for you", SentAt = DateTime.UtcNow.AddMinutes(-39) });
            conv1.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We need API access and dedicated support", SentAt = DateTime.UtcNow.AddMinutes(-35) });
            conv1.Messages.Add(new Message { Role = MessageRole.SalesAgent, Content = "Absolutely! Enterprise tier includes both", SentAt = DateTime.UtcNow.AddMinutes(-33) });
      
            conv1.Analyses.Add(new ConversationAnalysis
            {
                Summary = "High intent enterprise lead - ready to buy",
                Sentiment = "Positive",
                LeadScore = 95,
                SuggestedReply = "Send proposal with enterprise pricing",
                SuggestedNextAction = "Schedule enterprise demo",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Conv2: AI Integration for E-Commerce (Hot - 92)

            conv2.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We run an e-commerce platform with 50K daily orders", SentAt = DateTime.UtcNow.AddHours(-3) });
            conv2.Messages.Add(new Message { Role = MessageRole.SalesAgent, Content = "Our AI can optimize your order processing", SentAt = DateTime.UtcNow.AddHours(-2.5) });
            conv2.Messages.Add(new Message { Role = MessageRole.Customer, Content = "Can you handle multi-currency transactions?", SentAt = DateTime.UtcNow.AddHours(-2) });
        
            conv2.Analyses.Add(new ConversationAnalysis
            {
                Summary = "High-volume e-commerce integration opportunity",
                Sentiment = "Positive",
                LeadScore = 92,
                SuggestedReply = "Yes, we support 50+ currencies",
                SuggestedNextAction = "Technical deep-dive call",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Conv3: Multi-Agent System Demo (Hot - 88)
            conv3.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We need a multi-agent system for customer support", SentAt = DateTime.UtcNow.AddHours(-5) });
            conv3.Messages.Add(new Message { Role = MessageRole.SalesAgent, Content = "Our agent orchestration platform is perfect", SentAt = DateTime.UtcNow.AddHours(-4.5) });
            
            conv3.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Strategic multi-agent system opportunity",
                Sentiment = "Positive",
                LeadScore = 88,
                SuggestedReply = "Schedule a technical demo",
                SuggestedNextAction = "Solution architecture review",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // =========================
            // 📝 MESSAGES & ANALYSES - Warm Leads
            // =========================
            // Conv5: Demo Request (Warm - 55)
            conv5.Messages.Add(new Message { Role = MessageRole.Customer, Content = "Can I see a demo of your CRM?", SentAt = DateTime.UtcNow.AddDays(-1) });
            conv5.Messages.Add(new Message { Role = MessageRole.Support, Content = "Certainly! When would you be available?", SentAt = DateTime.UtcNow.AddDays(-0.5) });
            
            conv5.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Interested but not urgent",
                Sentiment = "Neutral",
                LeadScore = 55,
                SuggestedReply = "Send demo link and schedule call",
                SuggestedNextAction = "Follow up in 2 days",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Conv6: API Documentation Question (Warm - 60)
            conv6.Messages.Add(new Message { Role = MessageRole.Customer, Content = "Your API docs are unclear about webhooks", SentAt = DateTime.UtcNow.AddDays(-2) });
            conv6.Messages.Add(new Message { Role = MessageRole.Support, Content = "Let me clarify: webhooks are sent to your endpoint", SentAt = DateTime.UtcNow.AddDays(-1.5) });

            conv6.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Developer interested in integration",
                Sentiment = "Neutral",
                LeadScore = 60,
                SuggestedReply = "Provide detailed webhook examples",
                SuggestedNextAction = "Share additional documentation",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // =========================
            // 📝 MESSAGES & ANALYSES - SupportX (Cold & Warm)
            // =========================
            // Conv9: Payment Failed - Urgent (Cold - 20)
            conv9.Messages.Add(new Message { Role = MessageRole.Customer, Content = "Payment failed 3 times, getting error codes", SentAt = DateTime.UtcNow.AddHours(-2) });
            conv9.Messages.Add(new Message { Role = MessageRole.Support, Content = "I'll escalate this to our billing team", SentAt = DateTime.UtcNow.AddHours(-1.5) });
            
            conv9.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Critical billing issue - immediate attention needed",
                Sentiment = "Negative",
                LeadScore = 20,
                SuggestedReply = "Escalate to engineering team",
                SuggestedNextAction = "Fix payment gateway integration",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Conv10: Account Suspension (Warm - 45)
            conv10.Messages.Add(new Message { Role = MessageRole.Customer, Content = "Why was my account suspended?", SentAt = DateTime.UtcNow.AddHours(-4) });
            conv10.Messages.Add(new Message { Role = MessageRole.Support, Content = "Checking the reason...", SentAt = DateTime.UtcNow.AddHours(-3.5) });

            conv10.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Account suspension inquiry",
                Sentiment = "Negative",
                LeadScore = 45,
                SuggestedReply = "Explain suspension reason and next steps",
                SuggestedNextAction = "Follow up with customer",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // =========================
            // 📝 MESSAGES & ANALYSES - TechStartup Hub
            // =========================
            // Conv15: YC Application (Hot - 85)
            conv15.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We need 3 senior AI engineers", SentAt = DateTime.UtcNow.AddDays(-1) });
            conv15.Messages.Add( new Message { Role = MessageRole.Customer, Content = "We're applying to Y Combinator, need pitch feedback", SentAt = DateTime.UtcNow.AddDays(-3) });

            conv15.Analyses.Add(new ConversationAnalysis
            {
                Summary = "YC application preparation - high value customer",
                Sentiment = "Positive",
                LeadScore = 85,
                SuggestedReply = "Schedule pitch review session",
                SuggestedNextAction = "Prepare feedback document",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Conv19: Hiring Technical Talent (Hot - 82)
            conv19.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We need 3 senior AI engineers", SentAt = DateTime.UtcNow.AddDays(-1) });
            conv19.Messages.Add(new Message { Role = MessageRole.Support, Content = "We can help with recruitment strategy", SentAt = DateTime.UtcNow.AddHours(-12) });

            conv19.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Technical hiring support needed",
                Sentiment = "Positive",
                LeadScore = 82,
                SuggestedReply = "Share recruitment package",
                SuggestedNextAction = "Schedule HR consultation",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Conv20: Cloud Infrastructure (Warm - 65)
            conv20.Messages.Add(new Message { Role = MessageRole.Customer, Content = "We're migrating to cloud, need architecture guidance", SentAt = DateTime.UtcNow.AddDays(-5) });
            conv20.Messages.Add(new Message { Role = MessageRole.Support, Content = "We specialize in cloud migrations", SentAt = DateTime.UtcNow.AddDays(-4) });
            
            conv20.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Cloud migration consultation",
                Sentiment = "Neutral",
                LeadScore = 65,
                SuggestedReply = "Schedule architecture review",
                SuggestedNextAction = "Prepare migration roadmap",
                ModelName = "openai-gpt4",
                Version = "v2"
            });

            // Add a few more cold leads for variety
            var convCold1 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "General Information Inquiry",
                Company = ownerCompany,
                User = user4,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };
            convCold1.Messages.Add(new Message { Role = MessageRole.Customer, Content = "Just browsing your website", SentAt = DateTime.UtcNow.AddDays(-10) });
            convCold1.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Low interest - just browsing",
                Sentiment = "Neutral",
                LeadScore = 15,
                SuggestedReply = "Send general information package",
                SuggestedNextAction = "No immediate follow-up needed",
                ModelName = "openai-gpt4",
                Version = "v2"
            });
            ownerCompany.Conversations.Add(convCold1);

            var convCold2 = new Conversation
            {
                Id = Guid.NewGuid(),
                Title = "Competitor Comparison",
                Company = ownerCompany,
                User = user8,
                Messages = new List<Message>(),
                Analyses = new List<ConversationAnalysis>()
            };
            convCold2.Messages.Add(new Message { Role = MessageRole.Customer, Content = "How do you compare to competitors?", SentAt = DateTime.UtcNow.AddDays(-7) });
            convCold2.Analyses.Add(new ConversationAnalysis
            {
                Summary = "Price-sensitive customer comparing options",
                Sentiment = "Negative",
                LeadScore = 25,
                SuggestedReply = "Highlight unique value proposition",
                SuggestedNextAction = "Send case studies",
                ModelName = "openai-gpt4",
                Version = "v2"
            });
            ownerCompany.Conversations.Add(convCold2);

            // =========================
            // 💾 SAVE
            // =========================
            await context.Companies.AddRangeAsync(ownerCompany, supportCompany, techCompany);
            await context.SaveChangesAsync();
        }
    }
}
