using System.Security.Claims;

namespace AI_Assistans_CRM_Service.Companies.GetCompanyConversations
{
  
    public static class GetCompanyConversationsEndpoint
    {
        public static IEndpointRouteBuilder MapGetCompanyConversationsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("company/conversations",
                async (ClaimsPrincipal user , [AsParameters] GetCompanyConversationsRequest request, IAppDbContext context) =>
                {
                    var companyIdClaim = user.FindFirst("CompanyId")?.Value;

                    if (string.IsNullOrEmpty(companyIdClaim))
                        return Results.Unauthorized();

                    var companyId = int.Parse(companyIdClaim);

                    var query =  context.Conversations.Where(c => c.CompanyId == companyId);

                    var totalcount =await  context.Conversations.Where(c => c.CompanyId == companyId).CountAsync();


                    if (request.UserID.HasValue)
                    {
                        query = query.Where(c => c.UserId == request.UserID);
                    }

                    if (!string.IsNullOrEmpty(request.SearchText))
                    {
                        query = query.Where(c => c.Title.Contains(request.SearchText) || c.Description.Contains(request.SearchText));
                    }

                    var conversations =  await query
                                            .Include(c=>c.User)
                                            .OrderByDescending(c=>c.CreatedAt)
                                            .Skip(request.PageSize * request.PageIndex)
                                            .Take(request.PageSize)
                                            .ToListAsync();



                    var result =  new GetCompanyConversationsResponse
                    {
                        TotalCount = totalcount,

                        Conversations = conversations.Select(c => new ConversationsDto
                        {
                            Id = c.Id,
                            CompanyId = c.CompanyId,
                            Description = c.Description,
                            Title = c.Title,
                            UserId = c.UserId,
                            UserName = c.User.Username
                        }).ToList()
                    };

                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetCompanyConversations");
            return app;
        }
    }
}
