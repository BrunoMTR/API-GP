using Domain.Repositories;
using Infrastructure.SQL.DB;
using Microsoft.EntityFrameworkCore;
using Presentation.Mapping;
using Presentation.Mapping.Interfaces;
using Presentation.RouteGroups;
using FluentValidation;
using Infrastructure.SQL.Repositories;
using Domain.Services;
using BL.Services;
using Asp.Versioning;
using Presentation.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Infrastructure.Logging.Configuration;
using Presentation.Middlewares;
using Domain.Channels;
using Infrastructure.Channel.Documentation;
using InfrastructureFileStorage.Interfaces;
using InfrastructureFileStorage.Services;
using BL.Handlers;
using Infrastructure.Notifications.Email;
using Infrastructure.SignalR.Documentation;



var builder = WebApplication.CreateBuilder(args);
var DbConnection = builder.Configuration.GetConnectionString("DemoDb");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IApplicationMapper, ApplicationMapper>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IFlowRepository, FlowRepository>();
builder.Services.AddScoped<IFlowService, FlowService>();
builder.Services.AddScoped<IGraphMapper, GraphMapper>();
builder.Services.AddScoped<IUnitMapper, UnitMapper>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IProcessRepository, ProcessRepository>();
builder.Services.AddScoped<IProcessMapper, ProcessMapper>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddSingleton<IDocumentationChannel, DocumentationChannel>();
builder.Services.AddHostedService<Infrastructure.Channel.Documentation.DocumentationService>();
builder.Services.AddScoped<IDocumentationRepository, DocumentationRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IDocumentationService, BL.Services.DocumentationService>();
builder.Services.AddScoped<IDocumentMapper, DocumentMapper>();
builder.Services.AddScoped<DocumentationHandler>();

builder.Services.AddHostedService<EmailService>();
builder.Services.AddSingleton<IEmailChannel, EmailChannel>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });




builder.Host.UseSerilog(LoggingConfiguration.Configure);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://super-duper-garbanzo-rvqgxq97jwv2xwx9-5173.app.github.dev") // porta do React
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // necessário para WebSockets
    });
});



builder.Services.AddSwaggerGen(options =>
{
    options.AddXmlComments();

});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
});

builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationsOptions>();


builder.Services.AddDbContextPool<DemoContext>(Options => Options.UseSqlServer(DbConnection,
    sqlServerOptionsAction: SqlCompareOptions =>
    {
        SqlCompareOptions.EnableRetryOnFailure(maxRetryCount: 3);
    }));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://login.microsoftonline.com / 136544d9 - xxxx - xxxx - xxxx - 10accb370679 / v2.0";
    options.Audience = "257b6c36-xxxx-xxxx-xxxx-6f2cd81cec43";
    options.TokenValidationParameters.ValidateLifetime = true;
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ClockSkew = TimeSpan.
   FromMinutes(5);
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCustomLogging();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DemoContext>();
    db.Database.SetConnectionString(DbConnection);
    db.Database.Migrate();
}
app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/v1.0/swagger.json", "Version 1.0");

});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

}

app.UseHttpsRedirection();


app.AddApllicatioGroup();
app.AddFlowGroup();
app.AddUnitEndpoints();



app.AddProcessEndpoints();

app.UseCors("AllowAll");
app.MapHub<DocumentationHub>("/hubs/documentation");

app.UseAuthentication();
app.UseAuthorization();





app.Run();



