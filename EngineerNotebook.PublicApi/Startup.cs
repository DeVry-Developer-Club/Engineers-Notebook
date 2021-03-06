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
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Driver;
using AspNetCore.Identity.MongoDbCore.Models;

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

            services.AddGrpc();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseDbService<>));
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimsService>();
            
            services.AddSingleton<IHtmlToPdfConverter, HtmlToPdfConverter>();
            services.AddSingleton<IRazorToString, RazorToString>();
            
            services.AddRazorTemplating();
            RazorTemplateEngine.Initialize();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
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
                        builder.WithOrigins(baseUrlConfig.WebBase
                            .Replace("host.docker.internal", "localhost")
                            .TrimEnd('/'));
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.WithExposedHeaders("Grpc-Status",
                            "Grpc-Message",
                            "Grpc-Encoding",
                            "Grpc-Accept-Encoding");
                    });
            });
            
            #endregion
            
            services.AddMediatR(typeof(Shared.Models.Tag).Assembly);
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
            app.UseAuthorization();

            app.UseGrpcWeb();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", () => "Running");
                endpoints.MapGet("/health", () => "ok");

                endpoints.MapGrpcService<AuthenticationService>().EnableGrpcWeb().RequireCors(CORS_POLICY);
                endpoints.MapGrpcService<DocumentationService>().EnableGrpcWeb().RequireCors(CORS_POLICY);
                endpoints.MapGrpcService<TagService>().EnableGrpcWeb().RequireCors(CORS_POLICY);
                endpoints.MapGrpcService<GuideService>().EnableGrpcWeb().RequireCors(CORS_POLICY);
            });
        }
    }
}