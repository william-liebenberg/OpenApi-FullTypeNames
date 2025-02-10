using Microsoft.AspNetCore.OpenApi;

namespace OpenApiCustomSchema;

public static class OpenApiExtensions
{
    public static OpenApiOptions CustomSchemaIds(this OpenApiOptions config,
        Func<Type, string?> typeSchemaTransformer,
        bool includeValueTypes = false)
    {
        return config.AddSchemaTransformer((schema, context, _) =>
        {
            // Skip value types and strings
            if (!includeValueTypes && 
                (context.JsonTypeInfo.Type.IsValueType || 
                 context.JsonTypeInfo.Type == typeof(String) || 
                 context.JsonTypeInfo.Type == typeof(string)))
            {
                return Task.CompletedTask;
            }

            // Skip if the schema ID is not already set because we don't want to decorate the schema multiple times
            if (schema.Annotations == null || !schema.Annotations.TryGetValue("x-schema-id", out object? _))
            {
                return Task.CompletedTask;
            }
                    
            // transform the typename based on the provided delegate
            string? transformedTypeName = typeSchemaTransformer(context.JsonTypeInfo.Type);

            // Scalar - decorate the models section
            schema.Annotations["x-schema-id"] = transformedTypeName;

            // Swagger and Scalar specific:
            // for Scalar - decorate the endpoint section
            // for Swagger - decorate the endpoint and model sections
            schema.Title = transformedTypeName;

            return Task.CompletedTask;
        });           
    }
}