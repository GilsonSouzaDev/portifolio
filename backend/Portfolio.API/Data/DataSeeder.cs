using Portfolio.API.Models;

namespace Portfolio.API.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Profiles.Count() == 0)
        {
            context.Profiles.Add(new Profile
            {
                Name = "Gilson C. Souza",
                Title = "Full Stack Developer",
                Bio = "Desenvolvedor Full Stack apaixonado por tecnologia.",
                AvatarUrl = null,
                ResumeUrl = null
            });
        }

        if (context.SocialLinks.Count() == 0)
        {
            context.SocialLinks.AddRange(
                new SocialLink { Platform = "GitHub", Url = "https://github.com/GilsonSouzaDev", DisplayOrder = 1 },
                new SocialLink { Platform = "LinkedIn", Url = "https://linkedin.com/in/gilson", DisplayOrder = 2 }
            );
        }

        context.SaveChanges();
    }
}