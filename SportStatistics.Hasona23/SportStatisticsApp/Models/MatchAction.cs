using SportStatisticsApp.Data;

namespace SportStatisticsApp.Models;

public class MatchAction
{
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
