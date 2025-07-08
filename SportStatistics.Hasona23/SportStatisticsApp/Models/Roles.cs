namespace SportStatisticsApp.Models;

public static class Roles
{
    //Admin
    public const string Coach = "Coach";
    //Team Member
    public const string TeamPlayer = "TeamPlayer";
    //Normal User
    // Used as queue till Promote to player
    public const string NormalUser = "NormalUser";
}