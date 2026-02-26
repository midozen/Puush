using Microsoft.AspNetCore.Mvc;
using Puush.Models.API;
using Puush.Models.API.Enums;

namespace Puush.Controllers;

public abstract class PuushController : ControllerBase
{
    protected static IActionResult Puush(PuushResponse response)
        => new ContentResult
        {
            Content = response.Serialize(),
            ContentType = "text/plain",
            StatusCode = 200
        };
    
    protected static IActionResult PuushArray(ResponseCode code, IEnumerable<PuushResponse> items)
    {
        var content = $"{(int)code}\n{string.Join("\n", items.Select(i => i.Serialize()))}";

        return new ContentResult
        {
            Content = content,
            ContentType = "text/plain",
            StatusCode = 200
        };
    }
}