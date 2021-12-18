using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmitePB.API
{
    internal static class ServiceProviderExtentions
    {
        internal static T GetService<T>(this IServiceProvider services) where T : class => services.GetService(typeof(T)) as T;
    }
}
