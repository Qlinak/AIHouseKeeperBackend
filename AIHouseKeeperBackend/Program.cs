using AIHouseKeeperBackend;
using AIHouseKeeperBackend.AuthorisationDomain.Middlewares;
using AIHouseKeeperBackend.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Reuse appDbContext upon multiple requests
builder.Services.AddPooledDbContextFactory<AppDbContext>(
    options => options
        .UseNpgsql(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            b =>
            {
                b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }
        )
);
builder.Services.AddScoped(p =>
    p.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());
    
Modules.BuildServices(builder.Services);
Modules.BuildConfigs(builder.Services);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();});
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "For AIHouseKeeper Developers only", Version = "v1" });
    
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    };
    
    opt.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme, 
                    Id = "Bearer" 
                }
            },
            new string[] { }
        }
    };
    opt.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseMiddleware<JwtMiddleware>();
app.Run();