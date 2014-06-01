namespace PokeCore {

public interface ITurnAction
{
	ICharacter Subject { get; }
	int Priority { get; } 
	ActionStatus Execute();
}

}