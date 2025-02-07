using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1;

internal static class ModuleC
{
    public record Request(string Description, double Price);
    public record Response(Guid Id, string Description, double Price);

    public static async Task<Results<Ok<Response>, NotFound<int>>> Execute(Request request)
    {
        var resp = new Response(Guid.NewGuid(), request.Description, request.Price);
        return await Task.FromResult(TypedResults.Ok(resp));
    }
}