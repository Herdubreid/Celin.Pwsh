using System.Management.Automation;

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

[Cmdlet(VerbsCommon.Set, Nouns.Base)]
[Alias("cs")]
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
[Alias("cg")]
public class Get : BaseCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required object Member { get; set; }
	[Parameter]
	public SwitchParameter Label { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		if (Label.IsPresent)
		{
			var label = StateMachine.Current.States.Find(x => x.Label == Member);
			if (label != null)
			{
				WriteObject(label);
			}
			else
			{
				throw new InvalidOperationException($"State ${StateMachine.Current.Name} does not have label '${Member}'");
			}
		}
		else
		{
			WriteObject(StateMachine.Current[Member.ToString()]);
		}
	}
}

[Cmdlet(VerbsLifecycle.Resume, Nouns.Base)]
public class Resume : BaseCmdlet
{
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		StateMachine.Current.Resume();
	}
}

[Cmdlet(VerbsCommon.Undo, Nouns.Base)]
public class Undo : BaseCmdlet
{
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		StateMachine.Current.Undo();
	}
}
[Cmdlet(VerbsLifecycle.Confirm, Nouns.Base)]
public class Confirm : BaseCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required PSObject Label { get; set; }
	[Parameter]
	public SwitchParameter Override { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		StateMachine.Current.Label(Label, Override.IsPresent);
	}
}