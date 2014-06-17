using PokeCore;

namespace Moves {
namespace Power {

	// The basic, non-changing power level for moves
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
		
		public uint GetPower(Character attacker, Character defender, DamageMove move, IMoveImpl parentMove)
		{
			return m_power;
		}
	}
	
}}

