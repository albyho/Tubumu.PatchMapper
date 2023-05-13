using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tubumu.PatchMapper
{
    public class PatchModelBinder : IModelBinder
    {
        private readonly IModelBinder _internalModelBinder;

        public PatchModelBinder(IModelBinder internalModelBinder)
        {
            _internalModelBinder = internalModelBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _internalModelBinder.BindModelAsync(bindingContext);
            if (bindingContext.Model is PatchInput model)
            {
                model.BoundKeys = bindingContext.HttpContext.Request.Form.Keys;
            }
        }
    }
}
