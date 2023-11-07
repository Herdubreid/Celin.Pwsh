using System.Management.Automation;

namespace celin.po;

static class Nouns
{
	public const string Base = "Celin.Po";
}
[Cmdlet(VerbsCommon.New, Nouns.Base)]
[OutputType(typeof(PSObject))]
public class New : PSCmdlet
{
	[Parameter(Position = 0, Mandatory = true)]
	public required string[] Members { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		var po = new PSObject();
		foreach (var member in Members)
		{
			po.Properties.Add(new PSNoteProperty(member, null));
		}
		WriteObject(po);
	}
}
[Cmdlet(VerbsCommon.Add, Nouns.Base)]
[OutputType(typeof(PSObject))]
[Alias("cpo")]
public class Add : PSCmdlet
{
	[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
	public required PSObject[] Objects { get; set; }
	protected override void ProcessRecord()
	{
		base.ProcessRecord();

		var po = new PSObject();
		foreach (var obj in Objects)
		{
			var e = obj.Enumerate();
			foreach (var member in e)
			{
				if (po.Properties.Match(member.Key).Count() > 0)
				{
					po.Properties[member.Key].Value = member.Value;
				}
				else
				{
					po.Properties.Add(new PSNoteProperty(member.Key, member.Value));
				}
			}
		}
		WriteObject(po);
	}
}
