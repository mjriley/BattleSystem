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
		
		status.events.Add(new StatusUpdateEventArgs(context.Player.Name + " used a " + Name + "!"));
		
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
			moveName = "all its moves";
		}
		
		int totalReplenished = 0;
		foreach (Ability ability in abilities)
		{
			totalReplenished += ability.Replenish(Amount);
		}
		
		if (totalReplenished > 0)
		{
			status.events.Add(new StatusUpdateEventArgs(context.Player.Name + " restored " + moveName + "'s PP"));
		}
		else
		{
			status.events.Add(new StatusUpdateEventArgs("But it had no effect!"));
		}
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}

