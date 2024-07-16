using System.Globalization;

namespace API.Middlewares;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    
    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        var lang = "en";
        
        var supportedLangs = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        
        if (!string.IsNullOrWhiteSpace(requestCulture) 
            && supportedLangs.Any(c => c.Name == requestCulture))
        {
            lang = requestCulture.Split(',')[0];
        }
        
        var info = new CultureInfo(lang);
        
        CultureInfo.CurrentCulture = info;
        CultureInfo.CurrentUICulture = info;

        await _next(context);
    }
}