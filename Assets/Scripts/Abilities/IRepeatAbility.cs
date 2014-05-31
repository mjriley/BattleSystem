namespace Abilities {

public interface IRepeatAbility : IAbilityImpl
{
	int CurrentTurn { get; }
	int MaxTurns { get; }
}

}

