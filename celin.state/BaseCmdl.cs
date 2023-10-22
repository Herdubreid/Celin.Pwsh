using System.Management.Automation;

namespace celin.state;

static class Nouns
{
	public const string Base = "Celin.State";
}
public class BaseCmdlet : PSCmdlet
{
	protected override void BeginProcessing()
	{
		base.BeginProcessing();

		if (StateMachine.Current == null)
			throw new InvalidOperationException("No current state!");
	}
}
