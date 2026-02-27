using Microsoft.AspNetCore.Mvc;
using Puush.Contracts.Api.Responses;
using Puush.Contracts.Api.Responses.Enums;
using Puush.Infrastructure.Security.Attributes;
using Puush.Persistence.Models.Enums;
using Puush.Shared.Web;

namespace Puush.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : PuushControllerBase
{
    // TODO: Handle api key authentication through an attribute or middleware
    [HttpPost("auth")]
    public IActionResult Auth([FromForm(Name = "p")] string? password, [FromForm(Name = "k")] string? apiKey)
    {
        // return Ok("0,TEST,09-19-2025,5000000");
        
        return Puush(new AuthResponse
        {
            AccountType = AccountType.Haxor,
            ApiKey = "TEST",
            ExpirationDate = new DateTime(2025, 9, 19),
            Usage = 5000000
        });
    }
    
    [PuushAuthorize]
    [HttpPost("hist")]
    public IActionResult History()
    {
        // return Ok("0\n" +
        //           "1,09-19-2025,http://localhost:5168/ABCD,ABCD.png,67");

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
        // return Ok("0,http://localhost:5168/ABCD,ABCD.png,5000000");
        
        return Puush(new UploadResponse
        {
            Code = ResponseCode.Success,
            FileName = "http://localhost:5168/ABCD",
            Url = "ABCD.png",
            Usage = 5000000
        });
    }
}
