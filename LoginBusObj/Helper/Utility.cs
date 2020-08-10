using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoginBusObj.Helper
{
    public static class Utility
    {
        public static TTarget Migration<TSource, TTarget>(TSource sourceInstance)
           where TSource : class, new()
        {
            var sourceType = sourceInstance.GetType();
            var targetType = typeof(TTarget);
            var sourceProperties = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var targetProperties = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var targetInstance = Activator.CreateInstance<TTarget>();

            var mappingAttributeType = typeof(ObjectMappingAttribute);
            foreach (var sourceProperty in sourceProperties)
            {
                var sourcePropertyName = sourceProperty.Name;
                var sourceValue = sourceProperty.GetValue(sourceInstance);

                foreach (var targetProperty in targetProperties)
                {
                    var targetPropertyName = targetProperty.Name;
                    if (sourcePropertyName == targetPropertyName)
                    {
                        if (sourceProperty.PropertyType == targetProperty.PropertyType)
                        {
                            targetProperty.SetValue(targetInstance, sourceValue);
                            break;
                        }
                    }
                    var mappingAttributes = targetProperty.GetCustomAttributes(mappingAttributeType, false);
                    if (mappingAttributes.Any())
                    {
                        var mappingAttributePropertyName = ((ObjectMappingAttribute)mappingAttributes[0]).PropertyName;
                        if (mappingAttributePropertyName == sourcePropertyName)
                        {
                            if (sourceProperty.PropertyType == targetProperty.PropertyType)
                            {
                                targetProperty.SetValue(targetInstance, sourceValue);
                                break;
                            }
                        }

                    }

                }
            }
            return targetInstance;
        }

        public static IEnumerable<TTarget> MigrationIEnumerable<TSource, TTarget>(this IEnumerable<TSource> sourceInstanceList)
           where TSource : class, new()
        {
            List<TTarget> targetInstance = new List<TTarget>();
            foreach (var sourceInstance in sourceInstanceList)
                targetInstance.Add(Migration<TSource, TTarget>(sourceInstance));

            return targetInstance;
        }
    }
}
