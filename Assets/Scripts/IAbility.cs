public interface IAbility
{
	string Name { get; }
	ActionStatus Execute(Character actor, Player targetPlayer);
}

