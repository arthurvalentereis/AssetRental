using AssetRental.Domain.Interfaces.Services;
using AssetRental.Domain.Services;
using AssetRental.Infrastructure.Contexts;
using AssetRental.Infrastructure.Logs.Contexts;
using AssetRental.Infrastructure.Logs.Persistence;
using AssetRental.Infrastructure.Repositories;
using AssetRental.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Application.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

            AddDbContexts(services, configuration);
            AddDbRepositories(services, configuration);
            AddServices(services, configuration);
        }
        public static IServiceCollection AddDbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<UnitOfWork>();

            return services;
        }
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<LogMensagensPersistence>();
            services.AddTransient<MongoDBContext>();
            services.AddDbContext<AssetRentalDbContext>(x => x
                           .UseNpgsql(configuration["ConnectionStrings:AssetRental"], x => { x.CommandTimeout(60); }));
            //services.AddDbContext<ApplicationDbContext>(x => x
            //   .UseNpgsql(configuration["ConnectionStrings:Identity"], x => { x.CommandTimeout(60); }));
            services.AddTransient<DapperDataContext>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AppDb"));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.Configure<MongoDBSettings>(options => {
                options.Host = configuration.GetSection("MongoDBSettings:Host").Value;
                options.isSSL = configuration.GetSection("MongoDBSettings:isSSL").Value == "false" ? false: true;
                options.Name = configuration.GetSection("MongoDBSettings:Name").Value;
            });
            services.Configure<RabbitMQSettings>( options =>
            {
                options.Host = configuration.GetSection("RabbitMQSettings:Host").Value;
                options.Queue = configuration.GetSection("RabbitMQSettings:Queue").Value;
            });
            services.Configure<DapperSettings>( options =>
            {
                options.SqlServer = configuration.GetSection("ConnectionStrings:AssetRental").Value;
            });


            services.AddTransient<IDriverService, DriverService>();
            services.AddTransient<IMotorcycleService, MotorcycleService>();
            services.AddTransient<IRentalService, RentalService>();


            return services;
        }
    }
}
