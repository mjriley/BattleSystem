using PokeCore;
using Abilities;
using System.Collections.Generic;

namespace Items {

public class EtherItem : BaseItem
{
	public EtherItem(string name, int amount, bool allAbilities = false)
	: base(name, ItemCategory.Resource)
	{
		Name = name;
		Amount = amount;
		AllAbilities = allAbilities;
	}
	
	public int Amount { get; protected set; }
	public bool AllAbilities { get; protected set; }
	
	public override ActionStatus Use(ItemContext context)
	{
		ActionStatus status = new ActionStatus();
		
		string useFormat = L18N.Get("ITEM_USE"); // <X> used a <Y>!
		string useMessage = string.Format(useFormat, context.Player.Name, Name);
		status.events.Add(new StatusUpdateEventArgs(useMessage));
		
		string moveName;
		
		Character pokemon = context.Target;
		List<Ability> abilities;
		if (!AllAbilities)
		{
			int index = context.AbilityIndex;
			abilities = new List<Ability> {pokemon.getAbilities()[index]};
			moveName = pokemon.getAbilities()[index].Name;
		}
		else
		{
			abilities = pokemon.getAbilities();
			moveName = L18N.Get("MSG_ITEM_PP_ALL"); // all its moves
		}
		
		int totalReplenished = 0;
		foreach (Ability ability in abilities)
		{
			totalReplenished += ability.Replenish(Amount);
		}
		
		if (totalReplenished > 0)
		{
			string format = L18N.Get("MSG_ITEM_PP_RESTORE"); // <X> restored <Y>'s PP
			string message = string.Format(format, context.Player.Name, moveName);
			status.events.Add(new StatusUpdateEventArgs(message));
		}
		else
		{
			status.events.Add(new StatusUpdateEventArgs(L18N.Get("MSG_INERT"))); // But it had no effect!
		}
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}

