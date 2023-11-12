namespace celin.state;

public static class ListExtension
{
	public static IEnumerable<T> ReverseTakeWhile<T>(this List<T> source, Func<T, bool> predicate)
	{
		for (int i = source.Count - 1; i >= 0; i--)
		{
			if (!predicate(source[i]))
			{
				yield break; // Stop when the condition is no longer met
			}

			yield return source[i];
		}
	}
}
