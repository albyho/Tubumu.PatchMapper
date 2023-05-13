using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tubumu.PatchMapper
{
    public abstract class PatchInput
    {
        [BindNever]
        public ICollection<string>? PatchKeys { get; set; }
    }
}
