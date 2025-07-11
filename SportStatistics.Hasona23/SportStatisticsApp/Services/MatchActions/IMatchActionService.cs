using SportStatisticsApp.Models;
using SportStatisticsApp.Models.Dto;

namespace SportStatisticsApp.Services.MatchActions;

public interface IMatchActionService
{
    public Task<List<MatchAction>> GetAllMatchActionsAsync();
    public  Task<MatchAction?> GetMatchActionByIdAsync(int id);
    public Task<bool> CreateMatchActionAsync(MatchActionCreateDto matchActionDto);
    public Task<bool> UpdateMatchActionAsync(MatchAction matchAction);
    public Task<bool> DeleteMatchActionAsync(int id);
}