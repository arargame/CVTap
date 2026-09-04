using CVTap.Shared.Models;

namespace CVTap.Shared.Services.Implementations;

public static class DefaultTemplates
{
    public static List<EmailTemplate> GetDefaults()
    {
        return new List<EmailTemplate>
        {
            new EmailTemplate
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Job Application (EN)",
                Language = "EN",
                IsDefault = true,
                SubjectTemplate = "Application for {Position} - {Name}",
                BodyTemplate = "Hello,\n\nI would like to apply for the {Position} position at {Company}.\nPlease find my CV attached for your review.\n\nI look forward to the opportunity to discuss how my background fits your team.\n\nBest regards,\n{Name}\n{Phone}\n{LinkedIn}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EmailTemplate
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "İş Başvurusu (TR)",
                Language = "TR",
                IsDefault = false,
                SubjectTemplate = "{Position} Başvurusu - {Name}",
                BodyTemplate = "Merhaba,\n\n{Company} bünyesindeki {Position} pozisyonuna başvurmak istiyorum. Güncel özgeçmişimi ekte bilgilerinize sunarım.\n\nYetkinliklerimi ve deneyimlerimi detaylandırmak üzere sizinle görüşmekten memnuniyet duyarım.\n\nSaygılarımla,\n{Name}\n{Phone}\n{LinkedIn}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EmailTemplate
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Quick Application (EN)",
                Language = "EN",
                IsDefault = false,
                SubjectTemplate = "CV / Resume - {Name} - {Position}",
                BodyTemplate = "Hi,\n\nPlease find attached my CV for the {Position} role at {Company}.\nFeel free to reach out if you need any further information.\n\nThanks,\n{Name}\n{Phone}\n{Email}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EmailTemplate
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Title = "Recruiter Follow-up (EN)",
                Language = "EN",
                IsDefault = false,
                SubjectTemplate = "Follow-up: {Position} Application - {Name}",
                BodyTemplate = "Hello,\n\nI hope you are having a great week.\nI am following up regarding my recent application for the {Position} role at {Company}. I have re-attached my CV here for quick reference.\n\nThank you for your consideration.\n\nBest regards,\n{Name}\n{LinkedIn}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
    }
}
