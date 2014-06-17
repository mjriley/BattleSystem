using System;
using System.Collections.Generic;
using PokeCore;

namespace Moves {
namespace Effects {

public class OnHitEffects
{
	public static DamageMove.OnHitEffect CompositeEffect(params DamageMove.OnHitEffect[] effects)
	{
		return delegate(DamageMove move, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			foreach (DamageMove.OnHitEffect effect in effects)
			{
				effect(move, damage, attacker, defender, ref status);
			}
		};
	}
	

	public static DamageMove.OnHitEffect BasicEffectWrapper(EffectMove.BasicEffect effect)
	{
		return delegate(DamageMove move, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			return effect(move, attacker, defender.Owner, ref status);
		};
	}
	
	public static DamageMove.OnHitEffect ParalyzeEffect(int percent, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}	
		
		return delegate(DamageMove move, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			if (generator.Next(100) >= percent)
			{
				return;
			}
			
			defender.Paralyzed = true;
		};
		
	}
	
	public static DamageMove.OnHitEffect FlinchEffect(int percent, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}	
		
		return delegate(DamageMove move, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			if (generator.Next(100) >= percent)
			{
				return;
			}
			
			defender.Flinching = true;
		};
	}
	
	public static DamageMove.OnHitEffect BurnEffect(int percent, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		return delegate(DamageMove move, int damage, Character attacker, Character defender, ref ActionStatus status)
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
	
	public static DamageMove.OnHitEffect RecoilEffect(int percent)
	{
		return delegate(DamageMove move, int damage, Character attacker, Character defender, ref ActionStatus status)
		{
			int recoil_amount = (int)(damage * percent / 100.0);
			status.events.Add(new StatusUpdateEventArgs(attacker.Name + " was damaged by the recoil."));
			status.events.Add(new DamageEventArgs(attacker.Owner, recoil_amount));
		};
	}
	
}

}}
