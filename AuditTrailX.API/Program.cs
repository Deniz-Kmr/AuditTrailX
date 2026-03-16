using AuditTrailX.API.Extensions;
using AuditTrailX.API.Middleware;
using AuditTrailX.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithAuth();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy =>
    {
        // sadece local dashboarda izin veriyorum
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// middleware sırasına dikkat edelim...
// önce hata yakalama sonra correlation id sonra auth gelmeli
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWeb");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// uygulama ayağa kalkarken seed data varsa ekliyorum
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuditTrailXDbContext>();
    await DataSeeder.SeedAsync(db);
}

app.Run();