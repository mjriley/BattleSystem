public interface ITurnAction
{
	Character Subject { get; }
	ActionStatus Execute();
}
