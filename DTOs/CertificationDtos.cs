using PortfolioApi.Models;

namespace PortfolioApi.DTOs;

public class CertificationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class CertificationUpsertDto
{
    public string Name { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CredentialUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class CertificationMapping
{
    public static CertificationDto ToDto(this Certification c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Issuer = c.Issuer,
        IssueDate = c.IssueDate,
        ExpiryDate = c.ExpiryDate,
        CredentialUrl = c.CredentialUrl,
        ImageUrl = c.ImageUrl,
        DisplayOrder = c.DisplayOrder,
        IsActive = c.IsActive
    };

    public static void Apply(this Certification c, CertificationUpsertDto dto)
    {
        c.Name = dto.Name;
        c.Issuer = dto.Issuer;
        c.IssueDate = dto.IssueDate;
        c.ExpiryDate = dto.ExpiryDate;
        c.CredentialUrl = dto.CredentialUrl;
        c.ImageUrl = dto.ImageUrl;
        c.DisplayOrder = dto.DisplayOrder;
        c.IsActive = dto.IsActive;
    }
}
