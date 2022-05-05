using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using nhsapp.sample.web.integration.Certificate;
using nhsapp.sample.web.integration.ResponseParsers;
using nhsapp.sample.web.integration.Jwt;
using nhsapp.sample.web.integration.ServiceFilter;
using nhsapp.sample.web.integration.NhsLogin;
using nhsapp.sample.web.integration.NhsLogin.Models;

namespace nhsapp.sample.web.integration
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options => options.Cookie.SecurePolicy =
                _environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always);

            services.AddControllersWithViews();

            services.AddHttpClient();

            services.AddScoped<ConfigSettingsAttribute>();

            services.AddTransient<INhsLoginService, NhsLoginService>();

            services.AddScoped<INhsLoginClient, NhsLoginClient>();

            services.AddSingleton<INhsLoginConfig, NhsLoginConfig>();

            services.AddScoped<IJsonResponseParser, JsonResponseParser>();

            services.AddSingleton<INhsLoginConfiguration, NhsLoginConfiguration>();

            services.AddSingleton<IAppWebConfiguration, AppWebConfiguration>();

            services.AddScoped<INhsLoginUriService, NhsLoginUriService>();

            services.AddHttpClient<NhsLoginHttpClient>();

            services.AddSingleton<AuthSigningConfig>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddTransient<ISigning, Signing>();

            services.AddScoped<INhsLoginJwtHelper, NhsLoginJwtHelper>();

            services.AddScoped<IJwtTokenService<IdToken>, IdTokenService>();

            services.AddScoped<ITokenValidationParameterBuilder, TokenValidationParameterBuilder>();

            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();

            services.AddScoped<INhsLoginSigningKeysProvider, NhsLoginSigningKeysProvider>();

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy =
                    _environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            // app.UsePathBase("/service-name");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
