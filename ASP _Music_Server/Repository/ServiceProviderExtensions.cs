namespace ASP__Music_Server.Repository
{
    public static class ServiceProviderExtensions
    {
        public static void AddCounterService(this IServiceCollection services)
        {
            services.AddScoped<IRepository_User, Repository_User>();
            services.AddScoped<IRepository_Music, Repository_Music>();
        }
    }
}
