using SportStatisticsApp.Models;
using SportStatisticsApp.Models.Dto;

namespace SportStatisticsApp.Services.Matches;

public interface IMatchService
{
    public Task<List<Match>> GetMatchesAsync();
    public Task<Match?> GetMatchByIdAsync(int id);
    public Task<bool> CreateMatchAsync(MatchCreateDto  matchCreateDto);
    public Task<bool> UpdateMatchAsync(Match match);
    public Task<bool> DeleteMatchAsync(int id);
}