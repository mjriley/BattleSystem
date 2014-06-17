namespace PokeCore {
namespace Moves {

// A move that performs the same action until it misses or the max number of turns expire
public class RepeatMove : AbstractMove, IRepeatMove
{
	public int CurrentTurn { get; protected set; }
	public int MaxTurns { get; protected set; }
	
	AbstractMove m_move;
	
	public RepeatMove(AbstractMove childMove, int maxTurns)
	: base(childMove.Name, childMove.MoveType, childMove.BattleType, childMove.MaxPP, childMove.Priority, childMove.Description)
	{
		m_move = childMove;
		CurrentTurn = 0;
		MaxTurns = maxTurns;
	}
	
	public override void Reset()
	{
		base.Reset();
		CurrentTurn = 0;
	}
	
	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IMoveImpl parentMove=null)
	{
		ActionStatus childStatus = m_move.Execute(actor, targetPlayer, this);
		
		status.turnComplete = childStatus.turnComplete;
		
		if (childStatus.isComplete == true)
		{
			// if status contains a hit event
				CurrentTurn += 1;
			// else
				// Reset();
				
			if (CurrentTurn == MaxTurns)
			{
				status.turnComplete = true;
				status.isComplete = true;
				
				Reset();
			}
		}
		
		status.events.AddRange(childStatus.events);
		
		return status;
	}
}
	
}}