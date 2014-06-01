using PokeCore;

namespace Abilities {
namespace Status {

	// Seeded processes once per turn, draining 1/8th of current HP and transferring that amount to the opponent
	public class SeededStatus
	{
		public static void ProcessStatus(Character actor, Character enemy, ref ActionStatus status)
		{
			// This still saps from a dead actor, but it will not revive a dead enemy
			if (enemy.isDead())
			{
				return;
			}
			
			int amount = actor.CurrentHP / 8;
			actor.TakeDamage(amount);
			enemy.TakeDamage(-amount);
			
			status.events.Add(new DamageEventArgs(actor.Owner, amount));
			status.events.Add(new StatusUpdateEventArgs(actor.Name + "'s health is sapped by Leech Seed!"));
			status.events.Add(new DamageEventArgs(enemy.Owner, -amount));
		}
	}
	
}}

