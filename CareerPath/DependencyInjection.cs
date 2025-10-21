using CareerPath.Data;
using CareerPath.Helper;
using CareerPath.Mapping;
using CareerPath.Models;
using CareerPath.Services.Abstraction;
using CareerPath.Services.Implemintation;
using CareerPath.Sittings;
using CareerPath.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerPath
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddDbContext<MyContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitWork, UnitWork>();
            services.AddScoped<IJopAppServices, JopAppServices>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<ITokenHelper, TokenHelper>();

            services.Configure<EmailSittings>(configuration.GetSection("MailSitting"));
            services.AddTransient<IEmailServices, EmailServices>();

            services.AddIdentity<UserApp, IdentityRole>(options =>
            {
                //// إعدادات الباسورد / اللوك اوت إلخ
                //options.Password.RequiredLength = 4;
                //options.SignIn.RequireConfirmedEmail = true;
                //options.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<MyContext>() // DbContext بتاعك
                .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();


            services.AddMapper();






            return services;
        }




        public static IServiceCollection AddMapper(this IServiceCollection Service)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MappingConfigurations).Assembly);

            Service.AddSingleton(config);
            //  Service.AddScoped<MapsterMapper.IMapper, ServiceMapper>();
            return Service;
        }
    }
}
