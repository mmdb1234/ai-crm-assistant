using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.AI_Assistans.Persistence
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context )
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

        public async Task SaveRefreshTokenAsync(int companyId, string refreshToken)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(x => x.Id == companyId);

            if (company == null)
                throw new Exception("Company not found");

            company.RefreshToken = refreshToken;
            company.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();
        }
    }
}
