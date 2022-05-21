using VolgaIT.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VolgaIT.Services
{
    public static class DatabaseManagmentService
    {
        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
            }
        }
    }
}