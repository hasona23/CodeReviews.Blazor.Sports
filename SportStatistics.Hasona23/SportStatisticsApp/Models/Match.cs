using SportStatisticsApp.Data;
using SportStatisticsApp.Models.Dto;

namespace SportStatisticsApp.Models;

public class Match
{
    public Match()
    {
        
    }
    public Match(MatchCreateDto dto)
    {
        Name = dto.Name;
        Date = dto.MatchDate;
        Players = dto.Players;
        Actions = dto.MatchActions;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public List<ApplicationUser> Players { get; set; }
    public List<MatchAction> Actions { get; set; }
}
