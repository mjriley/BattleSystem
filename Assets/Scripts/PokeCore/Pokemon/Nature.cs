namespace PokeCore {
namespace Pokemon {

public class Nature
{
	public enum Type
	{
		Hardy,
		Lonely,
		Brave,
		Adamant,
		Naughty,
		Bold,
		Docile,
		Relaxed,
		Impish,
		Lax,
		Timid,
		Hasty,
		Serious,
		Jolly,
		Naive,
		Modest,
		Mild,
		Quiet,
		Bashful,
		Rash,
		Calm,
		Gentle,
		Sassy,
		Careful,
		Quirky
	};
	
	public Type Category { get; protected set; }
	Stat m_incStat;
	Stat m_decStat;
	
	public Nature(Type type, Stat incStat, Stat decStat)
	{
		Category = type;
		m_incStat = incStat;
		m_decStat = decStat;
	}
	
	public double GetMultiplier(Stat stat)
	{
		double multiplier = 1.0;
		
		if (stat == m_incStat)
		{
			multiplier += 0.1;
		}
		
		if (stat == m_decStat)
		{
			multiplier -= 0.1;
		}
		
		return multiplier;
	}
}

}}