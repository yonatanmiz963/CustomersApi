using Microsoft.OpenApi.Models;
using CustomersApi.Middlewares;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
{

    // Configure services (DI)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                          });
    });

    builder.Services.AddOutputCache(options =>
    {
        options.AddPolicy("UsersPolicy", builder =>
           builder.Expire(TimeSpan.FromMinutes(20)).Tag("UsersPolicy_Tag"));

    });

    // Add services to the container.
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("AppSettings:JwtSettings"));

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddTransient<IPasswordUtilityService, PasswordUtilityService>();
    builder.Services.AddSingleton<UserDataInitializer>();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(swagger =>
    {
        //This is to generate the Default UI of Swagger Documentation
        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "JWT Token Authentication API",
            Description = ".NET 8 Web API"
        });
        // To Enable authorization using Swagger (JWT)
        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        });
        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                    });
    });
}

var app = builder.Build();
{
    // configure request pipline

    using (var scope = app.Services.CreateScope())
    {
        var userDataInitializer = scope.ServiceProvider.GetRequiredService<UserDataInitializer>();
        userDataInitializer.Initialize(); // Initialize the users data
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors(MyAllowSpecificOrigins);
    }

    app.UseMiddleware<JwtMiddleware>();
    app.UseOutputCache();
    app.UseHttpsRedirection();
    app.MapControllers();
}

app.Run();