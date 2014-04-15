using System.Collections.Generic;

public interface IAbility
{
	string Name { get; }
	AbilityStatus Execute(Character actor, List<Character> enemies);
}

