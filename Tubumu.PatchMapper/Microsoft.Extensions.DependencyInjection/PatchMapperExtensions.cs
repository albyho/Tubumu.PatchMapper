using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tubumu.PatchMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PatchMapperExtensions
	{
        public static IServiceCollection AddPatchMapper(this IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IModelBinderFactory, PatchModelBinderFactory>());
            return services;
        }
    }
}
