using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;

namespace Tubumu.PatchMapper
{
    public class PatchConverter<T> : ITypeConverter<PatchInput, T> where T : new()
    {
        private static readonly IDictionary<string, Action<T, object>> _propertySetters;

        static PatchConverter()
        {
            _propertySetters = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(p => p.Name, CreatePropertySetter);
        }

        private static Action<T, object> CreatePropertySetter(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType!;
            var parameterExpression = Expression.Parameter(typeof(object), "value");
            var castExpression = Expression.Convert(parameterExpression, propertyInfo.PropertyType);
            var targetExpression = Expression.Parameter(targetType, "target");
            var propertyExpression = Expression.Property(targetExpression, propertyInfo);
            var assignExpression = Expression.Assign(propertyExpression, castExpression);
            var lambdaExpression = Expression.Lambda<Action<T, object>>(assignExpression, targetExpression, parameterExpression);
            return lambdaExpression.Compile();
        }

        /// <inheritdoc />
        public T Convert(PatchInput source, T destination, ResolutionContext context)
        {
            destination ??= new T();

            foreach (var key in source.PatchKeys ?? Enumerable.Empty<string>())
            {
                if (_propertySetters.TryGetValue(key, out var propertySetter))
                {
                    var sourceValue = source.GetType().GetProperty(key)?.GetValue(source)!;
                    propertySetter(destination, sourceValue);
                }
            }

            return destination;
        }
    }
}
