using ActionCommandGame.Api.Installers.Extensions;
using ActionCommandGame.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Registreer CORS‑service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:7138")    // Blazor‑origin
            .AllowAnyHeader()                         
            .AllowAnyMethod()                         // allow GET/POST/etc
            .AllowCredentials();                      // allow cookies/auth/etc
    });
});

// Install services to the container using IInstaller classes.
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

//Initialize dbContext data
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ActionCommandGameDbContext>();
if (dbContext.Database.IsInMemory())
{
    await dbContext.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Action Command Game API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
