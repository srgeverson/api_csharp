namespace api_csharp.API.exceptionhandler
{
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
        {
            return services.AddTransient<ApiExceptionHandler>();
        }

        public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiExceptionHandler>();
        }
    }
}
