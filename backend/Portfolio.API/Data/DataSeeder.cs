using Portfolio.API.Models;

using System.Diagnostics.CodeAnalysis;

namespace Portfolio.API.Data;

[ExcludeFromCodeCoverage]
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
                Bio = "Sou Gilson C. Souza, um profissional em transição de carreira para a área de tecnologia, formado em Análise e Desenvolvimento de Sistemas pela Fatec Mogi das Cruzes. Estou em busca de uma oportunidade em desenvolvimento de software e, para isso, dedico-me ao estudo de diversas vertentes da área, como frontend, backend, banco de dados e cloud computing.",
                ResumeUrl = "assets/docs/Gilson_Souza_Curriculo.pdf"
            });
        }
        else
        {
            var profile = context.Profiles.First();
            profile.ResumeUrl = "assets/docs/Gilson_Souza_Curriculo.pdf";
        }

        var existingLinks = context.SocialLinks.ToList();

        if (!existingLinks.Any(s => s.Platform == "GitHub"))
        {
            context.SocialLinks.Add(new SocialLink { Platform = "GitHub", Url = "https://github.com/GilsonSouzaDev", DisplayOrder = 1 });
        }
        else
        {
            var gh = existingLinks.First(s => s.Platform == "GitHub");
            gh.Url = "https://github.com/GilsonSouzaDev";
        }

        if (!existingLinks.Any(s => s.Platform == "LinkedIn"))
        {
            context.SocialLinks.Add(new SocialLink { Platform = "LinkedIn", Url = "https://www.linkedin.com/in/gilsonsouza-dev/", DisplayOrder = 2 });
        }
        else
        {
            var ln = existingLinks.First(s => s.Platform == "LinkedIn");
            ln.Url = "https://www.linkedin.com/in/gilsonsouza-dev/";
        }

        if (!existingLinks.Any(s => s.Platform == "WhatsApp"))
        {
            context.SocialLinks.Add(new SocialLink { Platform = "WhatsApp", Url = "https://wa.me/5511947799976", DisplayOrder = 3 });
        }
        else
        {
            var wa = existingLinks.First(s => s.Platform == "WhatsApp");
            wa.Url = "https://wa.me/5511947799976";
        }

        context.SaveChanges();
    }
}