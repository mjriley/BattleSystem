using System;

public class BasicEffects
{
	public static EffectAbility.BasicEffect TargetStatModificationEffect(Stat stat, int stage_change, int percent=100, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		return delegate(AbstractAbility ability, Character actor, Player targetPlayer, ref ActionStatus status)
		{
			if (generator.Next(0, 100) >= percent)
			{
				return;
			}
			
			Character target = targetPlayer.ActivePokemon;
			int actual_change = target.ModifyStage(Stat.Attack, stage_change);
			
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
			
			status.events.Add(new StatusUpdateEventArgs(target.Name + "'s " + stat + " " + suffix));
		};
	}
}

