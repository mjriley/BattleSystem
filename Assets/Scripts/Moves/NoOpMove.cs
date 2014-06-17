using PokeCore;

namespace Moves {

public class NoOpMove : AbstractMove
{
	string m_moveMessage;
	
	public NoOpMove(string name, MoveType moveType, BattleType battleType, int maxPP, string moveMessage = "", int priority=0, string description="")
	: base(name, moveType, battleType, maxPP, priority, description)
	{
		m_moveMessage = moveMessage;
	}
	
	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IMoveImpl parentMove=null)
	{
		if (m_moveMessage != "")
		{
			status.events.Add(new StatusUpdateEventArgs(m_moveMessage));
		}
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}