using System;

public class OnHitEffects
{
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

