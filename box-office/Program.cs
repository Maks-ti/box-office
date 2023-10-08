
using box_office;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Npgsql;
using box_office.DataBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using box_office.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Rewrite;
using StackExchange.Redis;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

try
{
    var builder = WebApplication.CreateBuilder(args);


    // Add services to the container.

    builder.Services.AddControllers();

    // builder ������������
    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);

    IConfiguration configuration = configurationBuilder.Build();

    builder.Configuration.AddConfiguration(configuration); // ������������ ������������

    #region Kestrel
    // ����������� ������ �� ������ ���������
    int httpPort, httpsPort;
    if (builder.Environment.IsDevelopment())
    {
        httpPort = builder.Configuration.GetValue<int>("Kestrel:Development:HttpPort");
        httpsPort = builder.Configuration.GetValue<int>("Kestrel:Development:HttpsPort");
    }
    else
    {
        httpPort = builder.Configuration.GetValue<int>("Kestrel:Production:HttpPort");
        httpsPort = builder.Configuration.GetValue<int>("Kestrel:Production:HttpsPort");
    }

    // ��������� Kestrel
    bool successParseBodySize = int.TryParse(configuration["MaxRequestBodySize"], out int maxRequestBodySize); // ������ �������� �� ������������
    builder.WebHost.ConfigureKestrel(options =>
    {
        int size = (successParseBodySize) ? maxRequestBodySize : 30;
        // ������������� ����� �� ���� �������
        options.Limits.MaxRequestBodySize = size * 1024 * 1024;

        // ��������� ���������� � ����� ����������� ��� httpS � ������
        string sertificeteFilePath = builder.Configuration.GetValue<string>("httpsSertificateInfo:filepath");
        string sertificatePassword = builder.Configuration.GetValue<string>("httpsSertificateInfo:password");

        // ��������� ������
        options.ListenAnyIP(httpPort); // HTTP
        options.ListenAnyIP(httpsPort, listenOptions => // HTTPS
        {
            listenOptions.UseHttps(sertificeteFilePath, sertificatePassword);
        });
    });
    #endregion

    #region Logging
    LayoutRenderer.Register<BoxOfficeLayoutRendererWrapper>("intercept");
    var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    #endregion

    #region Controllers

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    #endregion

    #region Swagger
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1.0", new OpenApiInfo { Version = "v1.0", Title = "Box Office Service" });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "����������� Bearer-����� ��� ����������� ����� ���������. Example: \"Authrization: {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Type = SecuritySchemeType.Http
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        options.CustomSchemaIds(type => $"{type.Name}_{Guid.NewGuid()}");
    });
    #endregion

    #region AUTH
    builder.Services.AddSingleton<RsaSecurityKey>(provider =>
    {
        RSA rsa = RSA.Create();
        rsa.ImportRSAPublicKey(
            source: Convert.FromBase64String(configuration["Jwt:PublicKey"]),
            bytesRead: out int _
        );

        return new RsaSecurityKey(rsa);
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;


    }).AddJwtBearer(options =>
    {
        SecurityKey rsa = builder.Services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();
        options.RequireHttpsMetadata = false; // ��������� ���������� HTTPS
        options.SaveToken = true;
        // �������� ���������� ������
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "BOX_OFFICE", // �������� ������
            ValidateIssuerSigningKey = true,

            ValidateAudience = true,
            ValidAudience = "REST_CLIENT",

            ValidateLifetime = true,
            RequireExpirationTime = true,

            IssuerSigningKey = rsa,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
    });
    #endregion

    #region DataBase Contexts
    // ������������ ������ ��������� ��������� timestamp � postgres
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    //������ ������� ��������
    LoggerFactory dbloggerFactory = new(new[] { new NLogLoggerProvider() });

    // ��������� ����������� ������� ����� NpgSQL ��� JSON
    NpgsqlConnection.GlobalTypeMapper.UseJsonNet(settings: new()
    {
        Formatting = Formatting.None,
        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
        }
    });

    // ��������� DbContext 
    builder.Services.AddDbContext<DataBaseContext>((serviceProvider, options) =>
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var connectionString = configuration.GetConnectionString("Postgres");

        options.UseLoggerFactory(loggerFactory)
               .UseNpgsql(connectionString, config => config.CommandTimeout(30));

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    }, ServiceLifetime.Transient);

    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

    #endregion

    #region Behavior
    builder.Services.AddMvcCore();
    #endregion

    #region Services

    // ����������� ����� ������������ ����������

    // ������������ �������
    builder.Services.AddSingleton<AuthService>();
    builder.Services.AddTransient<PlayService>();
    builder.Services.AddTransient<HallService>();
    builder.Services.AddTransient<SessionService>();
    builder.Services.AddTransient<TicketService>();

    // ��������� hhtp-������������
    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.RequestHeaders |
                                HttpLoggingFields.RequestBody |
                                HttpLoggingFields.ResponseHeaders |
                                HttpLoggingFields.ResponseBody;
    });

    builder.Services.AddControllersWithViews(); // ���������� ����� ��� ������ � ������������� � ���������������
    #endregion

    #region CORS
    // ��������� ��������� CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("http://*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            });
    });
    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("enable_swagger"))
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("../swagger/v1.0/swagger.json", "v1.0");
        });

        // ����� ������ ReriteOptions
        RewriteOptions redirections = new();
        // ��������� ������� ���������������
        redirections.AddRedirect("^$", "swagger");
        // ������������ �������� ������� ���������������
        app.UseRewriter(redirections);
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    // ��������� CORS middleware ����� UseRouting � ����� UseAuthorization
    app.UseCors("AllowSpecificOrigin");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
        name: "login",
        pattern: "LoginPage/LoginPage", // ���� � �������� �����������
        defaults: new { controller = "LoginPage", action = "LoginPage" } // ���������� � �������� ��� �������� �����������
        );

        endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    });


    await app.RunAsync();
}
finally
{
    LogManager.Shutdown();
}
