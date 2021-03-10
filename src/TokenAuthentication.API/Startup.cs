using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TokenAuthentication.API.Authorization;
using TokenAuthentication.Common.Automapper;
using TokenAuthentication.Common.Interface;
using TokenAuthentication.Entity.Entity;
using TokenAuthentication.Interfaces;
using TokenAuthentication.Repositories;
using TokenAuthentication.Services;

namespace TokenAuthentication.API
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
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            #region IdentityEntityFramework
            services.AddDbContext<TokenAuthenticationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TokenAuthenticationDbContext"));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<TokenAuthenticationDbContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region AutoMapper
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TokenAuthenticationProfile());
            });
            IMapper mapper = mapperConfiguration.CreateMapper();
            #endregion

            #region RegisterServices
            services.AddSingleton(mapper);
            services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));            
            services.AddScoped(typeof(IDepartmentService), typeof(DepartmentService));            
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IRoleService), typeof(RoleService));
            services.AddScoped(typeof(IUserRoleService), typeof(UserRoleService));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddSingleton(typeof(IActionContextAccessor), typeof(ActionContextAccessor));
            #endregion

            #region Authentication
            const string Token_Authentication_Scheme = "TokenAuthentication";
            var key = Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:SecretKey").Value);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Token_Authentication_Scheme;
                options.DefaultChallengeScheme = Token_Authentication_Scheme;
                options.DefaultScheme = Token_Authentication_Scheme;
                options.DefaultSignInScheme = Token_Authentication_Scheme;
                options.DefaultForbidScheme = Token_Authentication_Scheme;
            }).AddJwtBearer(Token_Authentication_Scheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RequireExpirationTime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetSection("Jwt:Issuer").Value,
                    ValidAudience = Configuration.GetSection("Jwt:Audience").Value,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Token Authentication API",
                    Description = "Token Authentication API",

                });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "bearer"
                                 }
                             },
                             new string[] {}
                     }
                 });
            });
            #endregion
            #region AuthorizationPolicy
            services.AddSingleton(typeof(IAuthorizationHandler), typeof(MinimumAgeHandler));
            services.AddAuthorization(config => {
                config.AddPolicy("AgeRestriction", policy =>
                {
                    policy.AddRequirements(new MinimumAgeRequirement(30));
                });
            });
                
            #endregion
            services.AddCors();
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(config =>
            {
                config.WithOrigins("http://localhost:4200").AllowCredentials().AllowAnyMethod().AllowAnyHeader();
            });


            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Token Authentication API");
                c.RoutePrefix = string.Empty; ;
            });
        }
    }
}
