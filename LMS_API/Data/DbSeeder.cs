using LMS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS_API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Prefer migrations, but if the model has pending changes in development,
        // rebuild schema so required tables (e.g. Students) still exist.
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("PendingModelChangesWarning"))
        {
            // Do not drop data on startup. If DB does not exist yet, create it.
            var canConnect = await context.Database.CanConnectAsync();
            if (!canConnect)
            {
                await context.Database.EnsureCreatedAsync();
            }
        }

        // Seed one default teacher only if it does not already exist.
        var teacherExists = await context.Teacher
            .AsNoTracking()
            .AnyAsync(t => t.Email.ToLower() == "teacher@school.com");

        if (!teacherExists)
        {
            var teacher = new Teacher
            {
                Email = "teacher@school.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                CreatedDate = DateTime.UtcNow,
            };

            context.Teacher.Add(teacher);
            await context.SaveChangesAsync();
        }
    }
}
