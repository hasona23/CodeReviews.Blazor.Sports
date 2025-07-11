using SportStatisticsApp.Data;

namespace SportStatisticsApp.Models.Dto;

public record MatchCreateDto(string Name,DateTime MatchDate,List<MatchAction> MatchActions,List<ApplicationUser> Players);
