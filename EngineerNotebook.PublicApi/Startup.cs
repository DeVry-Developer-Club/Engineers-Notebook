using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;
using EngineerNotebook.Core.Constants;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Infrastructure.Data;
using EngineerNotebook.Infrastructure.Identity;
using EngineerNotebook.Infrastructure.Utility;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Razor.Templating.Core;
using System.IO;
using System.Threading.Tasks;

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
                section.GetValue<string>("Server"),
                section.GetValue<int>("Port"),
                section.GetValue<string>("Database"),
                section.GetValue<string>("Username"),
                section.GetValue<string>("Password"));
        }

        /// <summary>
        /// Ensures the provided configuration for a MySql database exists
        /// - Creates database if it does not exists
        /// - Executes the migration script to ensure schema exists at runtime
        /// </summary>
        /// <param name="configSectionPath"></param>
        /// <param name="sqlScriptPath"></param>
        async Task EnsureDatabaseExists(string configSectionPath, string sqlScriptPath)
        {
            Console.WriteLine($"Making sure {configSectionPath} database exists using {sqlScriptPath}");
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                var config = GetConnectionString(Configuration.GetSection(configSectionPath));
                using var connection = new MySqlConnection(config.FullConnectionString);

                Console.WriteLine(config.FullConnectionString);

                await connection.OpenAsync();

                var sqlScript = File.ReadAllText(sqlScriptPath);
                var createDatabase = new MySqlCommand(sqlScript, connection);
                await createDatabase.ExecuteNonQueryAsync();
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"Was unable to apply migrations for {configSectionPath}:\n\t{ex.Message}");
                Environment.Exit(1);
            }
        }        

        private void ApplyMigrations()
        {

            if (Environment.GetEnvironmentVariable("APPLY_MIGRATIONS")
                ?.Equals("false", StringComparison.OrdinalIgnoreCase) ?? true)
                return;

            EnsureDatabaseExists("Databases:Engineer", Path.Join("sql", "engineer.sql")).GetAwaiter().GetResult();
            EnsureDatabaseExists("Databases:Identity", Path.Join("sql", "identity.sql")).GetAwaiter().GetResult();
        }

        private void ConfigureInMemoryDatabases(IServiceCollection services)
        {            
            services.AddDbContext<EngineerDbContext>(c => c.UseInMemoryDatabase("EngineerContext"));
            services.AddDbContext<AppIdentityDbContext>(c => c.UseInMemoryDatabase("Identity"));
        }
        
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // use in-memory database
            //ConfigureInMemoryDatabases(services);
            ConfigureMySqlDatabase(services);
            ConfigureServices(services);
        }
            
        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureMySqlDatabase(services);
            ConfigureServices(services);
        }
        
        private void ConfigureMySqlDatabase(IServiceCollection services)
        {
            ApplyMigrations();

            var engCon = GetConnectionString(Configuration.GetSection("Databases:Engineer"));
            var idCon = GetConnectionString(Configuration.GetSection("Databases:Identity"));

            Console.WriteLine($"The engineer connection string is:\n\t{engCon.FullConnectionString}");
            Console.WriteLine($"The identity connection string is:\n\t{idCon.FullConnectionString}");

            services.AddDbContext<EngineerDbContext>(c =>
                c.UseMySQL(engCon.FullConnectionString, 
                    x=>x.MigrationsAssembly(GetType().Assembly.FullName)));
            
            services.AddDbContext<AppIdentityDbContext>(c =>
                c.UseMySQL(idCon.FullConnectionString,
                    x=>x.MigrationsAssembly(GetType().Assembly.FullName)));
        }

        public void ConfigureTestingServices(IServiceCollection services)
        {
            ConfigureInMemoryDatabases(services);
            ConfigureServices(services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            #region Utilities

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
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
                    });
            });
            
            #endregion
            
            services.AddControllers();
            services.AddMediatR(typeof(Tag).Assembly);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Engineer Notebook API", Version = "v1" });
                x.EnableAnnotations();
                x.SchemaFilter<CustomSchemaFilters>();
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
Enter 'Bearer' [space] and then your token in the text input below.
\r\n\r\nExample: 'Bearer 123456789abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
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
            
            // Serve generated swagger as a JSON endpoint
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Engineer Notebook API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}