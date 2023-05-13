using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Newtonsoft.Json.Linq;

namespace Tubumu.PatchMapper
{
    public class PatchConverter<T> : ITypeConverter<PatchInput, T> where T : new()
    {
        /// <inheritdoc />
        public T Convert(PatchInput source, T destination, ResolutionContext context)
        {
            destination ??= new T();

            var sourceType = source.GetType();
            var destinationType = typeof(T);

            foreach (var key in source.PatchKeys ?? Enumerable.Empty<string>())
            {
                var sourcePropertyInfo = sourceType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (sourcePropertyInfo != null)
                {
                    var destinationPropertyInfo = destinationType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (destinationPropertyInfo != null)
                    {
                        var sourceValue = sourcePropertyInfo.GetValue(source);
                        destinationPropertyInfo.SetValue(destination, sourceValue);
                    }
                }
            }

            return destination;
        }
    }
}
