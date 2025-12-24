
using AppApi.Common.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;

namespace AppApi.DataAccess.Base
{
    public static class ServiceRegistrar
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<ServiceFactory>(provider => serviceType => provider.GetRequiredService(serviceType));
            services.AddScoped<ServiceFactory>(s => s.GetRequiredService);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        // public static IServiceCollection AddAllServices(this IServiceCollection services)
        // {
        //     foreach (var type in AssemblyHelper.ExportTypes)
        //     {
        //         if (!type.Name.EndsWith("Repository") && !type.Name.EndsWith("Service")) continue;

        //         if (typeof(IBaseService).IsAssignableFrom(type) || typeof(IGenericRepository).IsAssignableFrom(type))
        //         {
        //             foreach (var serviceType in type.GetInterfaces().Where(t => !t.Name.StartsWith("System")))
        //             {
        //                 services.TryAddScoped(serviceType, type);
        //             }
        //         }
        //     }

        //     return services;
        // }

        public static IServiceCollection AddAllServices(this IServiceCollection services, string tag)
        {
            foreach (var type in AssemblyHelper.ExportTypes)
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                var ns = type.Namespace ?? "";
                if (!ns.Contains($".{tag}") && !ns.Contains(".LogServ") && !ns.Contains(".Common"))
                    continue;

                // Bỏ qua các service nào bạn không muốn đăng ký tự động, ví dụ CronJobUpdateDonViDeNghiService
                // if (type.Name == "CronJobUpdateDonViDeNghiService")
                //     continue;

                if (!typeof(IBaseService).IsAssignableFrom(type) &&
                !typeof(IGenericRepository).IsAssignableFrom(type))
                    continue;

                // ⭐ Chỉ register nếu tên class kết thúc bằng Service hoặc Repository
                if (!type.Name.EndsWith("Service") && !type.Name.EndsWith("Repository"))
                    continue;

                var interfaces = type.GetInterfaces()
                    .Where(i => !i.Namespace.StartsWith("System") &&
                                !(i.IsGenericType && i.ContainsGenericParameters))
                    .ToList();

                if (!interfaces.Any()) continue;

                foreach (var iface in interfaces)
                {
                    services.TryAddScoped(iface, type);
                }

            }

            return services;
        }
    }
}