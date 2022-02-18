using AutoMapper;
using Core;
using Entity.Category;
using Entity.Offer;
using Entity.Product;
using Entity.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
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
using MLS_Data.Context;
using MLS_Data.DataModels;
using MLS_Data.UoW;
using System;
using System.IO;
using System.Reflection;
using System.Text;


namespace MLS_Api
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
            //cors
            services.AddCors(opt => opt.AddPolicy("Cors", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            //connection to database
            var connectionString = Configuration.GetConnectionString("MyLittleShopDatabaseString");
            services.AddDbContext<MyLittleShopDbContext>(options => options.UseSqlServer(connectionString));

            //identity
            services.AddIdentity<ApplicationUser_DataModel, IdentityRole>()
                .AddEntityFrameworkStores<MyLittleShopDbContext>()
                .AddDefaultTokenProviders();


            //identity options
            services.Configure<IdentityOptions>(options =>
            {
                //password rules
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                //lock rules
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;

                //email rules
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<PasswordHasherOptions>(opt =>
            {
                opt.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            });

            //hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration["ConnectionStrings:MLS_Hangfire"]));
            services.AddHangfireServer();

            //jwt
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Tokens:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidateLifetime = true
                };
            });

            //swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("MyLittleShopApi", new OpenApiInfo
                {
                    Title = "My Little Shop Api Documentation",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Enes Can UYAR",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });

            //automapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            //unitofwork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers();

            //fluentvalidator
            services.AddMvc().AddFluentValidation();

            //validations
            //user
            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddTransient<IValidator<LogInUserDto>, LogInUserDtoValidator>();
            //product
            services.AddTransient<IValidator<RegisterProductDto>, RegisterProductDtoValidator>();
            //category
            services.AddTransient<IValidator<CreateCategoryDto>, CreateCategoryDtoValidator>();
            //offer
            services.AddTransient<IValidator<MakeOfferDto>, MakeOfferDtoValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //cors enable
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MLS_Api v1"));
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/MyLittleShopApi/swagger.json", "My Little Shop Api"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //auth
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
