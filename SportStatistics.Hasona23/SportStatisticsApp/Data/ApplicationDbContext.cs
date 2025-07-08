using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportStatisticsApp.Models;

namespace SportStatisticsApp.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Match> Matches { get; set; }
    public DbSet<MatchAction> MatchActions { get; set; }

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<MatchAction>().HasOne(matchAction => matchAction.Match)
            .WithMany(match => match.Actions);
        
        builder.Entity<MatchAction>().HasOne(matchAction => matchAction.Player)
            .WithMany(user => user.MatchActions);
        
        builder.Entity<ApplicationUser>().HasMany(user => user.MatchesPlayed)
            .WithMany(match => match.Players)
            .UsingEntity(e=>e.ToTable("MatchUserJointTable"));
       
         }
}
