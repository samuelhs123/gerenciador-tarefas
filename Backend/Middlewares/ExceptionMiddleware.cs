using System.Net;
using GerenciadorTarefas.Util;

namespace GerenciadorTarefas.Middlewares;

public static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new RespostaAPI(false, "Ocorreu um erro, tente novamente mais tarde."));
            });
        });
    }
}