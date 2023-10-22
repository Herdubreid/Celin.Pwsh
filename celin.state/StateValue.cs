using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace celin.state;

public record StateValue(PSObject? Label, Dictionary<PSObject, PSObject?> Value);
public class State : IEnumerable<StateValue>
{
	public PSObject Name { get; }
	public List<StateValue> States { get; }
	StateValue _current;
	public PSObject this[PSObject member]
	{
		get => _current.Value[member];
		set
		{
			if (_current.Value.ContainsKey(member))
			{
				if (_current.Value[member] != null)
				{
					var d = new Dictionary<PSObject, PSObject>();
					States.Add(new StateValue(null, d));
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
	public Dictionary<PSObject, PSObject?> Last
		=> States
			.Where(x => x.Label == null)
			.SelectMany(x => x.Value)
			.GroupBy(x => x.Key)
			.ToDictionary(group => group.Key, group =>
			{
				var l = group.Where(x => x.Value != null);
				return l.Any() ? l.Last().Value : null;
			});
	public Hashtable? Value => new Hashtable(Last);
	public void Resume()
		=> _current = States.Last();
	public void Undo()
	{
		if (States.Count > 1)
			States.RemoveAt(States.Count - 1);

		throw new InvalidOperationException("Can't undo initial state");
	}
	public void Label(PSObject Label, bool force)
	{
		if (force)
		{
			States.RemoveAll(x => x.Label == Label);
		}
		else
		{
			if (null != States.Find(x => x.Label == Label))
			{
				throw new InvalidOperationException($"'${Label}' already exists!");
			}
		}
		States.Add(new StateValue(Label, Last));
		States.Add(new StateValue(null, new Dictionary<PSObject, PSObject?>()));
		States.RemoveAll(x => x.Label == null);
		_current = States.Last();
	}

	IEnumerator IEnumerable.GetEnumerator()
		=> States.GetEnumerator();

	IEnumerator<StateValue> IEnumerable<StateValue>.GetEnumerator()
		=> States.GetEnumerator();

	public State(PSObject name, List<StateValue> states)
	{
		Name = name;
		States = states;
		_current = States.Last();
	}
}
public static class StateMachine
{
	public static IDictionary<PSObject, List<StateValue>> States { get; set; }
		= new Dictionary<PSObject, List<StateValue>>();
	public static State? Current { get; set; }
	public static State Add(PSObject name, PSObject[] members, bool force)
	{
		if (States.ContainsKey(name) && force)
		{
			States.Remove(name);
		}
		var d = new Dictionary<PSObject, PSObject?>();
		foreach (var fn in members)
			d.Add(fn, null);
		States.Add(name, new List<StateValue> { new StateValue(null, d) });
		Current = new State(name, States.Last().Value);

		return Current;
	}
}
