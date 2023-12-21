using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using SecretGiftExchange.API;
using SecretGiftExchange.Services;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
/////////////////////////////////////
// Add ParticipantService to the DI container
builder.Services.AddSingleton<ParticipantService>();

builder.Services.AddSingleton<ParticipantService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new ParticipantService(configuration);
});

builder.Services.AddSwaggerGen(c =>
{
    // Other Swagger configuration...

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Secret Gift Exchange Service", Version = "v1" });
    c.OperationFilter<IncludeXmlCommentsWithExceptionsOperationFilter>(xmlPath);
});

///////////////////////////////

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
  
///////////////////
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Secret Gift Exchange Service");
});

app.UseMiddleware<ErrorHandlingMiddleware>();
//////////////////


app.Run();
