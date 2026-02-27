using Microsoft.AspNetCore.Mvc;
using Puush.Contracts.Api.Responses;
using Puush.Contracts.Api.Enums;
using Puush.Infrastructure.Security.Attributes;
using Puush.Infrastructure.Services;
using Puush.Shared.Web;

namespace Puush.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController(IAuthService authService) : PuushControllerBase
{
    [HttpPost("auth")]
    public async Task<IActionResult> Auth(
        [FromForm(Name = "e")] string username,
        [FromForm(Name = "p")] string? password,
        [FromForm(Name = "k")] string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey) && string.IsNullOrEmpty(password))
            return PuushCode(ResponseCode.AuthenticationFailure);

        var response = await authService.AuthenticateAsync(username, password, apiKey);
        return response is null ? PuushCode(ResponseCode.AuthenticationFailure) : Puush(response);
    }
    
    [PuushAuthorize]
    [HttpPost("hist")]
    public IActionResult History()
    {
        return PuushArray(ResponseCode.Success, [
            new RecentUpload
            {
                Id = 1,
                UploadDate = new DateTime(2025, 9, 19),
                Url = "http://localhost:5168/ABCD",
                FileName = "ABCD.png",
                ViewCount = 67
            },
            new RecentUpload
            {
                Id = 2,
                UploadDate = new DateTime(2025, 9, 19),
                Url = "http://localhost:5168/ABCDE",
                FileName = "ABCDE.jpg",
                ViewCount = 54
            },
            new RecentUpload
            {
                Id = 3,
                UploadDate = new DateTime(2025, 9, 19),
                Url = "http://localhost:5168/ABCDE",
                FileName = "TEST.zip",
                ViewCount = 32
            },
        ]);
    }
    
    [PuushAuthorize]
    [HttpPost("thumb")]
    public IActionResult Thumbnail([FromForm(Name = "i")] int id)
    {
        // EXPECT: 200 with image data, 404 if not found
        return NotFound();
    }
    
    [PuushAuthorize]
    [HttpPost("up")]
    public IActionResult UploadImage([FromForm(Name = "f")] IFormFile file)
    {
        return Puush(new UploadResponse
        {
            Code = ResponseCode.Success,
            FileName = "http://localhost:5168/ABCD",
            Url = "ABCD.png",
            Usage = 5000000
        });
    }
}
