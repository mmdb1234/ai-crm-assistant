namespace AI_Assistans_CRM_Service.Companies.GetCompanyConversations
{
    public class GetCompanyConversationsRequest
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; }= 10;  
        public string SearchText { get; set; }

        public Guid? UserID { get; set; }
    }
}
