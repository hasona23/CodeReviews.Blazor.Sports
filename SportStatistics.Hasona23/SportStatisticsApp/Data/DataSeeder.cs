using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Models;

namespace SportStatisticsApp.Data;

public static class DataSeeder
{
    
    private static Random _rand = new Random();
    public static async Task SeedData(IServiceScope scope)
    {
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>() ;
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!dbContext.Users.Any())
            await SeedUsers(userManager, roleManager);
        
        if(!dbContext.MatchActions.Any())
            await SeedMatches(dbContext);
            
    }

    private static async Task SeedMatches(ApplicationDbContext context)
    {
       
        for (int i = 0; i < 6; i++)
        {
            var match = new Match()
            {
                Date = GetRandomData(),
                Players = await context.Users.Where(usr => usr.DisplayName.Contains("Player")).ToListAsync(),
                Name = $"Match_{i}",
                MatchResult = GetRandomMatchResult(),
            };
            await context.Matches.AddAsync(match);
        }
        await context.SaveChangesAsync();
        var matches = context.Matches.Include(match => match.Players).ToList();
        var users = context.Users.Include(user => user.MatchesPlayed).Include(applicationUser => applicationUser.MatchActions).ToList();
        foreach (var match in matches)
        {

            
            foreach (var user in users)
            {
                if (user.DisplayName.Contains("Coach"))
                    break;
               
                
                int actionCount = _rand.Next(3, 11);

                for (int i = 0; i < actionCount; i++)
                {
                    var action = new MatchAction()
                    {
                        TimeAfterMatchBeginSeconds = _rand.NextSingle() * 48,
                        ActionType = GetRandomAction(),
                        Player = user,
                        Match = match,
                    };


                    await context.MatchActions.AddAsync(action);
                }
            }
        }
        await context.SaveChangesAsync();
    }

   

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(Roles.Coach));
        await roleManager.CreateAsync(new IdentityRole(Roles.TeamPlayer));
        await roleManager.CreateAsync(new IdentityRole(Roles.NormalUser));
        //5 main + 2 Substitution + Coach
        for (int i = 0; i < 7; i++)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            user.DisplayName = $"Player_{i}";
            user.Email = $"abc12{i}@example.com";
            user.UserName = $"abc12{i}@example.com";
            if (i < 2)
                user.IsActivePlayer = false;
            var result = await userManager.CreateAsync(user, "Password123!");
            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }
            user = await userManager.FindByEmailAsync($"abc12{i}@example.com");
            await userManager.AddToRoleAsync(user, Roles.TeamPlayer);
            
        }
        for (int i = 0; i < 3; i++)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            user.DisplayName = $"Normal_{i}";
            user.Email = $"abc13{i}@example.com";
            user.UserName = $"abc13{i}@example.com";
            var result = await userManager.CreateAsync(user, "Password123!");
            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }
            user = await userManager.FindByEmailAsync($"abc13{i}@example.com");
            await userManager.AddToRoleAsync(user, Roles.NormalUser);
            
        }
        var coachUser = Activator.CreateInstance<ApplicationUser>();
        coachUser.DisplayName = $"Mr. Coach";
        coachUser.Email = $"coach@example.com";
        coachUser.UserName = $"coach@example.com";
        await userManager.CreateAsync(coachUser, "Password123!");
        coachUser = await userManager.FindByEmailAsync($"coach@example.com");
        await userManager.AddToRoleAsync(coachUser, Roles.Coach);
    }
    private static MatchActionType GetRandomAction()
    {
        var values = Enum.GetValues<MatchActionType>();
        return values[_rand.Next(0, values.Length)];
    }
    private static DateTime GetRandomData()
    {
        return new DateTime(_rand.Next(2010,2026),_rand.Next(1,13),_rand.Next(1,28));
    }

    private static MatchResult GetRandomMatchResult()
    {
        var values = Enum.GetValues<MatchResult>();
        return values[_rand.Next(0, values.Length-1)];
    }
}
