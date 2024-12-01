using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class amministratore
{
    private readonly RequestDelegate _next;

    public amministratore(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString().ToLower();
        var emailUtente = context.Session.GetString("EmailUtente");

        if (!string.IsNullOrEmpty(emailUtente) && emailUtente == "admin@admin.com")
        {
            if (path.StartsWith("/settings") && !path.Equals("/settings/cronologia"))
            {
                context.Response.Redirect("/Admin");
                return;
            }
        }
        else
        {
            if (path.StartsWith("/admin") || path.StartsWith("/user"))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }
        }

        if(path.StartsWith("/user"))
        {
            context.Response.Redirect("/Admin");
        }

        await _next(context);
    }
}
