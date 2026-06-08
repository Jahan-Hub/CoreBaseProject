using CoreBaseProject.Api.Extensions;
using CoreBaseProject.Application.ApplicationLogic.DatabaseSeedConfiguration.command;
using CoreBaseProject.Application.Configurations;
using CoreBaseProject.Shared.Exceptions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//Add Application Services
builder.Services.AddApplicationService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add identity handler services and configure identity services
builder.Services.AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .AddIdentityHandlersAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomResponseCompression();

var app = builder.Build();

builder.Services.AddResponseCaching();
app.UseResponseCaching();

// Enable forwarded headers for IP Address
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerExplorer();

    using (var scope = app.Services.CreateScope())
    {
        var databaseSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeederConfiguration>();
        await databaseSeeder.SeedAsync();
    }
}

// Register job scheduler
app.Services.ConfigureRecurringJobs();

// Configuration CORS
app.ConfigureCors(builder.Configuration)
   .AddIdentityAuthMiddlewares();

// Enables serving static files from wwwroot
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use exception handler 
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.UseAuthorization();

app.MapGroup("/api");

//app.MapHub<NotificationHub>("/hubs/notification");

app.MapControllers();

app.Run();
