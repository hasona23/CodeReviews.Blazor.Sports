namespace SportStatisticsApp.Models.Dtos;

public record MatchCreateDto(string Name, MatchResult MatchResult,DateTime Date, List<string> playersId, List<int> matchActionsId);
