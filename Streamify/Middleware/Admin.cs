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
            if (path.StartsWith("/home"))
            {
                context.Response.Redirect("/Admin");
                return;
            }

            if (path.StartsWith("/settings") && !path.Equals("/settings/cronologia"))
            {
                context.Response.Redirect("/Admin");
                return;
            }
        }
        else if (!string.IsNullOrEmpty(emailUtente))
        {
            if (path.StartsWith("/admin"))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }
        }
        else
        {
            if (path.StartsWith("/admin") || path.StartsWith("/user") || path.StartsWith("/settings"))
            {
                context.Response.Redirect("/Account/Login");
                return;
            }
        }

        await _next(context);
    }
}
