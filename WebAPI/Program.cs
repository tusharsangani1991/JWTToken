using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using WebAPI;
using WebAPI.Cache;
using WebAPI.Infrastructure;
using WebAPI.Utilities.Jwt;
using ConfigurationManager = WebAPI.ConfigurationManager;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json", optional: false)
     .Build();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var jwtTokenConfig = Configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
builder.Services.AddSingleton(jwtTokenConfig);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//        options => builder.Configuration.Bind("JwtSettings", options))
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//        options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = ApiAuthDefaults.Scheme;
    options.AddScheme<ApiAuthHandler>(ApiAuthDefaults.Scheme, ApiAuthDefaults.Scheme);
    
});

var conString = Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<DbContextEx>(options =>
{
    options.UseSqlServer(conString);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authorization",
        Description = "Bearer Authentication with JWT Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
          {jwtSecurityScheme, new string[] { }}
    });
    options.ExampleFilters();
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "Product WebAPI"
    });

    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //options.IncludeXmlComments(xmlPath);

});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

builder.Services.ConfigureSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options => options.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
    );
});
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

builder.Services.AddScoped<ICacheService, CacheService>();
//builder.Services.AddSingleton<IUserService, UserService>();
//builder.Services.AddDbContext<DbContextClass>();
builder.Services.AddSingleton<IConfig, Config>();




//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = ConfigurationManager.AppSetting["JWT:issuer"],
//            ValidAudience = ConfigurationManager.AppSetting["JWT:audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]))
//        };
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Product WebAPI");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.Run();
