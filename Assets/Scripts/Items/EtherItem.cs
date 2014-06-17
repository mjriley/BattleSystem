using PokeCore;
using Moves;
using System.Collections.Generic;

namespace Items {

public class EtherItem : BaseItem
{
	public EtherItem(string name, int amount, bool allMoves = false)
	: base(name, ItemCategory.Resource)
	{
		Name = name;
		Amount = amount;
		AllMoves = allMoves;
	}
	
	public int Amount { get; protected set; }
	public bool AllMoves { get; protected set; }
	
	public override ActionStatus Use(ItemContext context)
	{
		ActionStatus status = new ActionStatus();
		
		string useFormat = L18N.Get("ITEM_USE"); // <X> used a <Y>!
		string useMessage = string.Format(useFormat, context.Player.Name, Name);
		status.events.Add(new StatusUpdateEventArgs(useMessage));
		
		string moveName;
		
		Character pokemon = context.Target;
		List<Move> moves;
		if (!AllMoves)
		{
			int index = context.MoveIndex;
			moves = new List<Move> {pokemon.getMoves()[index]};
			moveName = pokemon.getMoves()[index].Name;
		}
		else
		{
			moves = pokemon.getMoves();
			moveName = L18N.Get("MSG_ITEM_PP_ALL"); // all its moves
		}
		
		int totalReplenished = 0;
		foreach (Move move in moves)
		{
			totalReplenished += move.Replenish(Amount);
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

