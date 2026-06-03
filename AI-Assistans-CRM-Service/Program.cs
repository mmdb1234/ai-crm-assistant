
using FluentValidation;
using Scalar.AspNetCore;
using Infrastructure.AI_Assistans;
using Features.AI_Assistans;
using AI_Assistans_CRM_Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Features
builder.Services.AddFeatures();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment()) 
{ 
    app.MapOpenApi(); 
    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();

// Endpoints
app.MapConversationEndpoints();
app.MapMessageEndpoints();
app.MapUsersEndpoints();

app.Run();

