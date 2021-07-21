using Infrastructure.Model;
using Infrastructure.Repository;
using Infrastructure.Repository.Interfaces;
using Infrastructure.UnitOfWork;
using Infrastructure.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.ApplicationService;
using Service.ApplicationService.Interfaces;

namespace Interface
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
            services.AddControllers();
            services.AddScoped<IPessoaApplicationService, PessoaAplicationService>();
            services.AddScoped<IDev2bUnitOfWork, Dev2bUnitOfWork>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddDbContext<Dev2bDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Dev2bConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Dev2bDbContext dev2BDbContext)
        {
            //Ou usa esses 2 comandos ou usa Migrations, os 2 juntos da conflito
            //Esses comandos serve pra apagar e sempre criar um novo banco de dados com o estado atual dos mapeamentos
            //dev2BDbContext.Database.EnsureDeleted();
            //dev2BDbContext.Database.EnsureCreated();

            //Aplica as Migrations no startup do projeto
            dev2BDbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
