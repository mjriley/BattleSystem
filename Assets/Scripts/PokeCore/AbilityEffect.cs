namespace PokeCore {

public class AbilityEffect
{
	public enum EffectType
	{
		Burn,
		Freeze,
		Paralysis,
		Poison,
		Sleep,
		None
	}
	
	public EffectType Type { get; set; }
	public float Rate { get; set; }
	
	public AbilityEffect(EffectType type, float rate)
	{
		Type = type;
		Rate = rate;
	}
	
	public void Apply(Character target)
	{
		switch (Type)
		{
			case EffectType.Burn:
			{
				target.Burned = true;
				break;
			}
			case EffectType.Freeze:
			{
				target.Frozen = true;
				break;
			}
			case EffectType.Paralysis:
			{
				target.Paralyzed = true;
				break;
			}
			case EffectType.Poison:
			{
				target.Poisoned = true;
				break;
			}
			case EffectType.Sleep:
			{
				target.FallAsleep();
				break;
			}
			default:
			{
				// do nothing
				break;
			}
		}
	}
	
	public string GetActionMessage()
	{
		return "was afflicted by " + System.Enum.GetName(typeof(EffectType), Type);
		
	}
}

}