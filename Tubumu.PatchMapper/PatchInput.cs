using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tubumu.PatchMapper
{
    public abstract class PatchInput : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [BindNever]
        public ICollection<string>? PatchKeys { get; set; }
    }
}
