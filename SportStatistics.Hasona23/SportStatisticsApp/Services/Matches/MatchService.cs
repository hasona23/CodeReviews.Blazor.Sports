using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Data;
using SportStatisticsApp.Models;
using SportStatisticsApp.Models.Dtos;

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

    public async Task<Match> CreateMatchAsync(MatchCreateDto matchDto)
    {
        try
        {

            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {
                var players = await db.Users.Where(user => matchDto.playersId.Contains(user.Id)).ToListAsync();
                var actions = await db.MatchActions.Where(action => matchDto.matchActionsId.Contains(action.Id)).ToListAsync();
                var match = new Match()
                {
                    Name = matchDto.Name,
                    Date = matchDto.Date,
                    Players = players,
                    Actions = actions,
                };
                await db.Matches.AddAsync(match);
                await db.SaveChangesAsync();
                return match;
            }


        }
        catch (Exception e)
        {
            _logger.LogError($"Error Adding Match of Name:{matchDto.Name} Date:{matchDto.Date}.\nERROR:{e}");
            return null;
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
                matchFound.MatchResult = match.MatchResult;
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