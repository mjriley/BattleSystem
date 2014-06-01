using System;
using System.Collections.Generic;
using PokeCore;

namespace Abilities {
namespace Effects {

public class OnHitEffects
{
	public static DamageAbility.OnHitEffect CompositeEffect(params DamageAbility.OnHitEffect[] effects)
	{
		return delegate(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			foreach (DamageAbility.OnHitEffect effect in effects)
			{
				effect(ability, damage, attacker, defender, ref status);
			}
		};
	}
	

	public static DamageAbility.OnHitEffect BasicEffectWrapper(EffectAbility.BasicEffect effect)
	{
		return delegate(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			return effect(ability, attacker, defender.Owner, ref status);
		};
	}
	
	public static DamageAbility.OnHitEffect ParalyzeEffect(int percent, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}	
		
		return delegate(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			if (generator.Next(100) >= percent)
			{
				return;
			}
			
			defender.Paralyzed = true;
		};
		
	}
	
	public static DamageAbility.OnHitEffect FlinchEffect(int percent, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}	
		
		return delegate(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			if (generator.Next(100) >= percent)
			{
				return;
			}
			
			defender.Flinching = true;
		};
	}
	
	public static DamageAbility.OnHitEffect BurnEffect(int percent, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		return delegate(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			int rand = generator.Next(100);
			if (rand >= percent)
			{
				return;
			}
			
			if (!defender.Burned)
			{
				defender.Burned = true;
				status.events.Add(new StatusUpdateEventArgs(defender.Name + " was burned!"));
			}
			else
			{
				status.events.Add(new StatusUpdateEventArgs(defender.Name + " already has a burn."));
			
			}
		};
	}
	
	public static DamageAbility.OnHitEffect RecoilEffect(int percent)
	{
		return delegate(DamageAbility ability, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			int recoil_amount = (int)(damage * percent / 100.0);
			status.events.Add(new StatusUpdateEventArgs(attacker.Name + " was damaged by the recoil."));
			status.events.Add(new DamageEventArgs(attacker.Owner, recoil_amount));
		};
	}
	
}

}}
