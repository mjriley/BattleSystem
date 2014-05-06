using System;

public class DamageAbility : AbstractAbility
{
	public uint Power { get; protected set; }
	public int Accuracy { get; protected set; }
	
	private Random m_generator;
	
	public DamageAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, uint power, int accuracy, string description="", Random generator=null)
	: base(name, abilityType, battleType, maxPP, description)
	{
		Power = power;
		Accuracy = accuracy;
		
		if (generator == null)
		{
			generator = new Random();
		}
		
		m_generator = generator;
	}
	
	protected override ActionStatus ExecuteImpl(Character attacker, Player targetPlayer, ref ActionStatus status)
	{
		Character defender = targetPlayer.ActivePokemon;
		
		if (!CheckHit())
		{
			status.events.Add(new StatusUpdateEventArgs(attacker.Name + " missed!"));
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		ApplyDamage(attacker, defender, ref status);
		
		HandleTargetDeath(defender, ref status);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
	
	protected bool CheckHit()
	{
		int rand = m_generator.Next(100);
		
		return (rand < Accuracy);
	}
	
	protected void ApplyDamage(Character attacker, Character defender, ref ActionStatus status)
	{
		DamageResult result = DamageCalculations.CalculateDamage(this, attacker, defender, m_generator);
		
		if (result.effectiveness == Effectiveness.SuperEffective)
		{
			status.events.Add(new StatusUpdateEventArgs("It's super effective!"));
		}
		else if (result.effectiveness == Effectiveness.Weak)
		{
			status.events.Add(new StatusUpdateEventArgs("It's not very effective..."));
		}
		else if (result.effectiveness == Effectiveness.Immune)
		{
			status.events.Add(new StatusUpdateEventArgs("It doesn't affect the opposing " + defender.Name + "..."));
		}
		
		if (result.crit)
		{
			status.events.Add(new StatusUpdateEventArgs("A critical hit!"));
		}
		
		status.events.Add(new DamageEventArgs(defender.Owner, result.amount));
	}
	
	protected void HandleTargetDeath(Character target, ref ActionStatus status)
	{
		if (target.isDead())
		{
			status.events.Add(new StatusUpdateEventArgs(target.Name + " fainted!"));
			status.events.Add(new WithdrawEventArgs(target));
		}
	}
}
