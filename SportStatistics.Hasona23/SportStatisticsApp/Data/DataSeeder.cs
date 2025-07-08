using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Models;

namespace SportStatisticsApp.Data;

public static class DataSeeder
{
    
    private static Random _rand = new Random();
    public static void SeedData(IServiceScope scope)
    {
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>() ;
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!dbContext.Users.Any())
            SeedUsers(userManager, roleManager);
        
        if(!dbContext.MatchActions.Any())
            SeedMatches(dbContext);
            
    }

    private static void SeedMatches(ApplicationDbContext context)
    {
       
        for (int i = 0; i < 3; i++)
        {
            var match = new Match()
            {
                Date = GetRandomData(),
                Players = context.Users.Where(usr => usr.DisplayName.Contains("Player")).ToList(),
                Name = $"Match_{i}"
            };
            context.Matches.Add(match);
        }
        context.SaveChanges();
        foreach (var match in context.Matches.Include(match => match.Players))
        {


            foreach (var user in context.Users.Include(applicationUser => applicationUser.MatchesPlayed)
                         .Include(applicationUser => applicationUser.MatchActions))
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


                    context.MatchActions.Add(action);
                }
            }
        }
        context.SaveChanges();
    }

   

    private static void SeedUsers(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        roleManager.CreateAsync(new IdentityRole(Roles.Coach)).Wait();
        roleManager.CreateAsync(new IdentityRole(Roles.TeamPlayer)).Wait();
        roleManager.CreateAsync(new IdentityRole(Roles.NormalUser)).Wait();
        //5 main + 2 Substitution + Coach
        for (int i = 0; i < 7; i++)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            user.DisplayName = $"Player_{i}";
            user.Email = $"abc12{i}@example.com";
            user.UserName = $"abc12{i}@example.com";
            var result = userManager.CreateAsync(user, "Password123!").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }
            user = userManager.FindByEmailAsync($"abc12{i}@example.com").Result;
            userManager.AddToRoleAsync(user, Roles.TeamPlayer).Wait();
            
        }
        for (int i = 0; i < 3; i++)
        {
            var user = Activator.CreateInstance<ApplicationUser>();
            user.DisplayName = $"Normal_{i}";
            user.Email = $"abc13{i}@example.com";
            user.UserName = $"abc13{i}@example.com";
            var result = userManager.CreateAsync(user, "Password123!").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.ToString());
            }
            user = userManager.FindByEmailAsync($"abc13{i}@example.com").Result;
            userManager.AddToRoleAsync(user, Roles.NormalUser).Wait();
            
        }
        var coachUser = Activator.CreateInstance<ApplicationUser>();
        coachUser.DisplayName = $"Mr. Coach";
        coachUser.Email = $"coach@example.com";
        coachUser.UserName = $"coach@example.com";
        userManager.CreateAsync(coachUser, "Password123!").Wait();
        coachUser = userManager.FindByEmailAsync($"coach@example.com").Result;
        userManager.AddToRoleAsync(coachUser, Roles.Coach).Wait();
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
}
