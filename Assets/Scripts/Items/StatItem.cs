using PokeCore;
using Pokemon;

namespace Items {

public class StatItem : BaseItem
{
	public StatItem(string name, Stat stat, int stages)
	: base(name, ItemCategory.Battle)
	{
		this.Stat = stat;
		Stages = stages;
	}
	
	public Stat Stat { get; protected set; }
	public int Stages { get; protected set; }
	
	public override ActionStatus Use(ItemContext context)
	{
		ActionStatus status = new ActionStatus();
		
		Character pokemon = context.Target;
		int actualChange = pokemon.ModifyStage(Stat, Stages);
		
		string message = Character.GetStageModificationMessage(context.Target.Name, Stat, actualChange, (Stages > 0));
		
		status.events.Add(new StatusUpdateEventArgs(context.Player.Name + " used a " + Name + "!"));
		status.events.Add(new StatusUpdateEventArgs(message));
		
		status.isComplete = true;
		status.turnComplete = true;
		
		return status;
	}
}

}