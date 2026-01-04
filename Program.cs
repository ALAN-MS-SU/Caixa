using Microsoft.EntityFrameworkCore;
using CaixaAPI.DB;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});;

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
    )
);

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();
}
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthorization();


app.Run();
