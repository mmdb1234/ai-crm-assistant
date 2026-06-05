
using FluentValidation;
using Infrastructure.AI_Assistans;
using Features.AI_Assistans;
using AI_Assistans_CRM_Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Features
builder.Services.AddFeatures();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});



var app = builder.Build();
app.UseCors("frontend");
// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoints
app.MapConversationEndpoints();
app.MapMessageEndpoints();
app.MapUsersEndpoints();

app.Run();

