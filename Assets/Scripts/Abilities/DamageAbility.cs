using System;
using PokeCore;
using Abilities.Power;

namespace Abilities {

public class DamageAbility : AbstractAbility
{
	public delegate void OnHitEffect(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status);
	public delegate DamageResult DamageFormula(Character attacker, Character defender, DamageAbility ability, IAbilityImpl parentAbility);
	public delegate uint PowerFunction(Character attacker, Character defender, DamageAbility ability, IAbilityImpl parentAbility);

	private IPowerCalculation m_powerCalculation;
	public PowerFunction Power { get { return m_powerCalculation.GetPower; } }
	public int Accuracy { get; protected set; }
	
	protected Random m_generator;
	private DamageFormula damageFormula;
	private OnHitEffect onHitHandler;
	
	bool m_highCritRate;
	public int CritStage { get { return (m_highCritRate ? 1 : 0); } }
	
	public DamageAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, uint power, int accuracy, int priority=0, 
		bool highCritRate=false, DamageFormula damageFormula=null, OnHitEffect onHitHandler=null, string description="", Random generator=null)
	: this(name, abilityType, battleType, maxPP, new FixedPower(power), accuracy, priority, highCritRate, damageFormula, onHitHandler, description, generator)
	{
	}
	
	public DamageAbility(string name, AbilityType abilityType, BattleType battleType, int maxPP, IPowerCalculation power, int accuracy, int priority=0, 
		bool highCritRate=false, DamageFormula damageFormula=null, OnHitEffect onHitHandler=null, string description="", Random generator=null)
	: base(name, abilityType, battleType, maxPP, priority, description)
	{
		m_powerCalculation = power;
		Accuracy = accuracy;
		
		if (generator == null)
		{
			generator = new Random();
		}
		
		m_generator = generator;
		
		m_highCritRate = highCritRate;
		
		if (damageFormula == null)
		{
			damageFormula = DamageCalculations.CalculateDamage;
		}
		this.damageFormula = damageFormula;
		
		this.onHitHandler = onHitHandler;
	}
	
	protected override ActionStatus ExecuteImpl(Character attacker, Player targetPlayer, ref ActionStatus status, IAbilityImpl parentAbility=null)
	{
		Character defender = targetPlayer.ActivePokemon;
		
		if (!CheckHit())
		{
			string format = L18N.Get("MOVE_MISSED");
			string message = string.Format(format, attacker.Name);
			status.events.Add(new StatusUpdateEventArgs(message));
			status.turnComplete = true;
			status.isComplete = true;
			
			return status;
		}
		
		DamageResult result = ApplyDamage(attacker, defender, ref status, parentAbility);
		
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
	
	protected DamageResult ApplyDamage(Character attacker, Character defender, ref ActionStatus status, IAbilityImpl parentAbility)
	{
		DamageResult result = damageFormula(attacker, defender, this, parentAbility);
		
		if (result.effectiveness == Effectiveness.SuperEffective)
		{
			string message = L18N.Get("EFFECTIVENESS_GOOD"); // It's super effective!
			status.events.Add(new StatusUpdateEventArgs(message));
		}
		else if (result.effectiveness == Effectiveness.Weak)
		{
			string message = L18N.Get("EFFECTIVENESS_BAD"); // It's not very effective...
			status.events.Add(new StatusUpdateEventArgs(message));
		}
		else if (result.effectiveness == Effectiveness.Immune)
		{
			string format = L18N.Get("EFFECTIVENESS_IMMUNE"); // It's doesn't affect the opposing <X>...
			string message = string.Format(format, defender.Name);
			status.events.Add(new StatusUpdateEventArgs(message));
		}
		
		if (result.crit)
		{
			string message = L18N.Get("MOVE_CRIT"); // A critical hit!
			status.events.Add(new StatusUpdateEventArgs(message));
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
			string format = L18N.Get("MSG_DEATH"); // <X> fainted!
			string message = string.Format(format, target.Name);
			status.events.Add(new StatusUpdateEventArgs(message));
			status.events.Add(new WithdrawEventArgs(target));
		}
	}
}

}