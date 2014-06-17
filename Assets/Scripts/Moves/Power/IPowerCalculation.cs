using PokeCore;

namespace Moves {
namespace Power {

	public interface IPowerCalculation
	{
		string GetPowerString();
		uint GetPower(Character attacker, Character defender, DamageMove move, IMoveImpl parentMove);
	}
	
}}

