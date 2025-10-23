var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aktiver Swagger altid
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
    c.RoutePrefix = string.Empty; // Swagger vises på root URL
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();