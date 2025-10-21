using AutoMapper;
using Microsoft.OpenApi.Models;
using Valoriza.Application.Mapping;
using Valoriza.Application.Services;
using Valoriza.Infrastructure;
using Valoriza.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddValorizaDb(builder.Configuration);


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());


builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<IntegrationService>();


builder.Services.AddHttpClient("brasilapi", c =>
{
    c.BaseAddress = new Uri("https://brasilapi.com.br/api/");
});


builder.Services.AddHttpClient("openai", c =>
{
    c.BaseAddress = new Uri("https://api.openai.com/v1/");
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Valoriza API",
        Version = "v1",
        Description = "Educação financeira + IA comportamental (Open Finance ready)"
    });
    c.EnableAnnotations();
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();


app.Run();