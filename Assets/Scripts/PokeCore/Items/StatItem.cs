using PokeCore.Pokemon;

namespace PokeCore {
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
		
		string changeMessage = Character.GetStageModificationMessage(context.Target.Name, Stat, actualChange, (Stages > 0));
		
		string useFormat = L18N.Get("MSG_ITEM_USE"); // <X> used a <Y>!
		string useMessage = string.Format(useFormat, context.Player.Name, Name);
		status.events.Add(new StatusUpdateEventArgs(useMessage));
		status.events.Add(new StatusUpdateEventArgs(changeMessage));
		
		status.isComplete = true;
		status.turnComplete = true;
		
		return status;
	}
}

}}