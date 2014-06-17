using PokeCore;

namespace Moves {

public interface IMoveImpl
{
	void Reset();
	ActionStatus Execute(Character actor, Player targetPlayer, IMoveImpl parentMove);
}

}

