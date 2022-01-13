using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Interfaces;
using Domain.UseCases;
using Gateway;
using Web.Presenters;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICreateUserUseCase, CreateUserUseCase>();
            services.AddTransient<ICreateOtpUseCase, CreateOtpUseCase>();
            services.AddTransient<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddTransient<IGetUserDetailsUseCase, GetUserDetailsUseCase>();
            services.AddTransient<IResetPasswordUseCase, ResetPasswordUseCase>();
            services.AddTransient<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddTransient<IUserGateway, UserGateway>();
            services.AddTransient<IMessageGateway, MessageGateway>();
            services.AddTransient<IActionResultPresenter, RestPresenter>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
