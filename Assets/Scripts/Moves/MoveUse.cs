using System.Collections.Generic;

namespace PokeCore {
namespace Moves {

public class MoveUse : ITurnAction
{
	public Character m_actor;
	public Player m_targetPlayer;
	public Move m_move;
	
	public MoveUse(Character actor, Player targetPlayer, Move move)
	{
		m_actor = actor;
		m_targetPlayer = targetPlayer;
		m_move = move;
	}
	
	public string Name { get { return m_move.Name; } }
	
	public virtual ICharacter Subject { get { return m_actor; } }
	
	public virtual ActionStatus Execute()
	{
		return m_move.Execute(m_actor, m_targetPlayer);
	}
	
	public int Priority { get { return m_move.Priority; } } 
	
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		return true;
	}
}

}}