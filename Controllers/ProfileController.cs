using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;
using PortfolioApi.DTOs;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ProfileController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var profile = await _db.Profiles.FirstOrDefaultAsync();
        if (profile is null) return NotFound(ApiResponse<object>.Fail("Profile not set up yet."));
        return Ok(ApiResponse<ProfileDto>.Ok(profile.ToDto()));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] ProfileUpdateDto dto)
    {
        var profile = await _db.Profiles.FirstOrDefaultAsync();
        if (profile is null)
        {
            profile = new Models.Profile();
            _db.Profiles.Add(profile);
        }

        profile.Apply(dto);
        await _db.SaveChangesAsync();
        return Ok(ApiResponse<ProfileDto>.Ok(profile.ToDto(), "Profile updated."));
    }
    [HttpPost("upload-image")]
[Authorize]
public async Task<IActionResult> UploadImage(IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest(new { message = "Please select an image." });

    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

    if (!allowedExtensions.Contains(extension))
        return BadRequest(new { message = "Invalid image format." });

    if (file.Length > 5 * 1024 * 1024)
        return BadRequest(new { message = "Image must be less than 5 MB." });

    var uploadsFolder = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        "uploads",
        "profile"
    );

    Directory.CreateDirectory(uploadsFolder);

    var fileName = $"{Guid.NewGuid()}{extension}";
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var imageUrl = $"/uploads/profile/{fileName}";

    // Persist immediately so the image survives a refresh even if the
    // admin never clicks "Save Changes" after uploading.
    var profile = await _db.Profiles.FirstOrDefaultAsync();
    if (profile is null)
    {
        profile = new Models.Profile();
        _db.Profiles.Add(profile);
    }
    profile.ProfileImageUrl = imageUrl;
    profile.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();

    return Ok(new
    {
        data = new
        {
            imageUrl
        }
    });
}
[HttpPost("upload-resume")]
[Authorize]
public async Task<IActionResult> UploadResume(IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest(new { message = "Please select a resume file." });

    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

    if (extension != ".pdf")
        return BadRequest(new { message = "Only PDF resumes are allowed." });

    if (file.Length > 5 * 1024 * 1024)
        return BadRequest(new { message = "Resume must be smaller than 5 MB." });

    var uploadsFolder = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        "uploads",
        "resumes"
    );

    Directory.CreateDirectory(uploadsFolder);

    var fileName = $"{Guid.NewGuid()}.pdf";
    var filePath = Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    var resumeUrl = $"/uploads/resumes/{fileName}";

    // Persist immediately, same reasoning as the image upload above.
    var profile = await _db.Profiles.FirstOrDefaultAsync();
    if (profile is null)
    {
        profile = new Models.Profile();
        _db.Profiles.Add(profile);
    }
    profile.ResumeUrl = resumeUrl;
    profile.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();

    return Ok(new
    {
        message = "Resume uploaded successfully.",
        data = new
        {
            resumeUrl
        }
    });
}
}
