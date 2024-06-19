using Turnos.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register servicios
builder.Services.AddSingleton<TransaccionesData>();
builder.Services.AddSingleton<ClientesData>();
builder.Services.AddSingleton<AgenciasData>();
builder.Services.AddSingleton<MotivoTransaccionData>();
builder.Services.AddSingleton<TipoTransaccionData>();
builder.Services.AddSingleton<UsuariosData>();
builder.Services.AddSingleton<TipoClienteData>();
builder.Services.AddSingleton<CanalServicioData>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NuevaPolitica");
app.UseAuthorization();

app.MapControllers();

app.Run();