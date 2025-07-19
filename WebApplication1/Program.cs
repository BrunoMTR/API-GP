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


var builder = WebApplication.CreateBuilder(args);
var DbConnection = builder.Configuration.GetConnectionString("DemoDb");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IProcessMapper, ProcessMapper>();
builder.Services.AddScoped<IProcessRepository, ProcessRepositoty>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IApplicationMapper, ApplicationMapper>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

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

//app.AddProcessEndpoints();
app.AddApllicatioGroup();

app.Run();


