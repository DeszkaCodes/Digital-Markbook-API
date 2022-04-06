using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Generators;
using System.Diagnostics;
using SchoolAPI.Services;
using SchoolAPI.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlite<SchoolContext>("Data Source=./School.db");

builder.Services.AddScoped<SchoolService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<SchoolClassService>();
builder.Services.AddScoped<GradeService>();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<SchoolContext>();
        if(context.Database.EnsureCreated())
        {
            Generator.StartGenerating(
                context,
                12000, 600, 300, 150
            );
        }
    }
}

app.Run();
