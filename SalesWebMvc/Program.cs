using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
namespace SalesWebMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("SalesWebMvcContext"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMvcContext")),
                    mySqlOptions =>
                        mySqlOptions.MigrationsAssembly("SalesWebMvc")));

            builder.Services.AddScoped<SeedingService>(); //registra o servico para rodar uma vez por escopo

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using (var scope = app.Services.CreateScope()) // Cria um escopo em Services para recuperar SeedingService
                {

                    try
                    {
                        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>(); // Acessa os registros do escopo (service provider) e recupera a instancia de SeedingService
                        seedingService.Seed();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao executar o seeding: {ex.Message}");
                    }
                }
            }
            // Configure the HTTP request pipeline.
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
