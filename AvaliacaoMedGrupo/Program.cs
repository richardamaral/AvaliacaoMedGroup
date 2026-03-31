using System.Text.Json.Serialization;
using AvaliacaoMedGrupo.Data;
using AvaliacaoMedGrupo.Repositories;
using AvaliacaoMedGrupo.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// configuro os controllers pra aceitar enum como string no json (ex: "Masculino" em vez de 1)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configuro o entity framework pra usar sql server com a connection string do appsettings
builder.Services.AddDbContext<AvaliacaoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// registro as dependencias - quando alguem pedir a interface, o framework entrega a implementacao
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IContatoService, ContatoService>();

var app = builder.Build();

// swagger so aparece em desenvolvimento, em producao nao precisa
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }
