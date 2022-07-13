using Common.Logging;
using Discount.GRPC.Extensions;
using Discount.GRPC.MapperProfile;
using Discount.GRPC.Repositories;
using Discount.GRPC.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);
builder.Services.MigrateDatabase<Program>();
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddGrpc();


var app = builder.Build()
    ;
app.MapGrpcService<DiscountService>();
// Configure the HTTP request pipeline.
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();