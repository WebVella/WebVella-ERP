using System.Linq;
using System.Reflection;

namespace WebVella.Erp.Api.Models.Mapping;

public class PropertyCopyStrategy<T> : IMapStrategy<T, T> where T : new()
{
	private static readonly PropertyInfo[] Properties = typeof(T)
		.GetProperties(BindingFlags.Public | BindingFlags.Instance)
		.Where(p => p.CanRead && p.CanWrite)
		.ToArray();

	public static readonly PropertyCopyStrategy<T> Instance = new();

	public T Map(T source)
	{
		if (source == null) return default!;

		var target = new T();
		foreach (var prop in Properties)
		{
			var value = prop.GetValue(source);
			prop.SetValue(target, value);
		}

		return target;
	}
}
