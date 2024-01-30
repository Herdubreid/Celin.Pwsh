using System.Dynamic;
using System.Collections;
using System.Management.Automation;

namespace celin.state;

public record StateValue(string Label, Dictionary<string, object?> Value);
public class State : DynamicObject, IEnumerable<Hashtable>
{
    public string GetName { get; }
    public bool GetTracing { get; }
    public List<StateValue> GetStates { get; }
    StateValue _current;
    public IEnumerable<PSObject> GetTrace
        => GetStates.Select(x =>
        {
            var po = new PSObject();
            po.Properties.Add(new PSNoteProperty("#", x.Label));
            foreach (var v in x.Value)
            {
                po.Properties.Add(new PSNoteProperty(v.Key, v.Value?.ToString()));
            }
            return po;
        }).ToArray();
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        if (!_current.Value.ContainsKey(binder.Name))
            throw new PSArgumentException("Invalid Variable", binder.Name);
        result = _current.Value[binder.Name];
        return true;
    }
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (!_current.Value.ContainsKey(binder.Name))
            throw new PSArgumentException("Invalid Variable", binder.Name);
        if (_current.Value[binder.Name] != null)
        {
            var d = GetLast.ToDictionary(x => x.Key, x => default(object));
            GetStates.Add(new StateValue(string.Empty, d));
            _current = GetStates.Last();
        }
        _current.Value[binder.Name] = value;
        return true;
    }
    protected Dictionary<string, object?> GetLast
        => GetStates
            .ReverseTakeWhile(x => string.IsNullOrEmpty(x.Label))
            .Reverse()
            .SelectMany(x => x.Value)
            .GroupBy(x => x.Key)
            .ToDictionary(group => group.Key, group =>
            {
                var l = group.Where(x => x.Value != null);
                return l.Any() ? l.Last().Value : null;
            });
    public void UndoLast()
    {
        if (GetValues.Count() > 1)
            GetStates.RemoveAt(GetStates.Count - 1);
        else
            throw new InvalidOperationException("Can't undo initial state");
    }
    public IEnumerable<Hashtable> GetLabels
    {
        get
        {
            var d = GetStates
                .Where(x => !string.IsNullOrEmpty(x.Label))
                .Select(x =>
                {
                    var h = new Hashtable(x.Value)
                    {
                        { "#", x.Label }
                    };
                    return h;
                });
            return d
                .Reverse()
                .ToArray();
        }
    }
    public IEnumerable<Hashtable> GetValues
    {
        get
        {
            var v = GetStates
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
            return d
                .Select(x => new Hashtable(x))
                .Reverse()
                .ToArray();
        }
    }
    public Hashtable SetLabel(string label, bool clear = false, bool force = false)
    {
        if (force && !GetTracing)
        {
            GetStates.RemoveAll(x => label.CompareTo(x.Label) == 0);
        }
        else
        {
            var exist = GetTracing ? null : GetStates.Find(x => label.CompareTo(x.Label) == 0);
            if (null != exist)
            {
                throw new InvalidOperationException($"'${label}' already exists!");
            }
        }
        var nextState = clear
            ? GetLast.ToDictionary(x => x.Key, x => default(object))
            : new Dictionary<string, object?>(GetLast);

        var lb = new StateValue(label, GetLast);
        GetStates.Add(lb);
        if (!GetTracing)
        {
            GetStates.RemoveAll(x => string.IsNullOrEmpty(x.Label));
        }
        GetStates.Add(new StateValue(string.Empty, nextState));
        _current = GetStates.Last();

        var ht = new Hashtable(lb.Value)
                    {
                        { "#", lb.Label }
                    };
        return ht;
    }
    public IEnumerator<Hashtable> GetEnumerator()
    {
        IEnumerable<Hashtable> en = new[] { new Hashtable(GetLast) };
        return en.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    public State(string name, List<StateValue> states, bool trace)
    {
        GetName = name;
        GetStates = states;
        GetTracing = trace;
        _current = GetStates.Last();
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
        var d = new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var fn in members)
            d.Add(fn, null);
        StateNames.Add(name, new List<StateValue> { new StateValue(string.Empty, d) });
        Default = new State(name, StateNames[name], trace);

        return Default;
    }
}
