namespace AuditTrailX.API.Middleware;

public class CorrelationIdMiddleware
{
    
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    
    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        
        // her isteğe bir correlation id attım
        // istek dışardan id getirmediyse kendim üreteceğim
        // bu id sayesinde bir istegi loglar arasında takip edebiliceğim
        if(!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            correlationId = Guid.NewGuid().ToString();
        
        // correlation idyi hem contexte hem de response headerına ekledim
        context.Items[CorrelationIdHeader] = correlationId.ToString();
        context.Response.Headers[CorrelationIdHeader] = correlationId.ToString();
        
        await _next(context);
    }
}