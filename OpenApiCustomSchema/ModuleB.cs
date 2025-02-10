using Microsoft.AspNetCore.Http.HttpResults;

namespace OpenApiCustomSchema;

internal static class ModuleB
{
    public record Response(Guid Id, string Description, double Price);

    public static async Task<Results<Ok<Response>, NotFound<int>>> Execute(int id)
    {
        var resp = new Response(Guid.NewGuid(), "Thing from Module B", 200.0);
        return await Task.FromResult(TypedResults.Ok(resp));
    }
}