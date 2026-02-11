using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using DevHabitApi.DTOs.Common;

namespace DevHabitApi.Services;

public sealed class DatashapingService
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertiesCache = new();

    public ExpandoObject ShapeData<T>(T entity, string? fields)
    {
        var fieldsSet = fields?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(
            typeof(T),
            t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public));

        if (fieldsSet.Any())
        {
            propertyInfos = propertyInfos
                .Where(p => fieldsSet.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                .ToArray();
        }


        IDictionary<string, object?> shapedObject = new ExpandoObject();

        foreach (var propertyInfo in propertyInfos)
        {
            shapedObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
        }


        return (ExpandoObject)shapedObject;
    }


    public List<ExpandoObject> ShapeDataCollection<T>(
        IEnumerable<T> entities,
        string? fields,
        Func<T, List<LinkDto>>? linksFactory = null)
    {
        var fieldsSet = fields?
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(
            typeof(T),
            t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public));

        if (fieldsSet.Any())
        {
            propertyInfos = propertyInfos
                .Where(p => fieldsSet.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                .ToArray();
        }

        List<ExpandoObject> shapedObjects = [];
        foreach (T entity in entities)
        {
            IDictionary<string, object?> shapedObject = new ExpandoObject();

            foreach (var propertyInfo in propertyInfos)
            {
                shapedObject[propertyInfo.Name] = propertyInfo.GetValue(entity);
            }

            if(linksFactory is not null)
            {
                shapedObject["link"] = linksFactory(entity);
            }

            shapedObjects.Add((ExpandoObject)shapedObject);
        }

        return shapedObjects;
    }

    public bool Validate<T>(string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
        {
            return true;
        }

        var fieldsSet = fields
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        PropertyInfo[] propertyInfos = PropertiesCache.GetOrAdd(
           typeof(T),
           t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        return fieldsSet.All(f => propertyInfos.Any(p => string.Equals(p.Name, f, StringComparison.OrdinalIgnoreCase)));
    }
}
