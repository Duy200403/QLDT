using System.Reflection;
using AppApi.DataAccess.Base;
using AppApi.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Infrastructure.Extentions
{
    public static class ApiRoleMappingHelper
    {
        public static async Task SyncApiRoleMappingsAsync<TContext>(TContext dbContext, Assembly assembly, string controllerNamespaceFilter)
        where TContext : ApplicationDbContext
        {
            var controllerTypes = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)) && !t.IsAbstract)
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(controllerNamespaceFilter)); // Lọc theo namespace

            foreach (var controller in controllerTypes)
            {
                var controllerName = controller.Name.Replace("Controller", "");
                // var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                //     .Where(m => !m.IsDefined(typeof(NonActionAttribute)) && !m.IsSpecialName);

                // Chỉ lấy các phương thức public, declared in this class, không phải [NonAction],
                // và có ít nhất 1 attribute kế thừa HttpMethodAttribute (HttpGet, HttpPost, …)
                var actions = controller
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(m =>
                        !m.IsDefined(typeof(NonActionAttribute)) &&
                        m.GetCustomAttributes().Any(attr => attr is HttpMethodAttribute)
                    );

                foreach (var action in actions)
                {
                    var actionName = action.Name;

                    bool exists = await dbContext.ApiRoleMapping
                        .AnyAsync(x => x.Controller == controllerName && x.Action == actionName);

                    if (!exists)
                    {
                        dbContext.ApiRoleMapping.Add(new ApiRoleMapping
                        {
                            Controller = controllerName,
                            Action = actionName,
                            AllowedRoles = "" // Mặc định hoặc để trống
                        });
                    }
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
