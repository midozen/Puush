using Microsoft.AspNetCore.Mvc;
using Puush.Contracts.Api.Responses;
using Puush.Contracts.Api.Enums;

namespace Puush.Shared.Web;

public abstract class PuushControllerBase : ControllerBase
{
    /// <summary>
    /// Creates a ContentResult with the specified ResponseCode as the content, formatted as plain text, and a status code of 200.
    /// </summary>
    /// <param name="code">The ResponseCode to include in the response content.</param>
    /// <returns>A ContentResult containing the ResponseCode as plain text.</returns>
    protected static IActionResult PuushCode(ResponseCode code)
        => new ContentResult
        {
            Content = ((int)code).ToString(),
            ContentType = "text/plain",
            StatusCode = 200
        };
    
    /// <summary>
    /// Serializes an IPuushResponse to the format expected by the client and returns it as a ContentResult with the appropriate content type and status code.
    /// </summary>
    /// <param name="response">The response to serialize and return.</param>
    /// <returns>A ContentResult containing the serialized response.</returns>
    protected static IActionResult Puush(IPuushResponse response)
        => new ContentResult
        {
            Content = response.Serialize(),
            ContentType = "text/plain",
            StatusCode = 200
        };

    /// <summary>
    /// Serializes a collection of IPuushResponses to the format expected by the client and returns it as a ContentResult with the appropriate content type and status code.
    /// The first line of the response contains the response code, followed by each serialized item on a new line.
    /// </summary>
    /// <param name="code">The response code to include at the beginning of the response.</param>
    /// <param name="items">The collection of items to serialize and include in the response.</param>
    /// <returns>A ContentResult containing the serialized response.</returns>
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
