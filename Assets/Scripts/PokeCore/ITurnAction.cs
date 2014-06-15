using System.Collections.Generic;

namespace PokeCore {

public interface ITurnAction
{
	ICharacter Subject { get; }
	int Priority { get; } 
	ActionStatus Execute();
	bool Verify(out List<string> messages);
}

}