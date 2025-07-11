using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Data;

namespace SportStatisticsApp.Services.Users;

public class UserService:IUserService,IDisposable
{
    public UserManager<ApplicationUser> UserManager { get; }
    private RoleManager<IdentityRole>  _roleManager;
    private IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager,
        IDbContextFactory<ApplicationDbContext> dbContextFactory, ILogger<UserService> logger,RoleManager<IdentityRole> roleManager)
    {
        UserManager = userManager;
        _dbContextFactory = dbContextFactory;
        _logger = logger;
        _roleManager = roleManager;
    }
    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        await using (var db = await _dbContextFactory.CreateDbContextAsync())
        {
            return await db.Users.Include(user => user.MatchActions).Include(user => user.MatchesPlayed).ToListAsync();
        } 
    }

    public async Task<List<ApplicationUser>> GetAllUsersInRole(string role="all")
    {
        if (role == "all")
        {
            return await UserManager.Users.ToListAsync();
        }

        if (await _roleManager.RoleExistsAsync(role))
        {
            return (await UserManager.GetUsersInRoleAsync(role)).ToList();
        }

        return [];
    }

    public async Task<ApplicationUser?> GetUserById(string id)
    {
        return await UserManager.FindByIdAsync(id);
    }

    public async Task<ApplicationUser?> GetUserByEmail(string email)
    {
        return await UserManager.FindByEmailAsync(email);
    }

    public async Task EnableUser(string id)
    {
        try
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user is not null)
            {
                user.IsActivePlayer = true;
                user.NotActiveReason = string.Empty;
                await UserManager.UpdateAsync(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error enabling user of id {id}");
        }
    }

    public async Task DisableUser(string id, string reason)
    {
        try
        {
            var user = await UserManager.FindByIdAsync(id);
           
            if (user is not null )
            {
                if (string.IsNullOrEmpty(reason))
                {
                    _logger.LogWarning($"Reason cannot be null or empty when disabling player {user.DisplayName}-{user.Email}-{user.Id}");
                    return;
                }
                user.IsActivePlayer = false;
                user.NotActiveReason = reason;
                await UserManager.UpdateAsync(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error enabling user of id {id}");
        }
    }
    
    public void Dispose()
    {
        UserManager.Dispose();
    }
}