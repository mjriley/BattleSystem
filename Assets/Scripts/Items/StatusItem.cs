using PokeCore;

namespace Items {

public class StatusItem : BaseItem
{
	public StatusItem(string name, StatusType status)
	: base(name, ItemCategory.Status)
	{
		Status = status;
	}
	
	public StatusType Status { get; protected set; }
	
	public override ActionStatus Use(ItemContext context)
	{
		ActionStatus status = new ActionStatus();
		
		Character pokemon = context.Target;
		pokemon.ClearStatus(Status);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}