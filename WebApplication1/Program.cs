using Domain.Repositories;
using Infrastructure.SQL.DB;
using Microsoft.EntityFrameworkCore;
using Presentation.Mapping;
using Presentation.Mapping.Interfaces;
using Presentation.RouteGroups;
using System.Data.Common;
using System.Data.SqlTypes;
using FluentValidation;
using Infrastructure.SQL.Repositories;
using Domain.Services;
using BL.Services;
using Microsoft.AspNetCore.Builder;
using Asp.Versioning;
using Presentation.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.Run();


