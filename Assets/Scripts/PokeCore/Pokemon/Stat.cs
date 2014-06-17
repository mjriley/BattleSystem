namespace PokeCore {
namespace Pokemon {

public enum Stat
{
	HP,
	Attack,
	Defense,
	SpecialAttack,
	SpecialDefense,
	Speed,
	Accuracy,
	Evasion
}

public static class StatHelper
{
	public static string LocalName(this Stat stat)
	{
		string key = stat.ToString().ToUpper().Replace(' ', '_');
		return L18N.Get("STAT_" + key);	
	}
}

}}