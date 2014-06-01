using PokeCore;

namespace Abilities {
namespace Power {

	// The basic, non-changing power level for abilities
	public class FixedPower : IPowerCalculation
	{
		uint m_power;
		
		public FixedPower(uint power)
		{
			m_power = power;
		}
		
		public string GetPowerString()
		{
			return m_power.ToString();
		}
		
		public uint GetPower(Character attacker, Character defender, DamageAbility ability, IAbilityImpl parentAbility)
		{
			return m_power;
		}
	}
	
}}

