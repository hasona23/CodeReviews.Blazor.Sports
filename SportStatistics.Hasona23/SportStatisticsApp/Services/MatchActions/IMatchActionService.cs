using SportStatisticsApp.Models;

namespace SportStatisticsApp.Services.MatchActions;

public interface IMatchActionService
{
    public Task<List<MatchAction>> GetAllMatchActionsAsync();
    public Task<MatchAction?> GetMatchActionByIdAsync(int id);
    public Task<bool> CreateMatchActionAsync(MatchAction matchAction);
    public Task<bool> UpdateMatchActionAsync(MatchAction matchAction);
    public Task<bool> DeleteMatchActionAsync(int id);
}