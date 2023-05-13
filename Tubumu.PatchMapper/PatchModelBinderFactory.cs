using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Tubumu.PatchMapper
{
    public class PatchModelBinderFactory : IModelBinderFactory
    {
        private ModelBinderFactory _modelBinderFactory;

        public PatchModelBinderFactory(
            IModelMetadataProvider metadataProvider,
            IOptions<MvcOptions> options,
            IServiceProvider serviceProvider)
        {
            _modelBinderFactory = new ModelBinderFactory(metadataProvider, options, serviceProvider);
        }

        public IModelBinder CreateBinder(ModelBinderFactoryContext context)
        {
            var modelBinder = _modelBinderFactory.CreateBinder(context);
            // ComplexObjectModelBinder 是 internal 类
            if (typeof(PatchInput).IsAssignableFrom(context.Metadata.ModelType)
                && modelBinder.GetType().ToString().EndsWith("ComplexObjectModelBinder"))
            {
                modelBinder = new PatchModelBinder(modelBinder);
            }
            return modelBinder;
        }
    }
}
