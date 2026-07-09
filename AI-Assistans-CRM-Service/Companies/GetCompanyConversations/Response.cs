

namespace AI_Assistans_CRM_Service.Companies.GetCompanyConversations
{
    public class GetCompanyConversationsResponse
    {
        public long TotalCount { get; set; }
         
        public List<ConversationsDto> Conversations { get; set; } = new();

    }

    public class ConversationsDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public int CompanyId { get; set; }

        public string LatestSummary { get; set; } = default!;

        public string LatestSentiment { get; set; } = default!;

        public int LatestLeadScore { get; set; }

    }
}
