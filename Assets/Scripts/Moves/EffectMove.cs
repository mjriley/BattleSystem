using System;

namespace PokeCore {
namespace Moves {

public class EffectMove : AbstractMove
{
	public delegate void BasicEffect(AbstractMove move, Character actor, Player targetPlayer, ref ActionStatus status);
	
	BasicEffect EffectImpl;
	Random m_generator;
	
	public int Accuracy { get; protected set; }
	
	public EffectMove(string name, MoveType moveType, BattleType battleType, int maxPP, BasicEffect effectImpl, int accuracy=100, 
		int priority=0, string description="", Random generator=null)
	: base(name, moveType, battleType, maxPP, priority, description)
	{
		EffectImpl = effectImpl;
		
		if (generator == null)
		{
			generator = new Random();
		}
		m_generator = generator;
		
		Accuracy = accuracy;
	}
	
	protected bool CheckHit()
	{
		int rand = m_generator.Next(100);
		
		return (rand < Accuracy);
	}
	
	protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IMoveImpl parentMove=null)
	{
		if (!CheckHit())
		{
			string format = L18N.Get("MOVE_MISSED"); // <X> missed!
			string message = string.Format(format, actor.Name);
			status.events.Add(new StatusUpdateEventArgs(message));
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		EffectImpl(this, actor, targetPlayer, ref status);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
}

}}