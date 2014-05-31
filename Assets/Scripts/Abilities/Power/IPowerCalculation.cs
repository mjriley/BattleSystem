namespace Abilities {
namespace Power {

	public interface IPowerCalculation
	{
		string GetPowerString();
		uint GetPower(Character attacker, Character defender, DamageAbility ability, IAbilityImpl parentAbility);
	}
	
}}

