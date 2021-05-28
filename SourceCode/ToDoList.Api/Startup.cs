using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoList.Api.Helpers;
using ToDoList.Core.Interfaces;
using ToDoList.LiteDB;
using ToDoList.LiteDB.Interfaces;
using ToDoList.LiteDB.Repository;
using Microsoft.Extensions.Options;
using ToDoList.JsonDB.Repository;

namespace ToDoList.Api
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
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();

            if (Configuration.GetValue<string>("UseDB") == "LiteDB")
            {
                services.AddSingleton<ILiteDbContext>(l => 
                    new LiteDbContext(Options.Create(new LiteDbOptions() { DatabaseLocation = Configuration["LiteDbOptions:DatabaseLocation"] })));

                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericLiteDBRepository<>));
            }
            else
            {
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericJsonDBRepository<>));
            }
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
