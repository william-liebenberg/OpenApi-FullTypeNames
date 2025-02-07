using Scalar.AspNetCore;
using SwaggerThemes;

using WebApplication1;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add OpenAPI spec generation using the AddOpenApi extension method
builder.Services.AddOpenApi(config => 
{
    // Previously with Swashbuckle/Swagger/NSwag to support fully qualified type names for nested types in the schema,
    // use the CustomSchemaIds extension method as per:
    // https://github.com/swagger-api/swagger-ui/issues/7911#issuecomment-1068538276
    //
    //setup.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
    //

    //
    // with .NET 9 OpenApi, to support fully qualified type names for nested types in the schema, use the
    // CustomSchemaIds extension method (but from our own extension method :) )
    //
    config.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
});

WebApplication app = builder.Build();

app.UseHttpsRedirection();

// Map an endpoint for viewing the OpenAPI document in JSON format
// Link: https://localhost:7009/openapi/v1.json
const string documentName = "v1";
app.MapOpenApi("/openapi/{documentName}.json");
        
// Map an endpoint for viewing the OpenAPI document in via the Scalar UI
// Link: https://localhost:7009/scalar
app.MapScalarApiReference(o =>
{
    o.WithTheme(ScalarTheme.DeepSpace)
        .WithModels(true)
        .WithSidebar(true)
        .WithLayout(ScalarLayout.Modern)
        .WithClientButton(true)
        .WithOperationSorter(OperationSorter.Method)
        .WithDotNetFlag(true)
        .OpenApiRoutePattern = "/openapi/{documentName}.json";
});
 
// Map an endpoint for viewing the OpenAPI document in via the Swagger UI
// Link: https://localhost:7009/swagger
app.UseSwaggerUI(
    Theme.UniversalDark,
    setupAction: options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint($"/openapi/{documentName}.json", documentName);
    });

// Map a couple of endpoints to demonstrate the OpenAPI document generation
app.MapGet("/api1", ModuleA.Execute )
    .WithName("GetSomethingFromModuleA");

app.MapGet("/api2", ModuleB.Execute )
    .WithName("GetSomethingFromModuleB");

app.MapPost("/api3", ModuleC.Execute )
    .WithName("AddSomethingToModuleC");

// What does this do? :)
app.Run();