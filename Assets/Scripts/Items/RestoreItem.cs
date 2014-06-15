using PokeCore;

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
		
		string message = context.Player.Name + " used a " + Name + "!";
		DamageEventArgs healEvent = new DamageEventArgs(context.Player, actualAmount);
		status.events.Add(new StatusUpdateEventArgs(message));
		status.events.Add(healEvent);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}

