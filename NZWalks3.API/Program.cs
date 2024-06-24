    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.EntityFrameworkCore;
    using NZWalks3.API.DatabaseConnection;
    using NZWalks3.API.Mappings;
    using NZWalks3.API.Repository;
    using System.Text;
    using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    //this service for image path
    builder.Services.AddHttpContextAccessor();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        // Add JWT Bearer security definition
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Please enter a valid token"
        });

        // Add security requirement
        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });


builder.Services.AddDbContext<NZWalkDBContext>(Options =>
    {
        Options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
    });

    builder.Services.AddDbContext<NZAuthDBContext>(Options =>
    {
        Options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDBConnection"));
    });
    builder.Services.AddScoped<IRegionRepository, SQLConnectionRegion>();
    builder.Services.AddScoped<IWalkRepository  , SQLConnectionWalk>();
    builder.Services.AddScoped<ITokenRepository, TokenRepository>();
    builder.Services.AddScoped<IImageRepository, ImageRepository>();

    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

    builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
        .AddEntityFrameworkStores<NZAuthDBContext>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            }); 


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles( new StaticFileOptions
    {
          FileProvider = new PhysicalFileProvider (Path.Combine(Directory.GetCurrentDirectory(),"Images")),
          RequestPath = "/Images"
    });

    app.MapControllers();

    app.Run();
