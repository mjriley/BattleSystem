using PokeCore;

namespace Abilities {

	// An ability that performs the same action until it misses or the max number of turns expire
	public class RepeatAbility : AbstractAbility, IRepeatAbility
	{
		public int CurrentTurn { get; protected set; }
		public int MaxTurns { get; protected set; }
		
		AbstractAbility m_ability;
		
		public RepeatAbility(AbstractAbility childAbility, int maxTurns)
		: base(childAbility.Name, childAbility.AbilityType, childAbility.BattleType, childAbility.MaxPP, childAbility.Priority, childAbility.Description)
		{
			m_ability = childAbility;
			CurrentTurn = 0;
			MaxTurns = maxTurns;
		}
		
		public override void Reset()
		{
			base.Reset();
			CurrentTurn = 0;
		}
		
		protected override ActionStatus ExecuteImpl(Character actor, Player targetPlayer, ref ActionStatus status, IAbilityImpl parentAbility=null)
		{
			ActionStatus childStatus = m_ability.Execute(actor, targetPlayer, this);
			
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
	
}
