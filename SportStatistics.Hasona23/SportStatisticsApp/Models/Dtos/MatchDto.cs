namespace SportStatisticsApp.Models.Dtos;

public record MatchCreateDto(string Name, DateTime Date, List<string> playersId, List<int> matchActionsId);
