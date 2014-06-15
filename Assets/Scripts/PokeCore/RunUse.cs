using System.Collections.Generic;

namespace PokeCore {

// Placeholder class for running away from fights.
// Current unconditionally prohibits running away from fights
public class RunUse : ITurnAction
{
	Character m_pokemon;
	
	public RunUse(Character pokemon)
	{
		m_pokemon = pokemon;
	}
	
	public string Name { get { return "RunUse"; } }
	
	public ICharacter Subject { get { return m_pokemon; } }
	
	public int Priority { get { return 6; } }
	
	public ActionStatus Execute()
	{
		ActionStatus status = new ActionStatus();
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
	
	const string FLEE_PROHIBITED_MSG = "FLEE_PROHIBITED";
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		
		messages.Add(L18N.Get(FLEE_PROHIBITED_MSG));
		return false;
	}
}

}

