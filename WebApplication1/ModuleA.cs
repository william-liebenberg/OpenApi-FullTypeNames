using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1;

internal static class ModuleA
{
    public record Response(int Id, string Description, double Price);

    public static async Task<Results<Ok<Response>, NotFound<int>>> Execute(int id)
    {
        var resp = new Response(1, "Thing from Module A", 100.0);
        return await Task.FromResult(TypedResults.Ok(resp));
    }
}