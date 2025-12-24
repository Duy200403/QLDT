using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AppApi.Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;
using System.Linq;
using AppApi.Common.Helper;
using AppApi.Services.AuthService;
using AppApi.Services.Common;
using AppApi.Entities.Models.Base;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.EntityFrameworkCore.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AppApi.DataAccess.Base;
using Microsoft.EntityFrameworkCore;
using AppApi.Common.Model;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using AppApi.DTO.Models.RoleDto;
using AppApi.DataAccess.Migrations.Auth;

namespace AppApi.Services.LogServ
{
    public class SeedDataService
    {
        public static async Task SeedWebApi(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WebApiDbContext>();
            var factory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient("AuthServerClient");

            // 2) Gọi AuthServer.GetRoleAdmin để lấy Role “admin”
            var response = await client.GetAsync("/api/v1/Role/GetRoleAdmin");
            response.EnsureSuccessStatusCode();
            var adminDto = await response.Content
                .ReadFromJsonAsync<RoleResponse>();
            if (adminDto == null)
                throw new InvalidOperationException("Không lấy được role admin từ AuthServer.");

            // 3) Lấy toàn bộ mappings và update
            var mappings = await db.ApiRoleMapping.ToListAsync();
            foreach (var mapping in mappings)
            {
                var list = mapping.LstAllowedRoles
                        ?? new List<AllowedRole>();

                // nếu chưa có admin (compare bằng Id)
                if (!list.Any(r => r.Id == adminDto.Id))
                {
                    list.Add(new AllowedRole
                    {
                        Id = adminDto.Id,
                        Name = adminDto.Name
                    });
                    // gán lại để trigger setter serialize JSON
                    mapping.LstAllowedRoles = list;
                }
            }

            // 4) Lưu thay đổi về DB
            await db.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public static async Task SeedAccount(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<IAccountService>();
                var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                // thêm client id vao database thì mới chạy vào connect/token lúc login nếu ko sẽ bị UnAuthorized
                // tương lai tạo Api để người dụng tự đăng kí application
                var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

                if (await manager.FindByClientIdAsync("react_spa") is null)
                {
                    var descriptorReact = new OpenIddictApplicationDescriptor
                    {
                        ClientId                         = "react_spa",
                        DisplayName                      = "React Single-Page App",
                        ClientType  = ClientTypes.Public,
                    };

                    // 3) Where your SPA runs
                    descriptorReact.RedirectUris.Add(
                        new Uri("https://localhost:3000/callback"));
                    descriptorReact.RedirectUris.Add(new Uri("https://localhost:3000/silent-renew.html"));
                    descriptorReact.PostLogoutRedirectUris.Add(
                        new Uri("https://localhost:3000/"));

                    // 4) Give it exactly the permissions it needs
                    descriptorReact.Permissions.Add(Permissions.Endpoints.Authorization);
                    descriptorReact.Permissions.Add(Permissions.Endpoints.Token);

                    descriptorReact.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
                    descriptorReact.Permissions.Add(Permissions.GrantTypes.RefreshToken);

                    descriptorReact.Permissions.Add(Permissions.ResponseTypes.Code);

                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                    // any custom API scopes you’ve created, e.g. "webapi.access"
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + "webapi.access");

                    await manager.CreateAsync(descriptorReact);
                }

                if (await manager.FindByClientIdAsync("react_spa_V2") is null)
                {
                    var descriptorReact = new OpenIddictApplicationDescriptor
                    {
                        ClientId                         = "react_spa_V2",
                        DisplayName                      = "React Single-Page App V2",
                        ClientType  = ClientTypes.Public,
                    };

                    descriptorReact.RedirectUris.Add(
                        new Uri("https://localhost:3001/callback"));
                    descriptorReact.RedirectUris.Add(new Uri("https://localhost:3001/silent-renew.html"));
                    descriptorReact.PostLogoutRedirectUris.Add(
                        new Uri("https://localhost:3001/"));

                    // 4) Give it exactly the permissions it needs
                    descriptorReact.Permissions.Add(Permissions.Endpoints.Authorization);
                    descriptorReact.Permissions.Add(Permissions.Endpoints.Token);

                    descriptorReact.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
                    descriptorReact.Permissions.Add(Permissions.GrantTypes.RefreshToken);

                    descriptorReact.Permissions.Add(Permissions.ResponseTypes.Code);

                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                    // any custom API scopes you’ve created, e.g. "webapi.access"
                    descriptorReact.Permissions.Add(Permissions.Prefixes.Scope + "apiV2");

                    await manager.CreateAsync(descriptorReact);
                }

                var application = await manager.FindByClientIdAsync("WebApiId1992");

                // 2. Chuẩn bị descriptor
                var descriptor = new OpenIddictApplicationDescriptor();
                if (application is null)
                {
                    descriptor.ClientId = "WebApiId1992";
                    descriptor.ClientSecret = "WebApiSecret1992";
                    descriptor.DisplayName = "Web API";
                }
                else
                {
                    // Populate descriptor từ entity hiện có
                    await manager.PopulateAsync(descriptor, application);
                }

                // 3. Đảm bảo permissions cần thiết
                descriptor.Permissions.Add(Permissions.Endpoints.Token);
                // descriptor.Permissions.Add(Permissions.GrantTypes.ClientCredentials);
                descriptor.Permissions.Add(Permissions.Endpoints.Authorization);
                // descriptor.Permissions.Add(Permissions.GrantTypes.Password);
                descriptor.Permissions.Add(Permissions.GrantTypes.RefreshToken);
                descriptor.Permissions.Add(Permissions.Endpoints.Revocation);
                descriptor.Permissions.Add(Permissions.Endpoints.Introspection);
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                // custom
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + "webapi.access");

                // 4. Tạo hoặc cập nhật ứng dụng trong DB
                if (application is null)
                {
                    await manager.CreateAsync(descriptor);
                }
                else
                {
                    // Đẩy descriptor đã cập nhật ngược lên entity
                    await manager.PopulateAsync(application, descriptor);
                    await manager.UpdateAsync(application);
                }

                var scopeManager = scope.ServiceProvider
                            .GetRequiredService<OpenIddict.Abstractions.IOpenIddictScopeManager>();

                var authOpts = scope.ServiceProvider.GetRequiredService<IOptions<AuthServerOptions>>().Value;

                // only create if it doesn't already exist
                // tương lai tạo Api để người dụng tự đăng kí scope
                if (await scopeManager.FindByNameAsync("webapi.access") is null)
                {
                    // create a descriptor, then populate its Resources collection
                    var descriptorScope = new OpenIddictScopeDescriptor
                    {
                        Name = "webapi.access",
                        DisplayName = "Web API Access"
                    };

                    // add each resource from your config into the descriptor’s Resources list
                    foreach (var resource in authOpts.Resources)
                    {
                        descriptorScope.Resources.Add(resource);
                    }

                    await scopeManager.CreateAsync(descriptorScope);
                }

                // Danh sách standard scope bạn muốn hỗ trợ
                var standardScopes = new[]
                {
                    Scopes.OpenId,        // "openid"
                    Scopes.Profile,       // "profile"
                    Scopes.Roles,         // "roles"
                    Scopes.OfflineAccess  // "offline_access"
                };

                foreach (var name in standardScopes) // ghép thêm custom
                {
                    if (await scopeManager.FindByNameAsync(name) is null)
                    {
                        var descriptorScope = new OpenIddictScopeDescriptor
                        {
                            Name = name,
                            DisplayName = name  // hoặc map ra tên dễ đọc tuỳ bạn
                        };
                        // trỏ luôn về audience/resource chính của bạn
                        foreach (var resource in authOpts.Resources)
                        {
                            descriptorScope.Resources.Add(resource);
                        }
                        await scopeManager.CreateAsync(descriptorScope);
                    }
                }

                //  update standard scopes cho apiV2

                // 4) Loop through each scope, fetch the existing entity, then update it
                foreach (var name in standardScopes)
                {
                    // find the persisted scope
                    var scopeEdit = await scopeManager.FindByNameAsync(name);

                    if (scopeEdit is null)
                        continue;

                    // if it *does* exist, build a descriptor pre-populated from it
                    var descriptorEdit = new OpenIddictScopeDescriptor
                    {
                        Name = name,
                        DisplayName = name
                    };

                    // copy *all* existing resources
                    foreach (var resource in await scopeManager.GetResourcesAsync(scopeEdit))
                        descriptorEdit.Resources.Add(resource);

                    // add your new one if missing
                    if (!descriptorEdit.Resources.Contains("WebApiIdV2"))
                        descriptorEdit.Resources.Add("WebApiIdV2");

                    // propagate any existing permissions (optional)
                    // foreach (var permission in await scopeManager.GetPermissionsAsync(scope))
                    //     descriptor.Permissions.Add(permission);

                    // finally, apply the update
                    await scopeManager.UpdateAsync(scopeEdit, descriptorEdit);
                }

                // Api 2
                var applicationV2 = await manager.FindByClientIdAsync("WebApiIdV2");

                // 2. Chuẩn bị descriptor
                var descriptorV2 = new OpenIddictApplicationDescriptor();
                if (applicationV2 is null)
                {
                    descriptorV2.ClientId = "WebApiIdV2";
                    descriptorV2.ClientSecret = "WebApiSecretV2";
                    descriptorV2.DisplayName = "Web API";
                }
                else
                {
                    // Populate descriptor từ entity hiện có
                    await manager.PopulateAsync(descriptorV2, applicationV2);
                }

                // 3. Đảm bảo permissions cần thiết
                descriptorV2.Permissions.Add(Permissions.Endpoints.Token);
                // descriptorV2.Permissions.Add(Permissions.GrantTypes.Password);
                // descriptor.Permissions.Add(Permissions.GrantTypes.ClientCredentials);
                descriptorV2.Permissions.Add(Permissions.Endpoints.Authorization);
                descriptorV2.Permissions.Add(Permissions.GrantTypes.RefreshToken);
                descriptorV2.Permissions.Add(Permissions.Endpoints.Revocation);
                descriptorV2.Permissions.Add(Permissions.Endpoints.Introspection);
                descriptorV2.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OpenId);
                descriptorV2.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Profile);
                descriptorV2.Permissions.Add(Permissions.Prefixes.Scope + Scopes.Roles);
                descriptorV2.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);

                // custom
                descriptorV2.Permissions.Add(Permissions.Prefixes.Scope + "apiV2");

                // 4. Tạo hoặc cập nhật ứng dụng trong DB
                if (applicationV2 is null)
                {
                    await manager.CreateAsync(descriptorV2);
                }
                else
                {
                    // Đẩy descriptor đã cập nhật ngược lên entity
                    await manager.PopulateAsync(applicationV2, descriptorV2);
                    await manager.UpdateAsync(applicationV2);
                }

                // only create if it doesn't already exist
                // tương lai tạo Api để người dụng tự đăng kí scope
                if (await scopeManager.FindByNameAsync("apiV2") is null)
                {
                    // create a descriptor, then populate its Resources collection
                    var descriptorScope = new OpenIddictScopeDescriptor
                    {
                        Name = "apiV2",
                        DisplayName = "Web API V2 Access"
                    };

                    // add each resource from your config into the descriptor’s Resources list
                    // foreach (var resource in authOpts.Resources)
                    // {
                    //     descriptorScope.Resources.Add(resource);
                    // }

                    descriptorScope.Resources.Add("WebApiIdV2");

                    await scopeManager.CreateAsync(descriptorScope);
                }

                // var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
                            // Kiểm tra và thêm scope nếu chưa tồn tại
                // var managerScope = await scopeManager.FindByNameAsync("api1");

                // if (managerScope == null)
                // {
                //     var newScope = new OpenIddictScopeDescriptor
                //     {
                //         Name = "api1",
                //         DisplayName = "API 1",
                //         Description = "Access to API 1"
                //     };
                //     await managerScope.CreateAsync(newScope);
                // }

                var roleSvc = scope.ServiceProvider.GetRequiredService<IRoleService>();
                var roleEnums = new List<RoleEnum> { RoleEnum.admin, RoleEnum.doctor, RoleEnum.patient };
                var roles = new List<Role>();

                foreach (var roleEnum in roleEnums)
                {
                    string roleName = roleEnum.ToString();
                    var existRole = await roleSvc.GetOneAsync(x => x.Name == roleName);
                    if (existRole == null)
                    {
                        var newRole = new Role
                        {
                            Id = Guid.NewGuid(),
                            Name = roleName,
                            Description = "",
                            ParentId = Guid.Empty
                        };

                        await roleSvc.AddOneAsync(newRole);
                        roles.Add(newRole);
                    }
                    else
                    {
                        roles.Add(existRole);
                    }
                }

                // var existRoles = await roleSvc.AllAsync();
                // foreach (var itemRole in existRoles)
                // {
                //     itemRole.ParentId = Guid.Empty;
                // }
                // db.Roles.UpdateRange(existRoles);

                string userName = "admin";
                string pass = "Admin$$1234";
                string salt = BC.GenerateSalt();

                Account acc = new Account()
                {
                    FullName = userName,
                    Username = userName,
                    Roles = new List<Role>
                {
                    roles.First(x => x.Name == nameof(RoleEnum.admin))
                },
                    PasswordHash = BC.HashPassword(pass),
                    Email = "admin@tnh99.com.vn",
                    AccessFailedCount = 0,
                    Pseudonym = "admin_tnh99",
                    PhoneNumber = "0979666666",
                    IsLock = false,
                    CreatedDate = DateTime.UtcNow,
                    Salt = salt,
                    IsActive = true,
                    CreatedBy = "system",
                };
                // 
                var checkExits = await svc.AnyAsync(x => x.Username == userName);
                if (!checkExits)
                {
                    await svc.AddOneAsync(acc);
                }

                userName = "doctor";
                pass = "Admin$$1234";
                salt = BC.GenerateSalt();
                acc = new Account()
                {
                    FullName = userName,
                    Username = userName,
                    Roles = new List<Role>
                {
                    roles.First(x => x.Name == nameof(RoleEnum.doctor))
                },
                    PasswordHash = BC.HashPassword(pass),
                    Email = "doctor@tnh99.com.vn",
                    AccessFailedCount = 0,
                    Pseudonym = "doctor_tnh99",
                    PhoneNumber = "0979888888",
                    IsLock = false,
                    CreatedDate = DateTime.UtcNow,
                    Salt = salt,
                    IsActive = true,
                    CreatedBy = "system",
                };
                // 
                checkExits = await svc.AnyAsync(x => x.Username == userName);
                if (!checkExits)
                {
                    await svc.AddOneAsync(acc);
                }

                userName = "patient";
                pass = "Admin$$1234";
                salt = BC.GenerateSalt();
                acc = new Account()
                {
                    FullName = userName,
                    Username = userName,
                    Roles = new List<Role>
                {
                    roles.First(x => x.Name == nameof(RoleEnum.patient))
                },
                    PasswordHash = BC.HashPassword(pass),
                    Email = "patient@tnh99.com.vn",
                    AccessFailedCount = 0,
                    Pseudonym = "patient_tnh99",
                    PhoneNumber = "0979888888",
                    IsLock = false,
                    CreatedDate = DateTime.UtcNow,
                    Salt = salt,
                    IsActive = true,
                    CreatedBy = "system",
                };
                // 
                checkExits = await svc.AnyAsync(x => x.Username == userName);
                if (!checkExits)
                {
                    await svc.AddOneAsync(acc);
                }

                // 1. Lấy role admin (chỉ cần làm 1 lần)
                var adminRole = await db.Roles
                    .Where(r => r.Name == nameof(RoleEnum.admin))
                    .Select(r => new AllowedRole { Id = r.Id, Name = r.Name })
                    .FirstOrDefaultAsync();

                if (adminRole == null)
                    throw new InvalidOperationException("Role 'admin' không tồn tại trong bảng Roles.");

                // Lấy toàn bộ mapping
                var mappings = await db.ApiRoleMapping.ToListAsync();

                foreach (var mapping in mappings)
                {
                    // danh sách hiện tại
                    var rolesAllowed = mapping.LstAllowedRoles;

                    // kiểm tra xem đã có admin chưa
                    var hasAdmin = rolesAllowed
                        .Any(r => r.Id == adminRole.Id);

                    if (!hasAdmin)
                    {
                        // thêm đối tượng AllowedRole lấy từ DB
                        rolesAllowed.Add(new AllowedRole
                        {
                            Id = adminRole.Id,
                            Name = adminRole.Name
                        });

                        // gán lại để setter serialize JSON vào cột AllowedRoles
                        mapping.LstAllowedRoles = rolesAllowed;
                    }
                }

                // 3. Lưu thay đổi
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Exception occurred in SeedAccount:");
                Console.WriteLine(ex.ToString());
            }


            //userName = "editor";
            //pass = "Admin$$1234";
            //salt = BC.GenerateSalt();
            //acc = new Account()
            //{
            //    FullName = userName,
            //    Username = userName,
            //    Roles = nameof(Helper.Role.editor),
            //    PasswordHash = BC.HashPassword(pass),
            //    Email = "editor@gmail.com",
            //    AccessFailedCount = 0,
            //    Pseudonym = "editor_108hospital",
            //    PhoneNumber = "0979888888",
            //    IsLock = false,
            //    CreatedDate = DateTime.UtcNow,
            //    Salt = salt,
            //    IsActive = true,
            //};
            //checkExits = await svc.AnyAsync(x => x.Username == userName);
            //if (!checkExits)
            //{
            //    await svc.AddOneAsync(acc);
            //}

            // var svcNoiDungDanhSach = scope.ServiceProvider.GetRequiredService<INoiDungDeNghiDanhSachService>();

            // var ckeckNoiDungDanhSach = await svcNoiDungDanhSach.UpdateManyNoiDungDanhSach();

            // if (ckeckNoiDungDanhSach == true) {
            //     return;
            // }


            // var svcDuLieuOracle = scope.ServiceProvider.GetRequiredService<ISoLuongCancelService>();

            // var startDate = new DateTime(2021, 1, 1, 0, 0, 0);
            // var endDate = new DateTime(2024, 10, 14, 23, 59, 59);
            // var resultList = await svcDuLieuOracle.GetHmsFeeCount(startDate, endDate);

            // // Console.WriteLine("listSoLuongCancel", resultList);

            // if (resultList.Count > 0)
            // {
            //     await svcDuLieuOracle.AddManyAsync(resultList);
            // }

            // var startDate = new DateTime(2021, 1, 1, 0, 0, 0);
            // var endDate = new DateTime(2024, 12, 26, 23, 59, 59);
            // var resultList = await svcDuLieuOracle.GetHmsFeeCountStatusR(startDate, endDate);

            // Console.WriteLine("listSoLuongCancel", resultList);

            // if (resultList.Count > 0)
            // {
            //     await svcDuLieuOracle.AddManyAsync(resultList);
            // }
            // var svcDuLieu = scope.ServiceProvider.GetRequiredService<IGiamSatChinhSuaDuLieuBenhVienService>();

            // var checkItem = await svcDuLieu.UpdateManyGiamSatChinhSuaDuLieuBenhVien();

            // var svcDVDN = scope.ServiceProvider.GetRequiredService<IDonViDeNghiService>();

            // var request = new DonViDeNghiPagingFilter();

            // request.PageIndex = 1;
            // request.PageSize = 1000;
            // var lstDonViDeNghi = await svcDVDN.GetAllPaging(request);
            // foreach(var item in lstDonViDeNghi.Data) {
            //     var newItem = new DonViDeNghi();
            //     newItem.ID = item.ID;
            //     newItem.IDOriginal = item.ID;
            //     newItem.TenTat = item.TenTat;
            //     newItem.Ten = item.Ten;
            //     newItem.MasterID = item.MasterID;
            //     newItem.DiaChi = item.DiaChi;
            //     newItem.Khoi = item.Khoi;
            //     newItem.IsDeletedOriginal = item.IsDeletedOriginal;
            //     newItem.IsLockedOriginal = item.IsLockedOriginal;
            //     newItem.CreatedDateOriginal = item.CreatedDateOriginal;
            //     newItem.CreatedByOriginal = item.CreatedByOriginal;
            //     newItem.UpdatedDateOriginal = item.UpdatedDateOriginal;
            //     newItem.UpdateByOriginal = item.UpdateByOriginal;
            //     newItem.Tree = item.Tree;
            //     newItem.STT = item.STT;

            //     var checkItem = await svcDVDN.UpsertAsync(newItem);
            // }

            // return;

            // dynamic lstOriginalAccount = await svc.GetListOriginalAccount();
            // IEnumerable<dynamic> lstOriginalAccountEnumerable = lstOriginalAccount as IEnumerable<dynamic>;
            // List<string> userNamesOriginal = lstOriginalAccountEnumerable.Select(model => (string)model.UserName.Value).ToList();
            // var lstAccount = new List<Account>();

            // var predicateFilterAccount = PredicateBuilder.True<Account>(); // khởi tạo mệnh đề truy vấn linq
            // predicateFilterAccount = predicateFilterAccount.And(x => true);
            // predicateFilterAccount = predicateFilterAccount.And(x => userNamesOriginal.Contains(x.Username));

            // var existingUsernames = new HashSet<string>((await svc.GetPaginatedAsync(predicateFilterAccount, 1, 10000)).Select(x => x.Username));

            // var count = 0;

            // foreach (var model in lstOriginalAccount)
            // {
            //     // count++;
            //     string userNameModel = model.UserName.Value;
            //     // var existAccount = await svc.GetOneAsync(x => x.Username == userNameModel);
            //     if (!existingUsernames.Contains(userNameModel))
            //     {
            //         Account account = new Account
            //         {
            //             Id = Guid.NewGuid(),
            //             Username = userNameModel,
            //             PasswordHash = BC.HashPassword("Admin$$1234"), // = BC.HashPassword(model.Password, salt)
            //             Salt = BC.GenerateSalt(),
            //             IsActive = true,
            //             CreatedDate = DateTime.UtcNow
            //         };

            //         lstAccount.Add(account);
            //     }

            //     // if (count > 9)
            //     // {
            //     //     break;
            //     // }
            // }

            // if (lstAccount.Count > 0)
            // {
            //     await svc.AddManyAsync(lstAccount);
            // }
        }
    }
}
