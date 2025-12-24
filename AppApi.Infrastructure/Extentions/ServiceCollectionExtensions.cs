// RobotVibot.Libs/ServiceCollectionExtensions.cs
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using AppApi.DataAccess.Base;             // ApplicationDbContext
using AppApi.Common.Model;               // VirtualPathConfig, JwtIssuerOptions
using AppApi.Common.Model.Interface;     // IJwtIssuerOptions
using AppApi.Services.LogServ;           // SeedDataService, ILogService
using AppApi.Infrastructure.Middleware;   // JwtMiddleware
using AppApi.Mapping.Profiles;           // EmailConfigProfile, AccountProfile
using AppApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using AppApi.Entities.Models;
using System.Reflection;
using OpenIddict.Server;
using OpenIddict.Abstractions;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;
using static OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreHandlerFilters;
using Microsoft.AspNetCore;
using BC = BCrypt.Net.BCrypt;
using AppApi.Entities.Models.Base;
using OpenIddict.Server.AspNetCore;
using System.Text;
using OpenIddict.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using AppApi.Services.Common;
using System.Diagnostics;
using Azure.Core;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Nest;
using Elasticsearch.Net;

namespace AppApi.Infrastructure.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection InitCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("_allowSpecificOrigins",
                // b => b.AllowAnyOrigin()
                //       .AllowAnyMethod()
                //       .AllowAnyHeader()

                 b => b
                .WithOrigins("https://localhost:3000", "https://localhost:3001")     // Chỉ rõ origin
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()   
            ));
            return services;
        }

        public static IServiceCollection InitSqlServer<TContext>(this IServiceCollection services, IConfiguration config, string connectionName) where TContext : ApplicationDbContext
        {
            var cs = config.GetConnectionString(connectionName)
                 ?? throw new InvalidOperationException($"Không tìm thấy connection string '{connectionName}'");

            // Kiểm tra nếu DbContext là AuthDbContext thì cấu hình OpenIddict
            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(cs);

                // Nếu TContext là AuthDbContext, cấu hình OpenIddict
                if (typeof(TContext) == typeof(AuthDbContext))
                {
                    options.UseOpenIddict<Guid>();  // Chỉ áp dụng cho AuthDbContext
                    // options.UseOpenIddict();  // Chỉ áp dụng cho AuthDbContext
                }
            });

            // Đảm bảo rằng `ApplicationDbContext` được sử dụng đúng
            services.AddScoped<ApplicationDbContext>(sp =>
                    sp.GetRequiredService<TContext>());

            services.AddHttpClient("AuthServerClient", client =>
            {
                client.BaseAddress = new Uri(config["AuthServer:Issuer"]);
                // nếu dùng client‐credentials, set header Authorization = Bearer ... ở đây
            });

            return services;
        }

        public static IServiceCollection InitAuthOpeniddict<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : AuthDbContext
        {
            // var provider = services.BuildServiceProvider();
            // var authOpts = provider
            //     .GetRequiredService<IOptions<AuthServerOptions>>()
            //     .Value;

            services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                        .UseDbContext<TContext>()
                        .ReplaceDefaultEntities<Guid>()
                        ;
            })
            .AddServer(options =>
            {
                var authOpts = configuration
                    .GetSection("AuthServer")
                    .Get<AuthServerOptions>();

                // options
                // .SetTokenEndpointUris("connect/token")
                //     .SetUserInfoEndpointUris("connect/userinfo")
                //     .SetIntrospectionEndpointUris("connect/introspect")
                //     ;


                // // SSO
                // options.SetAuthorizationEndpointUris("connect/authorize")
                //     .AllowAuthorizationCodeFlow()
                //     .RequireProofKeyForCodeExchange();
                // //

                // options.AllowPasswordFlow()
                //         .AllowRefreshTokenFlow();

                // Định nghĩa các endpoint OpenID Connect
                options.SetAuthorizationEndpointUris("/connect/authorize")
                       .SetTokenEndpointUris("/connect/token")
                       //    .SetUserInfoEndpointUris("/connect/userinfo")
                       .SetIntrospectionEndpointUris("/connect/introspect")
                       //   .SetLogoutEndpointUris("/connect/logout")
                        .SetEndSessionEndpointUris("/connect/endsession")
                       ;

                // Bật các grant type cần thiết cho SSO
                options.AllowAuthorizationCodeFlow()
                       .RequireProofKeyForCodeExchange()
                       //   .AllowPasswordFlow()
                       .AllowRefreshTokenFlow();

                options.AcceptAnonymousClients();

                // Access token là JWT (self-contained)
                // Không gọi UseReferenceAccessTokens()
                // Refresh token là reference token (lưu DB)
                // options.UseJsonWebTokens();
                options.UseReferenceAccessTokens();
                options.UseReferenceRefreshTokens();

                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(1));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(8));

                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(authOpts.SecretKeyToken)));

                options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                options.DisableAccessTokenEncryption();

                options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                    //    .EnableTokenEndpointPassthrough()
                    //    .EnableUserInfoEndpointPassthrough()
                    //    .EnableLogoutEndpointPassthrough()
                     .EnableEndSessionEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    // .EnableUserInfoEndpointPassthrough()
                    ;
                
                // options.DisableTransportSecurityRequirement();

                // if save scope in-memory
                // options.RegisterScopes(
                //     Scopes.OpenId,        // "OpenId"
                //     Scopes.Profile,        // "profile"
                //     Scopes.Roles,          // "roles"
                //     Scopes.OfflineAccess   // "offline_access"
                // );

                // de instrospection co role va username, name
                options.RegisterClaims(Claims.Role, Claims.PreferredUsername, Claims.Name);

                // options.AddEventHandler<HandleAuthorizationRequestContext>(builder =>
                // {
                //     builder.UseInlineHandler(context =>
                //     {
                //         if (context.IsRejected)
                //         {
                //             Console.WriteLine($"[OpenIddict DEBUG] REJECTED: {context.Error} - {context.ErrorDescription}");
                //         }
                //         return default;
                //     });
                // });

                // options.AddEventHandler<HandleAuthorizationRequestContext>(builder => builder
                //     .AddFilter<RequireHttpRequest>()    // để có HttpContext
                //     .UseInlineHandler(async context =>
                //     {
                //         var http = context.Transaction.GetHttpRequest()!.HttpContext;
                //         var httpRequest = context.Transaction
                //                         .GetHttpRequest()
                //                         ?? throw new InvalidOperationException("Không lấy được HttpRequest.");
                //         var dbContext = httpRequest.HttpContext.RequestServices
                //                                     .GetRequiredService<TContext>();

                //         // 1) Nếu chưa có cookie SSO → redirect đi login
                //         if (!http.User.Identity!.IsAuthenticated)
                //         {
                //             var props = new AuthenticationProperties
                //             {
                //                 RedirectUri = http.Request.Path + http.Request.QueryString
                //             };
                //             context.Logger.LogInformation("Chưa login, challenge cookie.");
                //             context.Reject(Errors.AccessDenied);
                //             await http.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme, props);
                //             return;
                //         }

                //         var username = http.User
                //                     .FindFirst(OpenIddictConstants.Claims.PreferredUsername)?
                //                     .Value
                //                     ?? throw new InvalidOperationException("No preferred_username claim");

                //         // Hoặc lấy về subject (account.Id) nếu bạn muốn query theo Id
                //         // var userId = http.User
                //         //     .FindFirst(OpenIddictConstants.Claims.Subject)?
                //         //     .Value
                //         //     ?? throw new InvalidOperationException("No subject claim");

                //         // 2) Nếu đã login → load user từ http.User / DB
                //         var account = await dbContext.Accounts
                //                                             .Include(a => a.Roles)
                //                                             .FirstOrDefaultAsync(a => a.Username == username);

                //         if (account == null)
                //         {
                //             context.Reject(Errors.InvalidGrant, "Tài khoản không tồn tại.");
                //             return;
                //         }

                //         // 3) Build principal và issue code
                //         var principal = BuildClaimsPrincipal(account,
                //                         context.Request.GetScopes(),
                //                         http);

                //         principal.SetScopes(context.Request.GetScopes());
                //         context.SignIn(principal);
                //     })
                // );

                // options.AddEventHandler<HandleTokenRequestContext>(builder => builder
                //            // Chỉ chạy khi có HttpRequest (sẽ có HttpContext)
                //            .AddFilter<RequireHttpRequest>()
                //            .UseInlineHandler(async context =>
                //            {
                //                // 3.1 Lấy HttpRequest từ transaction
                //                var httpRequest = context.Transaction
                //                    .GetHttpRequest()
                //                    ?? throw new InvalidOperationException("Không lấy được HttpRequest.");

                //                var dbContext = httpRequest.HttpContext.RequestServices
                //                               .GetRequiredService<TContext>();
                //                var logService = httpRequest.HttpContext.RequestServices
                //                                   .GetRequiredService<ILogService>();

                //                if (context.Request.IsPasswordGrantType())
                //                {

                //                    // 3.2 Load user + roles
                //                    var account = await dbContext.Accounts
                //                                       .Include(a => a.Roles)
                //                                       .FirstOrDefaultAsync(a => a.Username == context.Request.Username);
                //                    if (account == null)
                //                    {
                //                        context.Reject(Errors.InvalidGrant, "Sai tên đăng nhập hoặc mật khẩu.");
                //                        return;
                //                    }

                //                    if (account == null || !await ValidateAccountStatus(account, context, logService))
                //                        return;
                //                    // 3.5 Kiểm tra mật khẩu
                //                    if (!BC.Verify(context.Request.Password, account.PasswordHash))
                //                    {
                //                        account.AccessFailedCount++;
                //                        if (account.AccessFailedCount >= 5)
                //                        {
                //                            account.IsLock = true;
                //                            account.TimeLock = DateTime.UtcNow.AddMinutes(5);
                //                        }
                //                        await dbContext.SaveChangesAsync();

                //                        context.Reject(Errors.InvalidGrant, "Sai tên đăng nhập hoặc mật khẩu.");
                //                        return;
                //                    }

                //                    // 3.6 Đăng nhập thành công: reset counter + unlock nếu cần
                //                    account.AccessFailedCount = 0;
                //                    account.IsLock = false;
                //                    account.TimeLock = null;
                //                    await dbContext.SaveChangesAsync();

                //                    //    var principal = BuildClaimsPrincipal(account, new[] { Scopes.OpenId, Scopes.Profile, Scopes.Roles, Scopes.OfflineAccess }, httpRequest.HttpContext);
                //                    var requestedScopes = context.Request.GetScopes();
                //                    var principal = BuildClaimsPrincipal(account, requestedScopes, httpRequest.HttpContext);

                //                    //    context.Principal = principal;
                //                    context.SignIn(principal);
                //                }
                //                else if (context.Request.IsRefreshTokenGrantType())
                //                {
                //                    // Principal cũ lấy từ refresh token
                //                    var principal = context.Principal;

                //                    var userId = principal.GetClaim(Claims.Subject);
                //                    var account = await dbContext.Accounts
                //                        .Include(a => a.Roles)
                //                        .FirstOrDefaultAsync(a => a.Id.ToString() == userId);

                //                    //    if (account == null || !account.IsActive || (account.IsLock && account.TimeLock.HasValue && DateTime.UtcNow < account.TimeLock.Value))
                //                    //    {
                //                    //        context.Reject(Errors.InvalidGrant, "Tài khoản không hợp lệ hoặc đang bị khóa.");
                //                    //        return;
                //                    //    }

                //                    if (account == null || !await ValidateAccountStatus(account, context, logService))
                //                        return;

                //                    var newPrincipal = BuildClaimsPrincipal(account, principal.GetScopes(), httpRequest.HttpContext);
                //                    context.SignIn(newPrincipal);
                //                }
                //                else
                //                {
                //                    context.Reject(Errors.UnsupportedGrantType, "Grant type không được hỗ trợ.");
                //                    return;
                //                }
                //            }));

                options.UseDataProtection();

            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
                options.UseDataProtection();
                // options.EnableTokenEntryValidation();
            })
            ;

            // services
            //     .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //     .AddCookie(options =>
            //     {
            //         options.Cookie.HttpOnly = true;
            //         options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // chỉ HTTPS
            //         options.Cookie.SameSite = SameSiteMode.None; // cần để cross-site OIDC
            //     });

            // // Cấu hình Authentication mặc định
            // services.AddAuthentication(options =>
            // {
            //     options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            //     // options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            // });
            // .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Cookie.Name = "SSO_Cookie"; // Unique name for shared cookie
                        // options.Cookie.Domain = ".yourdomain.com"; // Shared domain for all apps
                        options.ExpireTimeSpan = TimeSpan.FromHours(3); // Match access token lifetime
                    });

            return services;
        }

        // Hàm tiện ích để xây dựng ClaimsPrincipal từ Account
        private static ClaimsPrincipal BuildClaimsPrincipal(
                Account account,
                IEnumerable<string> scopes,
                HttpContext httpContext)
        {
            // Tạo identity dùng scheme của OpenIddict Server
            var identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name,
                Claims.Role);

            // Thêm các claim cơ bản
            identity.AddClaim(Claims.Subject, account.Id.ToString());
            identity.AddClaim(Claims.Name, account.FullName ?? string.Empty);
            identity.AddClaim(Claims.PreferredUsername, account.Username ?? string.Empty);

            foreach (var role in account.Roles)
            {
                identity.AddClaim(Claims.Role, role.Name);
            }

            // Quyết định claim nào cho access_token và/hoặc identity_token
            identity.SetDestinations(claim =>
            {
                return claim.Type switch
                {
                    // selalu cần trong access_token để API đọc via introspection
                    Claims.Subject => new[] { Destinations.AccessToken },
                    Claims.Name => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    Claims.PreferredUsername => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    Claims.Role => new[] { Destinations.AccessToken, Destinations.IdentityToken },
                    _ => Array.Empty<string>()
                };
            });


            // Tạo principal và gán scope
            var principal = new ClaimsPrincipal(identity);
            principal.SetScopes(scopes);

            var request = httpContext.GetOpenIddictServerRequest();
            if (request is not null)
            {
                // tell OpenIddict which resource servers this token can access:
                principal.SetResources(request.ClientId!);

                // (optional) also set audiences if you need JwtAudience checks:
                principal.SetAudiences(request.ClientId!);
            }

            // // Nếu client truyền resource, gán luôn
            // var request = httpContext.GetOpenIddictServerRequest();
            // if (request?.HasParameter(OpenIddictConstants.Parameters.Resource) == true)
            // {
            //     var resource = (string)request.GetParameter(OpenIddictConstants.Parameters.Resource)!;
            //     principal.SetResources(new[] { resource });
            // }

            // // Debug: in ra console để kiểm tra
            // foreach (var claim in principal.Claims)
            // {
            //     Console.WriteLine($"[BuildClaimsPrincipal] Claim: Type = {claim.Type}, Value = {claim.Value}");
            // }

            return principal;
        }

        private static async Task<List<string>> GetClientIdsAsync(
        IOpenIddictApplicationManager appManager,
        int maxDegreeOfParallelism = 10)
        {
            var clientIds = new List<string>();
            var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
            var tasks = new List<Task>();

            await foreach (var app in appManager.ListAsync())
            {
                await semaphore.WaitAsync();

                var task = Task.Run(async () =>
                {
                    try
                    {
                        var clientId = await appManager.GetClientIdAsync(app);
                        lock (clientIds)
                        {
                            clientIds.Add(clientId);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            return clientIds;
        }

        // Hàm tiện ích kiểm tra trạng thái Account
        private static async Task<bool> ValidateAccountStatus(Account account, HandleTokenRequestContext context, ILogService logService)
        {
            if (!account.IsActive)
            {
                await logService.AddLogWebInfo(LogLevelWebInfo.error, "Login", "Tài khoản chưa active");
                context.Reject(Errors.InvalidGrant, "Tài khoản chưa được kích hoạt.");
                return false;
            }

            if (account.IsLock && account.TimeLock.HasValue && DateTime.UtcNow < account.TimeLock.Value)
            {
                context.Reject(Errors.InvalidGrant, "Tài khoản đang bị khóa tạm thời.");
                return false;
            }

            return true;
        }

        public static IServiceCollection InitWebApiOpeniddict<TContext>(this IServiceCollection services, IConfiguration config) where TContext : ApplicationDbContext
        {
            var issuer = config["AuthServer:Issuer"] ?? throw new InvalidOperationException("Issuer không được cấu hình");
            var clientId = config["AuthServer:ClientId"] ?? throw new InvalidOperationException("Audience không được cấu hình");
            var secret = config["AuthServer:Secret"] ?? throw new InvalidOperationException("Audience không được cấu hình");
            var secretToken = config["AuthServer:SecretKeyToken"] ?? throw new InvalidOperationException("Audience không được cấu hình");

            services.AddOpenIddict()
                .AddValidation(options =>
                {
                    // Nếu WebApi và AuthService cùng một server, bạn có thể dùng LocalServer
                    // options.UseLocalServer();

                    // Nếu AuthService chạy riêng, bạn cần cấu hình Issuer và Introspection (kiểm tra token qua AuthService)
                    // Thay URL bên dưới bằng URL AuthService thực tế của bạn
                    options.SetIssuer(issuer);

                    // options.AddAudiences("webapi.access"); // Audience API nếu có
                    options.AddAudiences(clientId);

                    options.AddEncryptionKey(new SymmetricSecurityKey(
                             Convert.FromBase64String(secretToken)));

                    // chạy chính có SSL
                    options.UseSystemNetHttp();

                    // IISBypass test trên IIS localhost SSL thì dùng cái này để bỏ qua SSL:
                    // options.UseSystemNetHttp()
                    //     .ConfigureHttpClientHandler(handler => handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator);

                    options.UseAspNetCore();

                    options.UseIntrospection()  // Dùng introspection để xác thực reference token qua AuthService
                           .SetClientId(clientId)       // Client Id đăng ký trên AuthService cho WebAPI
                           .SetClientSecret(secret)
                           //    .EnableTokenEntryValidation()
                           ;
                    options.UseDataProtection();
                });

            // services.AddDataProtection()
            //     .SetApplicationName("MyCompany.SharedSSO")
            //     .PersistKeysToFileSystem(new DirectoryInfo(@"C:\dpkeys"));

            services.Configure<OpenIddictValidationOptions>(options =>
            {
                options.TokenValidationParameters.RoleClaimType = Claims.Role;
                options.TokenValidationParameters.NameClaimType = Claims.Name;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = OpenIddict.Validation.AspNetCore.OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DynamicRolesManagementSoftware", policy =>
                    policy.RequireAuthenticatedUser()
                        .AddRequirements(new DynamicRoleAttributeManagementSoftware()));
            });

            return services;
        }


        public static IServiceCollection InitRedis(this IServiceCollection services, IConfiguration config)
        {
            services.AddStackExchangeRedisCache(opts =>
            {
                opts.Configuration = config.GetConnectionString("RedisConnection");
            });
            return services;
        }

        public static IServiceCollection InitControllers(this IServiceCollection services)
        {
            services.AddControllers();
            services
              .AddControllers(options =>
              {
                  // turn off implicit [BindRequired] on non-nullable refs
                  options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
              })
              .AddJsonOptions(o =>
              {
                  o.JsonSerializerOptions.IgnoreNullValues = true;
                  o.JsonSerializerOptions.Converters
                    .Add(new JsonStringEnumConverter());
              })
              .AddNewtonsoftJson(o =>
              {
                  o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                  o.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                  o.SerializerSettings.Converters
                    .Add(new Newtonsoft.Json.Converters.StringEnumConverter());
              });
            return services;
        }

        public static IServiceCollection InitServerLimits(this IServiceCollection services)
        {
            // For IIS in‐process hosting
            services.Configure<IISServerOptions>(opts =>
            {
                opts.MaxRequestBodySize = int.MaxValue;
            });

            // For Kestrel
            services.Configure<KestrelServerOptions>(opts =>
            {
                opts.Limits.MaxRequestBodySize = long.MaxValue; // if don't set default value is: 30 MB
            });

            // For multipart/form
            services.Configure<FormOptions>(opts =>
            {
                opts.ValueLengthLimit = int.MaxValue;
                opts.MultipartBodyLengthLimit = long.MaxValue; // if don't set default value is: 128 MB
                opts.MultipartHeadersLengthLimit = int.MaxValue;
            });

            return services;
        }

        public static IServiceCollection InitApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opts =>
            {
                opts.DefaultApiVersion = new ApiVersion(1, 0);
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(opts =>
            {
                opts.GroupNameFormat = "'v'VVV";
                opts.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

        // public static IServiceCollection InitJWT(this IServiceCollection services)
        // {
        //     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //         .AddJwtBearer(options =>
        //         {
        //             var jwtIssuerOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtIssuerOptions>>().Value;

        //             options.TokenValidationParameters = new TokenValidationParameters
        //             {
        //                 ValidateIssuer = true,
        //                 ValidateAudience = true,
        //                 ValidateLifetime = true,
        //                 ValidateIssuerSigningKey = true,
        //                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtIssuerOptions.Secret)),
        //                 ValidIssuer = jwtIssuerOptions.JwtIssuer,
        //                 ValidAudience = jwtIssuerOptions.JwtAudience,
        //                 ClockSkew = TimeSpan.Zero
        //             };

        //             options.Events = new JwtBearerEvents
        //             {
        //                 OnAuthenticationFailed = context =>
        //                 {
        //                     Console.WriteLine($"Authentication failed: {context.Exception.Message}");
        //                     return Task.CompletedTask;
        //                 },
        //                 OnTokenValidated = context =>
        //                 {
        //                     Console.WriteLine("Token validated successfully");
        //                     return Task.CompletedTask;
        //                 }
        //             };
        //         });
        //     return services;
        // }

        public static IServiceCollection InitSwagger(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(c => c.OperationFilter<SwaggerDefaultValues>());
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            return services;
        }

        public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.Configure<AuthServerOptions>(config.GetSection("AuthServer"));
            services.Configure<List<VirtualPathConfig>>(
                config.GetSection(nameof(VirtualPathConfig))
            );
            services.Configure<JwtIssuerOptions>(
                config.GetSection(nameof(JwtIssuerOptions))
            );
            services.AddScoped<IJwtIssuerOptions>(sp =>
                sp.GetRequiredService<IOptions<JwtIssuerOptions>>().Value
            );
            services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, DynamicRolesHandler>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();

            services.AddSingleton<IElasticClient>(_ =>
            {
                var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200")); // or StaticConnectionPool for many nodes
                var settings = new ConnectionSettings(pool)
                    .ServerCertificateValidationCallback((o, cert, chain, errors) => true) // DEV ONLY
                    .DefaultMappingFor<EmailConfig>(m => m.IndexName("emailconfig"))
                    .EnableDebugMode()
                    .ThrowExceptions();
                    // map known types to indices (better than DefaultIndex-per-instance)
                    // .DefaultMappingFor<EmailConfig>(m => m.IndexName("emailconfig"))
                    // .ServerCertificateValidationCallback((o, cert, chain, errors) => true);
                // .DefaultMappingFor<EmailRule>(m => m.IndexName("emailrule"));
                return new ElasticClient(settings);
                // var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                //     .DefaultMappingFor<EmailConfig>(m => m.IndexName("emailconfig"));
                // return new ElasticClient(settings);
            });
            
            services.AddScoped(typeof(AppApi.DTO.ElasticSearch.ElasticSearch<>));
            return services;
        }

        public static IServiceCollection InitMapping(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<EmailConfigProfile>();
            return services;
        }

        public static IServiceCollection InitUnitOfWorkAndServices(this IServiceCollection services, string tag)
        {
            services.AddUnitOfWork();
            //services.AddAllServices();
            services.AddAllServices(tag);
            services.AddLogging();

            // var sp = services.BuildServiceProvider();
            // var genericLog = sp.GetRequiredService<ILogger<Log>>();
            // services.AddSingleton<ILogger>(genericLog);
            // services.AddLogging(logging =>
            // {
            //     logging.AddConsole();
            //     logging.AddDebug();
            // });
            return services;
        }

        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
        {
            // scan the assembly where your profiles live
            services.AddAutoMapper(typeof(AccountProfile).Assembly);
            return services;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAllMiddlewares<TContext>(this IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider, Assembly controllerAssembly,
        string controllerNamespace, bool seedAccounts = false, bool seedWebApi = false)
            where TContext : DbContext
        {
            // apply pending migrations
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TContext>();
                db.Database.Migrate();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Truyền Assembly của WebApi chứa Controller
                // Truyền đúng instance DbContext hiện tại
                ApiRoleMappingHelper.SyncApiRoleMappingsAsync(db, controllerAssembly, controllerNamespace).GetAwaiter().GetResult();
            }

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                    c.SwaggerEndpoint(
                        $"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
            });

            app.UseCors("_allowSpecificOrigins");
            app.UseRouting();
            // app.UseMiddleware<JwtMiddleware>();
            // app.UseAuthorization();
            app.UseAuthentication();  // Thiếu cái này sẽ không xác thực được
            app.UseAuthorization();

            // static files from VirtualPathConfig
            var paths = app.ApplicationServices.GetRequiredService<
                IOptions<List<VirtualPathConfig>>>().Value;
            var root = env.ContentRootPath;
            foreach (var p in paths)
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(root, p.RealPath)),
                    RequestPath = p.RequestPath
                });
            }

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });
            app.UseEndpoints(endpoints =>
                 {
                     endpoints.MapControllers();
                     endpoints.MapDefaultControllerRoute();
                     //  endpoints.MapControllerRoute(
                     // name: "default",
                     // pattern: "{controller}/{action=Index}/{id?}");
                 });

            // seed admin account
            Task.Run(async () =>
            {
                if (seedAccounts)
                {
                    await SeedDataService.SeedAccount(app.ApplicationServices);
                }
                if (seedWebApi)
                {
                    await SeedDataService.SeedWebApi(app.ApplicationServices);
                }
            }
                );

            return app;
        }
    }
}
