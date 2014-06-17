using PokeCore;

namespace PokeCore {
namespace Items {

public class RestoreItem : BaseItem
{
	public RestoreItem(string name, int amount)
	: base(name, ItemCategory.Resource)
	{
		Amount = amount;
	}
	
	public int Amount { get; protected set; }
	
	public override ActionStatus Use(ItemContext context)
	{
		ActionStatus status = new ActionStatus();
		Character target = context.Target;
		int actualAmount = target.TakeDamage(-Amount);
		
		string format = L18N.Get("MSG_ITEM_USE"); // <X> used a <Y>!
		string message = string.Format(format, context.Player.Name, Name);
		DamageEventArgs healEvent = new DamageEventArgs(context.Player, actualAmount);
		status.events.Add(new StatusUpdateEventArgs(message));
		status.events.Add(healEvent);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}}
