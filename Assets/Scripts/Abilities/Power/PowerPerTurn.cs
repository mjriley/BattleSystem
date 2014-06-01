using PokeCore;

namespace Abilities {
namespace Power {

using System;

public class PowerPerTurn : IPowerCalculation
{
	uint m_basePower;
	public PowerPerTurn(uint basePower)
	{
		m_basePower = basePower;
	}
	
	public string GetPowerString()
	{
		return "--";
	}
	
	public uint GetPower(Character attacker, Character defender, DamageAbility ability, IAbilityImpl parentAbility)
	{
		// Should only be used with multi-turn moves
		IRepeatAbility repeatAbility = (IRepeatAbility)parentAbility;
		
		return (uint)(m_basePower * Math.Pow(2, repeatAbility.CurrentTurn));
	}
}

}}
