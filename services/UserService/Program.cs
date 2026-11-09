using UserService.Data;
using UserService.Repositories;
using UserService.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<JwtService>();

builder.Services.AddSingleton<DapperContext>();

builder.Services.AddScoped<IUserRepository,
                           UserRepository>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// app.MapGet("/", () => "User Service Running");

app.Run();