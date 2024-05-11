using Microsoft.Extensions.DependencyInjection;
using BeireMKit.Data.Interfaces;
using BeireMKit.Data.Repositories;
using BeireMKit.Data.Contexts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeireMKit.Data.Extensions
{
    public static class ServiceCollections
    {
        public static IServiceCollection ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        public static IServiceCollection ConfigureDbContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped(typeof(IBaseDbContext), typeof(BaseDbContext<TContext>));
            return services;
        }

        public static IServiceCollection ConfigureIdentityDbContext<TContext, TUser>(this IServiceCollection services)
            where TContext : IdentityDbContext<TUser>
            where TUser : IdentityUser
        {
            services.AddScoped(typeof(IBaseDbContext), typeof(BaseDbContextIndentity<TContext, TUser>));
            return services;
        }
    }
}
