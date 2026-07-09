using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AI_Assistans.Persistence;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByUsernameAsync(string username)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<Company?> GetCompanyAsync(int companyID)
    {
        return await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == companyID);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Companies
            .AnyAsync(x => x.Username == username);
    }

    public async Task<Company> CreateAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task SaveRefreshTokenAsync(int companyId, string refreshToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == companyId);

        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.RefreshToken = refreshToken;
        company.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> CanAnalyzeAsync(int companyId)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == companyId);

        if (company == null)
            return false;

        if (company.LastAnalysisDate?.Date != DateTime.UtcNow.Date)
        {
            company.DailyAnalysisCount = 0;
            company.LastAnalysisDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        return company.DailyAnalysisCount < 3;
    }

    public async Task IncrementAnalysisCountAsync(int companyId)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == companyId);

        if (company == null)
            return;

        if (company.LastAnalysisDate?.Date != DateTime.UtcNow.Date)
        {
            company.DailyAnalysisCount = 1;
            company.LastAnalysisDate = DateTime.UtcNow;
        }
        else
        {
            company.DailyAnalysisCount++;
        }

        await _context.SaveChangesAsync();
    }
}
