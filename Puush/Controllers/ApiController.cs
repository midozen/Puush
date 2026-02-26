using Microsoft.AspNetCore.Mvc;

namespace Puush.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    // TODO: Handle api key authentication through an attribute or middleware
    [HttpPost("auth")]
    public IActionResult Auth([FromForm(Name = "p")] string? password, [FromForm(Name = "k")] string? apiKey)
    {
        /*
         * AUTH RESPONSE:
         * Account Type (enum)
         * Api Key (string)
         * Expiration Date (string)
         * Usage (long) num of bytes uploaded
         */
        return Ok("0,TEST,09-19-2025,5000000");
    }
    
    // -2 or -1 warrants a break in the response parser
    [HttpPost("hist")]
    public IActionResult History([FromForm(Name = "k")] string apiKey)
    {
        /*
         * RESPONSE CODE:
         * 0: Success
         * -1: N/A
         * -2: N/A
         */
        
        /*
         * RECENT UPLOAD ARRAY
         * ID (int)
         * Upload Date (string)
         * Full URL Path (string)
         * File Name With Extension (string)
         * View Count (int)
         */
        return Ok("0\n" +
                  "1,09-19-2025,http://localhost:5168/ABCD,ABCD.png,67");
    }
    
    [HttpPost("thumb")]
    public IActionResult Thumbnail([FromForm(Name = "k")] string apiKey, [FromForm(Name = "i")] int id)
    {
        // EXPECT: 200 with image data, 404 if not found
        return NotFound();
    }
    
    [HttpPost("uo")]
    public IActionResult UploadImage([FromForm(Name = "k")] string apiKey, [FromForm(Name = "c")] IFormFile file)
    {
        /*
         * RESPONSE CODE:
         * 0: Success
         * -1: Authentication Failure
         * -2: N/A
         * -3: Checksum error
         * -4: Insufficent storage.
         */
        
        /*
         * AUTH RESPONSE:
         * Response Code (int)
         * Full URL Path (string)
         * File Name With Extension (string)
         * Usage (long) num of bytes uploaded
         */
        return Ok("0,http://localhost:5168/ABCD,ABCD.png,5000000");
    }
}