using System.Collections;
using System.Management.Automation;

namespace celin.state;

[Cmdlet(VerbsCommon.New, Nouns.Base)]
public class New : PSCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required string Name { get; set; }
	[Parameter(Position = 1, Mandatory = true)]
	public required string[] Members { get; set; }
	[Parameter]
	public SwitchParameter Force { get; set; }
	[Parameter]
	public SwitchParameter UseIfExist { get; set; }
	[Parameter]
	public SwitchParameter Trace { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		if (UseIfExist.IsPresent && StateMachine.StateNames.ContainsKey(Name))
			StateMachine.Default = new State(Name, StateMachine.StateNames[Name], Trace);
		else
			StateMachine.Add(Name, Members, Force, Trace);

		WriteObject(StateMachine.Default);
	}
}
[Cmdlet(VerbsOther.Use, Nouns.Base)]
public class Use : PSCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required string Name { get; set; }
	[Parameter]
	public SwitchParameter Trace { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		var state = StateMachine.StateNames[Name];
		if (state == null)
		{
			throw new ArgumentException($"State '${Name}' does not exist!");
		}

		StateMachine.Default = new State(Name, state, Trace);

		WriteObject(StateMachine.Default);
	}

	[Cmdlet(VerbsCommon.Set, Nouns.Base)]
	[Alias("cstate")]
	public class Set : BaseCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public required string Member { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true)]
		public PSObject? Value { get; set; }
		[Parameter]
		public SwitchParameter FalseIfNull { get; set; }
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			var v = FalseIfNull.IsPresent ? Value ?? false : Value;

			StateMachine.Default[Member] = v;
		}
	}

	[Cmdlet(VerbsCommon.Get, Nouns.Base)]
	public class Get : BaseCmdlet
	{
		[Parameter(Position = 0, Mandatory = true)]
		public required string Label { get; set; }
		[Parameter(Position = 1)]
		public string? Name { get; set; }
		[Parameter()]
		public SwitchParameter FalseIfNone { get; set; }
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			var state = Name == null
				? StateMachine.Default?.States
				: StateMachine.StateNames[Name];

			if (state == null)
				throw new ArgumentException($"State '${Name}' does not exist!");

			var label = state.Find(x => Label.CompareTo(x.Label) == 0);
			if (label == null)
			{
				if (FalseIfNone.IsPresent)
					WriteObject(false);
				else
					throw new ArgumentException($"State ${StateMachine.Default?.Name} does not have label '${Label}'");
			}
			else
				WriteObject(new Hashtable(label.Value));
		}
	}
}
