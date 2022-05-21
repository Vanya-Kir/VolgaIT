using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolgaIT.Models;

namespace VolgaIT.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Application> Application { get; set; }
    public DbSet<Event> Event { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
