using System;

public class DamageAbility : AbstractAbility
{
	public delegate void OnHitEffect(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status);

	public uint Power { get; protected set; }
	public int Accuracy { get; protected set; }
	
	protected Random m_generator;
	private OnHitEffect onHitHandler;
	
	bool m_highCritRate;
	public int CritStage { get { return (m_highCritRate ? 1 : 0); } }
	
	public DamageAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, uint power, int accuracy, int priority=0, bool highCritRate=false, OnHitEffect onHitHandler=null, string description="", Random generator=null)
	: base(name, abilityType, battleType, maxPP, priority, description)
	{
		Power = power;
		Accuracy = accuracy;
		
		if (generator == null)
		{
			generator = new Random();
		}
		
		m_generator = generator;
		
		m_highCritRate = highCritRate;
		this.onHitHandler = onHitHandler;
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
		
		DamageResult result = ApplyDamage(attacker, defender, ref status);
		
		HandleTargetDeath(defender, ref status);
		HandleOnHit(attacker, defender, result, ref status);
		
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
	
	protected bool CheckHit()
	{
		int rand = m_generator.Next(100);
		
		return (rand < Accuracy);
	}
	
	protected DamageResult ApplyDamage(Character attacker, Character defender, ref ActionStatus status)
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
		defender.TakeDamage(result.amount);
		
		return result;
	}
	
	protected void HandleOnHit(Character attacker, Character defender, DamageResult result, ref ActionStatus status)
	{
		if (onHitHandler != null)
		{
			onHitHandler(this, result.amount, attacker, defender, ref status);
		}
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
