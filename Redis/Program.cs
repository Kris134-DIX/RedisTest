

using Microsoft.EntityFrameworkCore;
using Redis.Database;
using Redis.Repositories;
using Redis.Services;
using Redis.Services.Caching;
using StackExchange.Redis;

namespace Redis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.AddDbContext<PlatformaDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformaDb")));



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            builder.Services.AddScoped<ISubFundUnitPriceRepository, SubFundUnitPriceRepository>();
            builder.Services.AddScoped<ISubFundUnitPriceService, SubFundUnitPriceService>();
            builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
            builder.Services.AddScoped<IRedisCache, RedisCache>();
            builder.Services.AddScoped<IRedisService, RedisService>();

            //var multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            var multiplexer = ConnectionMultiplexer.Connect("10.17.102.93:6379");
            builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

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

            app.Run();
        }
    }
}
