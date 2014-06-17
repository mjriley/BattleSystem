using System;
using System.Collections.Generic;
using Moves;

namespace PokeCore {

public class RandomMoveRequest : IActionRequest
{
	Random m_generator;
	
	public RandomMoveRequest(Random generator = null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		m_generator = generator;
	}
	
	Move DetermineMove(Character actor)
	{
		uint cost = actor.getUsageCost();
		
		List<Move> moves = actor.getMoves();
		
		Move move = null;
		
		while (moves.Count > 0)
		{
			int moveIndex = m_generator.Next(0, moves.Count);
			move = moves[moveIndex];
			
			if (move.CurrentPP < cost)
			{
				moves.Remove(move);
			}
			else
			{
				return move;
			}
		}
		
		return null;
	}
	
	// Not utilized -- should be refactored
	public void SubmitAction(ITurnAction action)
	{
	
	}
	
	public void GetAction(Player player, Player enemyPlayer, NewBattleSystem.ProcessAction actionCallback)
	{
		Move move = DetermineMove(player.ActivePokemon);
		MoveUse action = new MoveUse(player.ActivePokemon, enemyPlayer, move);
		actionCallback(action);
	}
	
	public RequestType RequestType { get { return RequestType.Turn; } }
}

}

