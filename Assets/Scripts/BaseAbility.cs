public class BaseAbility
{
	IAbilityEffect m_effect;
	
	public BaseAbility(string name, uint max_pp, AbilityType abilityType, BattleType battleType, IAbilityEffect effect)
	{
		m_effect = effect;
	}
	
	public ActionStatus GenerateTurn(Player actor, Player target)
	{
		return m_effect.Execute(actor, target);
	}
}
