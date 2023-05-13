using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;

namespace Tubumu.PatchMapper
{
    public abstract class PatchInput
    {
        [BindNever]
        public ICollection<string>? BoundKeys { get; set; }
    }
}
