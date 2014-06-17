using PokeCore;

namespace Moves {
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
			string format = L18N.Get("MOVE_SEEDED_DRAIN"); // <X>'s health was sapped by Leech Seed!
			string message = string.Format(format, actor.Name);
			status.events.Add(new StatusUpdateEventArgs(message));
			status.events.Add(new DamageEventArgs(enemy.Owner, -amount));
		}
	}
	
}}

