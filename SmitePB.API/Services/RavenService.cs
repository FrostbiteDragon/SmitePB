using System;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;

namespace SmitePB.API.Services
{
    public static class RavenService
    {
        public static Task<TResult> AccessRaven<TResult>(IServiceProvider services, Func<IAsyncDocumentSession, Task<TResult>> func)
        {
            var session = services.GetService<IAsyncDocumentSession>();
            return func(session);
        }
        public static Task AccessRaven(IServiceProvider services, Func<IAsyncDocumentSession, Task> func)
        {
            var session = services.GetService<IAsyncDocumentSession>();
            return func(session);
        }

        public static Task Store<T>(IServiceProvider services, T document)
        {
            return AccessRaven(services, async session =>
            {
                await session.StoreAsync(document);
                await session.SaveChangesAsync();
            });
        }
    }
}
