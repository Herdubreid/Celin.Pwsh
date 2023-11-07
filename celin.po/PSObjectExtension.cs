using System.Collections;
using System.Management.Automation;

namespace celin.po;
public static class PSObjectExtensions
{
	public static IEnumerable<KeyValuePair<string, object?>> Enumerate(this PSObject pso)
	{
		var result = new List<KeyValuePair<string, object?>>();
		switch (pso.BaseObject)
		{
			case PSCustomObject:
				foreach (var e in pso.Properties)
				{
					result.Add(new(e.Name, e.Value));
				}
				break;
			case Hashtable ht:
				var dict = ht.GetEnumerator();
				while (dict.MoveNext())
				{
					result.Add(new((string)dict.Key, dict.Value));
				}
				break;
			default:
				throw new ArgumentException($"No Enumerator on ${pso}");
		};
		return result;
	}
}
