using Microsoft.AspNetCore.Mvc;
using Puush.Contracts.Api.Responses;
using Puush.Contracts.Api.Enums;

namespace Puush.Shared.Web;

public abstract class PuushControllerBase : ControllerBase
{
    protected static IActionResult Puush(IPuushResponse response)
        => new ContentResult
        {
            Content = response.Serialize(),
            ContentType = "text/plain",
            StatusCode = 200
        };
    
    protected static IActionResult PuushArray(ResponseCode code, IEnumerable<IPuushResponse> items)
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
