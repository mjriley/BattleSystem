using PokeCore;

namespace Abilities {
namespace Power {

	// Ability power is determined based on the attacker's percent of remaining health, gated by a table
	public class GatedPercentHP : IPowerCalculation
	{
		public GatedPercentHP()
		{
		}
		
		public string GetPowerString()
		{
			return "--";
		}
		
		public uint GetPower(Character attacker, Character defender, DamageAbility ability, IAbilityImpl parentAbility)
		{
			double percentRemaining = (double)attacker.CurrentHP / attacker.MaxHP;
			
			uint power;
			
			if (percentRemaining > 0.7)
			{
				power = 20;
			}
			else if (percentRemaining > 0.35 && percentRemaining <= 0.7)
			{
				power = 40;
			}
			else if (percentRemaining > 0.2 && percentRemaining <= 0.35)
			{
				power = 80;
			}
			else if (percentRemaining > 0.10 && percentRemaining <= 0.2)
			{
				power = 100;
			}
			else if (percentRemaining >= 0.05 && percentRemaining <= 0.1)
			{
				power = 150;
			}
			else
			{
				power = 200;
			}
			
			return power;
		}
	}
	
}}
