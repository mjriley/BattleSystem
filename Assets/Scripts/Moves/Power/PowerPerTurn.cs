using PokeCore;

namespace Moves {
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
	
	public uint GetPower(Character attacker, Character defender, DamageMove move, IMoveImpl parentMove)
	{
		// Should only be used with multi-turn moves
		IRepeatMove repeatMove = (IRepeatMove)parentMove;
		
		return (uint)(m_basePower * Math.Pow(2, repeatMove.CurrentTurn));
	}
}

}}
