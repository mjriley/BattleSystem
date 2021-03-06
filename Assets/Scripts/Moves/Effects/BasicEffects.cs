using System;
using PokeCore.Pokemon;
using PokeCore;

namespace PokeCore {
namespace Moves {
namespace Effects {

public class BasicEffects
{
	public static void SeedTarget(AbstractMove move, Character actor, Player targetPlayer, ref ActionStatus status)
	{
		Character target = targetPlayer.ActivePokemon;
		if (!target.Seeded)
		{
			target.Seeded = true;
			status.events.Add(new StatusUpdateEventArgs(targetPlayer.Name + " was seeded!"));
		}
		else
		{
			status.events.Add(new StatusUpdateEventArgs(L18N.Get("MSG_FAIL"))); // But it failed!
		}
	}
	
	public static EffectMove.BasicEffect StatEffect(Target target, Stat stat, int stage_change, int percent=100, Random generator=null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		
		return delegate(AbstractMove move, Character actor, Player enemyPlayer, ref ActionStatus status)
		{
			if (generator.Next(0, 100) >= percent)
			{
				return;
			}
			
			Character subject = (target == Target.Self) ? actor : enemyPlayer.ActivePokemon;
			int actual_change = subject.ModifyStage(stat, stage_change);
			
			string message = Character.GetStageModificationMessage(subject.Name, stat, actual_change, (stage_change > 0));
			
			status.events.Add(new StatusUpdateEventArgs(message));
		};
	}
	
}

}}}