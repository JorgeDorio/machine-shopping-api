namespace Machine.Shopping.Api;

public class Middleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value != null && (context.Request.Path.Value.Contains("login") ||
                                                   context.Request.Path.Value.Contains("register")))
        {
            await next(context);
        }

        // Captura o valor do cabeçalho Authorization
        if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            // Exemplo de uso: logar ou armazenar o token
            Console.WriteLine($"Authorization: {authorizationHeader}");
        }
    }
}