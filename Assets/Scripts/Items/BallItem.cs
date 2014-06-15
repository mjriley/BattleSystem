using PokeCore;
using System.Globalization;

namespace Items {

public class BallItem : BaseItem
{
	public BallItem(string name, double catchRate)
	: base(name, ItemCategory.Balls)
	{
		CatchRate = catchRate;
	}
	
	public double CatchRate { get; protected set; }
	
	// Not fully implemented. Right now does not allow pokemon to be captured
	public override ActionStatus Use(ItemContext context)
	{
		ActionStatus status = new ActionStatus();
		
		status.events.Add(new StatusUpdateEventArgs(context.Player.Name + " used a " + Name + "!"));
		status.events.Add(new StatusUpdateEventArgs(context.Target.Owner.Name + " blocked your " + Name + "!"));
		status.events.Add(new StatusUpdateEventArgs("Don't be a thief!"));
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}

