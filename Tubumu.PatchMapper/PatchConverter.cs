using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;

namespace Tubumu.PatchMapper
{
    public class PatchConverter<T> : ITypeConverter<PatchInput, T> where T : new()
    {
        private static readonly IDictionary<string, Action<T, object?>> _propertySetters;
        private static readonly IDictionary<string, Func<PatchInput, object?>> _propertyGetters;

        static PatchConverter()
        {
            _propertySetters = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(p => p.Name, CreatePropertySetter, StringComparer.OrdinalIgnoreCase);
            _propertyGetters = typeof(PatchInput).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(p => p.Name, CreatePropertyGetter, StringComparer.OrdinalIgnoreCase);
        }

        private static Action<T, object?> CreatePropertySetter(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType!;
            var parameterExpression = Expression.Parameter(typeof(object), "value");
            var castExpression = Expression.Convert(parameterExpression, propertyInfo.PropertyType);
            var targetExpression = Expression.Parameter(targetType, "target");
            var propertyExpression = Expression.Property(targetExpression, propertyInfo);
            var nullExpression = Expression.Constant(null, typeof(object));
            var conditionExpression = Expression.NotEqual(parameterExpression, nullExpression);
            var assignExpression = Expression.IfThen(conditionExpression, Expression.Assign(propertyExpression, castExpression));
            var lambdaExpression = Expression.Lambda<Action<T, object?>>(assignExpression, targetExpression, parameterExpression);
            return lambdaExpression.Compile();
        }

        private static Func<PatchInput, object?> CreatePropertyGetter(PropertyInfo propertyInfo)
        {
            var sourceParameterExpression = Expression.Parameter(typeof(PatchInput), "source");
            var sourcePropertyExpression = Expression.Property(sourceParameterExpression, propertyInfo);
            var sourceCastExpression = Expression.Convert(sourcePropertyExpression, typeof(object));
            var sourceLambdaExpression = Expression.Lambda<Func<PatchInput, object?>>(sourceCastExpression, sourceParameterExpression);
            return sourceLambdaExpression.Compile();
        }

        /// <inheritdoc />
        public T Convert(PatchInput source, T destination, ResolutionContext context)
        {
            destination ??= new T();

            foreach (var key in source.PatchKeys ?? Enumerable.Empty<string>())
            {
                if (_propertyGetters.TryGetValue(key, out var propertyGetter))
                {
                    var sourceValue = propertyGetter(source);
                    if (_propertySetters.TryGetValue(key, out var propertySetter))
                    {
                        propertySetter(destination, sourceValue);
                    }
                }
            }

            return destination;
        }
    }
}
