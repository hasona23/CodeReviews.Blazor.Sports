using Microsoft.AspNetCore.Identity;
using SportStatisticsApp.Models;

namespace SportStatisticsApp.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; }
    public List<Match> MatchesPlayed { get; set; }
    public List<MatchAction> MatchActions { get; set; }

    public bool IsActivePlayer { get; set; } = true;
    public string NotActiveReason { get; set; } = string.Empty;
}

