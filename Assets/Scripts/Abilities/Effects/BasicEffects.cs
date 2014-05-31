using System;

namespace Abilities {
namespace Effects {

public class BasicEffects
{
	public static void SeedTarget(AbstractAbility ability, Character actor, Player targetPlayer, ref ActionStatus status)
	{
		Character target = targetPlayer.ActivePokemon;
		if (!target.Seeded)
		{
			target.Seeded = true;
			status.events.Add(new StatusUpdateEventArgs(targetPlayer.Name + " was seeded!"));
		}
		else
		{
			status.events.Add(new StatusUpdateEventArgs("But it failed!"));
		}
	}
	
	public static EffectAbility.BasicEffect StatEffect(Target target, Stat stat, int stage_change, int percent=100, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		return delegate(AbstractAbility ability, Character actor, Player enemyPlayer, ref ActionStatus status)
		{
			if (generator.Next(0, 100) >= percent)
			{
				return;
			}
			
			Character subject = (target == Target.Self) ? actor : enemyPlayer.ActivePokemon;
			int actual_change = subject.ModifyStage(stat, stage_change);
			
			string suffix;
			
			if (stage_change > 0)
			{
				switch (actual_change)
				{
					case 0: 
						suffix = "won't go any higher!";
						break;
					case 1:
						suffix = "rose!";
						break;
					case 2:
						suffix = "rose sharply!";
						break;
					default:
						suffix = "rose drastically!";
						break;
				}
			}
			else
			{
				switch (actual_change)
				{
					case 0:
						suffix = "won't go any lower!";
						break;
					case -1:
						suffix = "fell!";
						break;
					case -2:
						suffix = "harshly fell!";
						break;
					default:
						suffix = "severely fell!";
						break;
				}
			}
			
			status.events.Add(new StatusUpdateEventArgs(subject.Name + "'s " + stat + " " + suffix));
		};
	}
	
}

}}