using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using PostOffice.Repository.Helpers;
using PostOffice.Repository.Repositories;
using PostOffice.Service.Services;
using PostOffice.Common.Exceptions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    //var databaseConnectionString = Configuration.GetConnectionString("WebApiDatabase");
    services.AddDbContext<DataContext>(options =>
                options.UseSqlServer("Server=SHIRINLENOVO;Database=PostOffice;Trusted_Connection=True;MultipleActiveResultSets=true"));
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure DI for application services
    services.AddScoped<IShipmentService, ShipmentService>();
    services.AddScoped<IBagService, BagService>();
    services.AddScoped<IParcelService, ParcelService>();

    services.AddScoped<IShipmentRepository, ShipmentRepository>();
    services.AddScoped<IBagRepository, BagRepository>();
    services.AddScoped<IParcelRepository, ParcelRepository>();
}

var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    //// global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");