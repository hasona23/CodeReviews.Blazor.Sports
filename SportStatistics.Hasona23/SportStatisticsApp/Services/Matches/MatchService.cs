using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Data;
using SportStatisticsApp.Models;
using SportStatisticsApp.Models.Dto;

namespace SportStatisticsApp.Services.Matches;

public class MatchService : IMatchService
{
    private IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private ILogger<MatchService> _logger;

    public MatchService(IDbContextFactory<ApplicationDbContext> dbContextFactory, ILogger<MatchService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<List<Match>> GetMatchesAsync()
    {
        await using (var db = await _dbContextFactory.CreateDbContextAsync())
        {
            return await db.Matches.Include(match => match.Players).Include(match => match.Actions).ToListAsync();
        }
    }

    public async Task<Match?> GetMatchByIdAsync(int id)
    {
        await using (var db = await _dbContextFactory.CreateDbContextAsync())
        {
            return await db.Matches.FindAsync(id);
        }
    }

    public async Task<bool> CreateMatchAsync(MatchCreateDto match)
    {
        try
        {
            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {
                await db.Matches.AddAsync(new Match(match));
                await db.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error Adding Match of Name:{match.Name} Date:{match.MatchDate}.\nERROR:{e.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateMatchAsync(Match match)
    {
        try
        {
            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {
                var matchFound = await db.Matches.FindAsync(match.Id);
                if (matchFound == null) return false;
                matchFound.Name = match.Name;
                matchFound.Date = match.Date;

                await db.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error Updating Match of Id:{match.Id} Name:{match.Name} Date:{match.Date}.\nERROR:{e.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteMatchAsync(int id)
    {
        try
        {
            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {
                var match = await db.Matches.FindAsync(id);

                if (match == null) return false;

                db.Matches.Remove(match);
                await db.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error Updating Match of Id:{id}.\nERROR:{e.Message}");
            return false;
        }
    }
}