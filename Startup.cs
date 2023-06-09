﻿using CashOut.Repository;
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
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:443", "http://localhost:8080")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddControllers();
            ScopedServices(services);
            TransientServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashOut API"); });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
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
        }

        public void TransientServices(IServiceCollection services)
        {
            services.AddTransient<SqlRepository>();
            services.AddTransient<Random>();
        }
    }
}
