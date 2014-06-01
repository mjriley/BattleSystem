using PokeCore;

namespace Abilities {

public interface IAbilityImpl
{
	void Reset();
	ActionStatus Execute(Character actor, Player targetPlayer, IAbilityImpl parentAbility);
}

}

