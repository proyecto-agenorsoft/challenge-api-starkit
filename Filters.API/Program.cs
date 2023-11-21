using Filters.Contracts;
using Filters.Infrastructure;
using Filters.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IGetterAllRepository, GetterAllRepository>();
builder.Services.AddScoped<IGetterByFiltersService, GetterByFiltersService>();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar logging 
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware para logging de requests y responses si es necesario
app.Use(async (context, next) =>
{
    // Logging del request
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Handling request: " + context.Request.Path);

    await next.Invoke();

    // Logging de la response puede ser añadido aquí si es necesario
    logger.LogInformation("Finished handling request.");
});

app.UseAuthorization();
app.MapControllers();

app.Run();
