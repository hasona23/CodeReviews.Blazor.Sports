using Microsoft.AspNetCore.Identity;
using SportStatisticsApp.Data;

namespace SportStatisticsApp.Services.Users;

public interface IUserService
{
    public UserManager<ApplicationUser> UserManager { get; }
    public Task<List<ApplicationUser>> GetAllUsers();
    public Task<List<ApplicationUser>> GetAllUsersInRole(string role);
    public Task<ApplicationUser?> GetUserById(string id);
    public Task<ApplicationUser?> GetUserByEmail(string email);
    public Task EnableUser(string id);
    public Task DisableUser(string id,string reason);
}