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
		
		string useFormat = L18N.Get("MSG_ITEM_USE"); // <X> used a <Y>!
		string useMessage = string.Format(useFormat, context.Player.Name, Name);
		status.events.Add(new StatusUpdateEventArgs(useMessage));
		
		string blockFormat = L18N.Get("MSG_ITEM_BALL_BLOCK"); // <X> blocked your <Y>!
		string blockMessage = string.Format(blockFormat, context.Target.Owner.Name, Name);
		status.events.Add(new StatusUpdateEventArgs(blockMessage));
		
		string thiefMessage = L18N.Get("MSG_ITEM_BALL_SCOLD"); // Don't be a thief!
		status.events.Add(new StatusUpdateEventArgs(thiefMessage));
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}

