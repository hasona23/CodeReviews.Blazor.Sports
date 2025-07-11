using SportStatisticsApp.Data;
using SportStatisticsApp.Models.Dto;

namespace SportStatisticsApp.Models;

public class MatchAction
{
    public MatchAction(){}

    public MatchAction(MatchActionCreateDto dto)
    {
        ActionType = dto.ActionType;
        TimeAfterMatchBeginSeconds = dto.TimeAfterMatchBeganSeconds;
        Player =  dto.Player;
        Match =  dto.Match;
    }
    public int Id { get; set; }
    public MatchActionType ActionType { get; set; }
    public float TimeAfterMatchBeginSeconds { get; set; }
    public ApplicationUser Player { get; set; }
    public Match Match { get; set; }
}
public enum MatchActionType
{
    Intercept,
    Pass,
    Block,
    ThreePoint,
    TwoPoint,
}
