using System.Collections;
using System.Management.Automation;

namespace celin.state;

public record StateValue(string Label, Dictionary<string, PSObject?> Value);
public class State : IEnumerable<Hashtable>
{
	public string Name { get; }
	public bool Tracing { get; }
	public List<StateValue> States { get; }
	StateValue _current;
	public IEnumerable<PSObject> Trace
		=> States.Select(x =>
		{
			var po = new PSObject();
			po.Properties.Add(new PSNoteProperty("#", x.Label));
			foreach (var v in x.Value)
			{
				po.Properties.Add(new PSNoteProperty(v.Key, v.Value?.ToString()));
			}
			return po;
		}).ToArray();
	public PSObject this[string member]
	{
		get => _current.Value[member];
		set
		{
			if (_current.Value.ContainsKey(member))
			{
				if (_current.Value[member] != null)
				{
					var d = Last.ToDictionary(x => x.Key, x => default(PSObject));
					States.Add(new StateValue(string.Empty, d));
					_current = States.Last();
				}
				_current.Value[member] = value;
			}
			else
			{
				throw new InvalidOperationException($"'${member}' is invalid!");
			}
		}
	}
	protected Dictionary<string, PSObject?> Last
		=> States
			.ReverseTakeWhile(x => string.IsNullOrEmpty(x.Label))
			.Reverse()
			.SelectMany(x => x.Value)
			.GroupBy(x => x.Key)
			.ToDictionary(group => group.Key, group =>
			{
				var l = group.Where(x => x.Value != null);
				return l.Any() ? l.Last().Value : null;
			});
	public void Undo()
	{
		if (Values.Count() > 1)
			States.RemoveAt(States.Count - 1);
		else
			throw new InvalidOperationException("Can't undo initial state");
	}
	public IEnumerable<Hashtable> Labels
	{
		get
		{
			var d = States
				.Where(x => !string.IsNullOrEmpty(x.Label))
				.Select(x =>
				{
					var h = new Hashtable(x.Value)
					{
						{ "#", x.Label }
					};
					return h;
				});
			return d.ToArray();
		}
	}
	public IEnumerable<Hashtable> Values
	{
		get
		{
			var v = States
				.ReverseTakeWhile(x => string.IsNullOrEmpty(x.Label))
				.Reverse();
			var d = v
				.Select((x, i) =>
				{
					var d = v
					.SkipLast(v.Count() - (i + 1))
					.SelectMany(x => x.Value)
					.GroupBy(x => x.Key)
					.ToDictionary(group => group.Key, group =>
					{
						var l = group.Where(x => x.Value != null);
						return l.Any() ? l.Last().Value : null;
					});
					return d;
				});
			var h = d.Select(x => new Hashtable(x));
			return h.ToArray();
		}
	}
	public Hashtable SetLabel(string label, bool clear = false, bool force = false)
	{
		if (force && !Tracing)
		{
			States.RemoveAll(x => label.CompareTo(x.Label) == 0);
		}
		else
		{
			var exist = Tracing ? null : States.Find(x => label.CompareTo(x.Label) == 0);
			if (null != exist)
			{
				throw new InvalidOperationException($"'${label}' already exists!");
			}
		}
		var nextState = clear
			? Last.ToDictionary(x => x.Key, x => default(PSObject))
			: new Dictionary<string, PSObject?>(Last);

		var lb = new StateValue(label, Last);
		States.Add(lb);
		if (!Tracing)
		{
			States.RemoveAll(x => string.IsNullOrEmpty(x.Label));
		}
		States.Add(new StateValue(string.Empty, nextState));
		_current = States.Last();

		var ht = new Hashtable(lb.Value)
					{
						{ "#", lb.Label }
					};
		return ht;
	}
	IEnumerator<Hashtable> IEnumerable<Hashtable>.GetEnumerator()
	{
		IEnumerable<Hashtable> en = new[] { new Hashtable(Last) };
		return en.GetEnumerator();
	}
	IEnumerator IEnumerable.GetEnumerator()
	{
		IEnumerable<Hashtable> en = new[] { new Hashtable(Last) };
		return en.GetEnumerator();
	}
	public State(string name, List<StateValue> states, bool trace)
	{
		Name = name;
		States = states;
		Tracing = trace;
		_current = States.Last();
	}
}
public static class StateMachine
{
	public static IDictionary<PSObject, List<StateValue>> StateNames { get; set; }
		= new Dictionary<PSObject, List<StateValue>>();
	public static State? Default { get; set; }
	public static State Add(string name, string[] members, bool force, bool trace)
	{
		if (StateNames.ContainsKey(name) && force)
		{
			StateNames.Remove(name);
		}
		var d = new Dictionary<string, PSObject?>();
		foreach (var fn in members)
			d.Add(fn, null);
		StateNames.Add(name, new List<StateValue> { new StateValue(string.Empty, d) });
		Default = new State(name, StateNames[name], trace);

		return Default;
	}
}
