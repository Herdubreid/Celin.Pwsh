using System.Diagnostics.CodeAnalysis;

namespace celin.po;

public class ObjectEqualityComparer : IEqualityComparer<object?>
{
	public new bool Equals(object? x, object? y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (x is string && y is string)
		{
			string sx = (string)x;
			string sy = (string)y;
			return sx.TrimEnd().Equals(sy.TrimEnd());
		}
		return x.Equals(y);
	}

	public int GetHashCode([DisallowNull] object? obj)
	{
		if (obj is string)
		{
			string s = (string)obj;
			return (s.TrimEnd()).GetHashCode();
		}
		return obj.GetHashCode();
	}
}
