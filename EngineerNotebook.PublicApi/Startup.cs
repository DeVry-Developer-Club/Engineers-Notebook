using System.Text;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Razor.Templating.Core;
using AspNetCore.Identity.MongoDbCore.Models;
using EngineerNotebook.Bot;
using EngineerNotebook.PublicApi.Endpoints;

namespace EngineerNotebook.PublicApi
{
    public class Startup
    {
        private const string CORS_POLICY = "CorsPolicy";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        ConnectionString GetConnectionString(IConfigurationSection section)
        {
            return new ConnectionString(
                section.GetValue<string>("Host"),
                section.GetValue<int>("Port"),
                section.GetValue<string>("DatabaseName"),
                section.GetValue<string>("Username"),
                section.GetValue<string>("Password"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConnectionString>(Configuration.GetSection("Database"));
            services.AddSingleton<IDatabaseOptions>(x => x.GetRequiredService<IOptions<ConnectionString>>().Value);

            var idCon = GetConnectionString(Configuration.GetSection("Database"));

            services.AddIdentity<ClubMember, MongoIdentityRole<Guid>>()
                .AddMongoDbStores<ClubMember, MongoIdentityRole<Guid>, Guid>(idCon.FullConnectionString, idCon.DatabaseName)
                .AddDefaultTokenProviders();

            #region Utilities

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseDbService<>));
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimsService>();
            
            services.AddSingleton<IHtmlToPdfConverter, HtmlToPdfConverter>();
            services.AddSingleton<IRazorToString, RazorToString>();
            
            services.AddRazorTemplating();
            RazorTemplateEngine.Initialize();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddScoped<IGuideService, GuideService>();
            
            if(!Configuration.GetValue<bool>("DisableBot"))
                services.AddEngineeringBot();
            
            #endregion

            var baseUrlConfig = new BaseUrlConfiguration();
            Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);

            services.AddMemoryCache();

            #region Authentication

            var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });
            services.AddAuthorization();
            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                    builder =>
                    {
                        //builder.WithOrigins(baseUrlConfig.WebBase
                        //    .Replace("host.docker.internal", "localhost")
                        //    .TrimEnd('/'));
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
            
            #endregion
            
            services.AddMediatR(typeof(Tag).Assembly);
            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(CORS_POLICY);
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", () => "Running");
                endpoints.MapGet("/health", () => "ok");

                endpoints.AddAuthEndpoints();
                endpoints.AddDocEndpoints();
                endpoints.AddTagEndpoints();
                endpoints.AddGuideEndpoints();
            });
        }
    }
}