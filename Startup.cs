using CashOut.Repository;
using CashOut.Repository.Interfaces;
using CashOut.Services;
using CashOut.Services.Interfaces;
using System.Text.Json.Serialization;

namespace CashOut
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddMvc();
            services.AddSwaggerGen();
            services.AddControllers();
            ScopedServices(services);
            TransientServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashOut API"); });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ScopedServices(IServiceCollection services)
        {
            //Repository
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICashOutRepository, CashOutRepository>();

            //Services
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICashOutService, CashOutService>();

            services.AddTransient<SqlRepository>();
        }

        public void TransientServices(IServiceCollection services)
        {
            services.AddTransient<Random>();
        }
    }
}
