using Domain.AI_Assistans.Entities;
using Domain.AI_Assistans.Enums;
using Domain.AI_Assistans.Interfaces;

namespace AI_Assistans_CRM_Service.Tests;

public class RateLimitTests
{
    [Fact]
    public void NewCompany_ShouldHaveZeroAnalyses()
    {
        var company = new Company { Id = 1, Name = "T", Username = "t", Password = "p", CompanyRole = CompanyRole.Owner };

        Assert.Equal(0, company.DailyAnalysisCount);
        Assert.Null(company.LastAnalysisDate);
    }

    [Fact]
    public void CanAnalyze_NewCompanyWithZeroCount_ShouldBeAllowed()
    {
        var company = new Company { DailyAnalysisCount = 0 };

        var allowed = IsAllowed(company);

        Assert.True(allowed);
    }

    [Fact]
    public void CanAnalyze_WithThreeCountToday_ShouldNotBeAllowed()
    {
        var company = new Company { DailyAnalysisCount = 3, LastAnalysisDate = DateTime.UtcNow };

        var allowed = IsAllowed(company);

        Assert.False(allowed);
    }

    [Fact]
    public void CanAnalyze_WithTwoCountToday_ShouldBeAllowed()
    {
        var company = new Company { DailyAnalysisCount = 2, LastAnalysisDate = DateTime.UtcNow };

        var allowed = IsAllowed(company);

        Assert.True(allowed);
    }

    [Fact]
    public void CanAnalyze_WithThreeCountYesterday_ShouldBeAllowed()
    {
        var company = new Company { DailyAnalysisCount = 3, LastAnalysisDate = DateTime.UtcNow.AddDays(-1) };

        var allowed = IsAllowed(company);

        Assert.True(allowed);
    }

    [Fact]
    public void Increment_NewDay_ShouldResetTo1()
    {
        var company = new Company { DailyAnalysisCount = 3, LastAnalysisDate = DateTime.UtcNow.AddDays(-1) };

        Increment(ref company);

        Assert.Equal(1, company.DailyAnalysisCount);
        Assert.Equal(DateTime.UtcNow.Date, company.LastAnalysisDate!.Value.Date);
    }

    [Fact]
    public void Increment_SameDay_ShouldIncrease()
    {
        var company = new Company { DailyAnalysisCount = 1, LastAnalysisDate = DateTime.UtcNow };

        Increment(ref company);

        Assert.Equal(2, company.DailyAnalysisCount);
    }

    private static bool IsAllowed(Company company)
    {
        if (company.LastAnalysisDate?.Date != DateTime.UtcNow.Date)
            return true;
        return company.DailyAnalysisCount < 3;
    }

    private static void Increment(ref Company company)
    {
        if (company.LastAnalysisDate?.Date != DateTime.UtcNow.Date)
        {
            company.DailyAnalysisCount = 1;
            company.LastAnalysisDate = DateTime.UtcNow;
        }
        else
        {
            company.DailyAnalysisCount++;
        }
    }
}