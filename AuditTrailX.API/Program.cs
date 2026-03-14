using AuditTrailX.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// burada dbcontexti postgres bağlantı cümlesi ile containera ekliyorum
builder.Services.AddDbContext<AuditTrailXDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// burada controller ve swagger servislerini ekliyorum
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// burada development ortamında swaggerı açıyorum
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// burada controller endpointlerini mapliyorum
app.MapControllers();

app.Run();