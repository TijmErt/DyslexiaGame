using Unity.VisualScripting;

[UnitTitle("On Dialogue Start")]
[UnitCategory("Events\\Custom Events")]
public class OnDialogueStart : EventUnit<EmptyEventArgs>
{
	
	protected override bool register => true;
	
	public override EventHook GetHook(GraphReference reference)
	{
		return new EventHook("OnDialogueStart", reference.self);
	}
}