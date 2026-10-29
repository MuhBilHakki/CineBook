using System.Net;

namespace CineBook.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

                await ctx.Response.WriteAsJsonAsync(new
                {
                    status = "error",
                    message = "Terjadi kesalahan server."
                });
            }
        }
    }
}
