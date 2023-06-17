using UNDEFINED.Areas.Admin.Services;

namespace UNDEFINED.Areas
{
    public static class AdminStartup
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }

}
