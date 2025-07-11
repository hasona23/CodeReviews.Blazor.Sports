using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Data;
using SportStatisticsApp.Models;

namespace SportStatisticsApp.Services.MatchActions;

public class MatchActionService : IMatchActionService
{
    private IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private ILogger<MatchActionService> _logger;

    public MatchActionService(IDbContextFactory<ApplicationDbContext> dbContextFactory, ILogger<MatchActionService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<List<MatchAction>> GetAllMatchActionsAsync()
    {
        await using (var db = await _dbContextFactory.CreateDbContextAsync())
        {
            return await db.MatchActions.Include(action => action.Match).Include(action => action.Player).ToListAsync();

        }
    }

    public async Task<MatchAction?> GetMatchActionByIdAsync(int id)
    {
        await using (var db = await _dbContextFactory.CreateDbContextAsync())
        {
            return await db.MatchActions.FindAsync(id);
        }

    }

    public async Task<bool> CreateMatchActionAsync(MatchAction matchAction)
    {
        try
        {

            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {
                var user = await db.Users.FindAsync(matchAction.Player.Id);
                var match = await db.Matches.FindAsync(matchAction.Match.Id);
                matchAction.Player = user;
                matchAction.Match = match;
                await db.MatchActions.AddAsync(matchAction);
                await db.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error creating match action({matchAction}): {e.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateMatchActionAsync(MatchAction matchAction)
    {
        try
        {
            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {

                var matchActionFound = await db.MatchActions.FindAsync(matchAction.Id);
                if (matchActionFound == null) return false;
                matchActionFound.ActionType = matchAction.ActionType;
                matchActionFound.TimeAfterMatchBeginSeconds = matchActionFound.TimeAfterMatchBeginSeconds;

                await db.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error Updating Match Action of Id:{matchAction.Id} Type:{matchAction.ActionType.ToString()} Time:{matchAction.TimeAfterMatchBeginSeconds}.\nERROR:{e.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteMatchActionAsync(int id)
    {
        try
        {
            await using (var db = await _dbContextFactory.CreateDbContextAsync())
            {
                var matchActionFound = await db.MatchActions.FindAsync(id);
                if (matchActionFound == null) return false;
                db.Remove(matchActionFound);
                await db.SaveChangesAsync();
                return true;
            }

        }
        catch (Exception e)
        {
            _logger.LogError($"Error Updating Match Action of Id:{id} .\nERROR:{e.Message}");
            return false;
        }
    }
}