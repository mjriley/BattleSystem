using System.Collections.Generic;

namespace PokeCore {

public class NoTurnAction : ITurnAction
{
	public NoTurnAction(ICharacter subject)
	{
		Subject = subject;
	}
	
	public string Name { get { return "NoTurnAction"; } }
	
	public ICharacter Subject { get; protected set; }
	
	public int Priority { get { return 0; } } 
	
	public ActionStatus Execute()
	{
		ActionStatus status = new ActionStatus();
		status.turnComplete = true;
		status.isComplete = true;
		
		return status;
	}
	
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		return true;
	}
}

}

