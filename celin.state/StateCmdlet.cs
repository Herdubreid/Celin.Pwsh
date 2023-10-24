using System.Collections;
using System.Management.Automation;
using System.Reflection.Emit;

namespace celin.state;

[Cmdlet(VerbsCommon.New, Nouns.Base)]
public class New : PSCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required PSObject Name { get; set; }
	[Parameter(Position = 1, Mandatory = true)]
	public required PSObject[] Members { get; set; }
	[Parameter]
	public SwitchParameter Force { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		var state = StateMachine.Add(Name, Members, Force.IsPresent);

		WriteObject(state);
	}
}
[Cmdlet(VerbsOther.Use, Nouns.Base)]
public class Use : PSCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required PSObject Name { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		var state = StateMachine.States[Name];
		if (state == null)
		{
			throw new ArgumentException($"State '${Name}' does not exist!");
		}

		StateMachine.Current = new State(Name, state);

		WriteObject(StateMachine.Current);
	}

	[Cmdlet(VerbsCommon.Set, Nouns.Base)]
	[Alias("cset", "cs")]
	public class Set : BaseCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public required PSObject Member { get; set; }
		[Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
		public required PSObject Value { get; set; }
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			StateMachine.Current[Member] = Value;
		}
	}

	[Cmdlet(VerbsCommon.Get, Nouns.Base)]
	public class Get : BaseCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public required PSObject Label { get; set; }
		[Parameter(Position = 1)]
		public PSObject? Name { get; set; }
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			var state = Name == null
				? StateMachine.Current?.States
				: StateMachine.States[Name];

			if (state == null)
				throw new ArgumentException($"State '${Name}' does not exist!");

			var label = state.Find(x => Label.CompareTo(x.Label) == 0);

			if (label == null)
				throw new ArgumentException($"State ${StateMachine.Current?.Name} does not have label '${Label}'");

			WriteObject(new Hashtable(label.Value));
		}
	}

	[Cmdlet(VerbsLifecycle.Resume, Nouns.Base)]
	public class Resume : BaseCmdlet
	{
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			StateMachine.Current?.Resume();
		}
	}

	[Cmdlet(VerbsCommon.Undo, Nouns.Base)]
	public class Undo : BaseCmdlet
	{
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			StateMachine.Current?.Undo();
		}
	}
	[Cmdlet(VerbsLifecycle.Confirm, Nouns.Base)]
	public class Confirm : BaseCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public required PSObject Label { get; set; }
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			StateMachine.Current?.SetLabel(Label);
		}
	}
}
