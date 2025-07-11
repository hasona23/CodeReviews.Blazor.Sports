using SportStatisticsApp.Data;

namespace SportStatisticsApp.Models.Dto;

public record MatchActionCreateDto(MatchActionType ActionType,float TimeAfterMatchBeganSeconds,ApplicationUser Player,Match Match);