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

var builder = WebApplication.CreateBuilder(args);
var DbConnection = builder.Configuration.GetConnectionString("DemoDb");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IProcessMapper, ProcessMapper>();
builder.Services.AddScoped<IProcessRepository, ProcessRepositoty>();
builder.Services.AddScoped<IProcessService, ProcessService>();
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddProcessEndpoints();

app.Run();


