using System.Collections.Generic;

namespace PokeCore {
namespace Pokemon {

public class NatureFactory
{
	static bool isInit = false;
	static Dictionary<Nature.Type, Nature> m_natures = new Dictionary<Nature.Type, Nature>();
	
	static void AddNature(Nature.Type type, Stat incStat, Stat decStat)
	{
		m_natures[type] = new Nature(type, incStat, decStat);
	}
	
	public static Nature GetNature(Nature.Type type)
	{
		Init();
		return m_natures[type];
	}
	
	static void Init()
	{
		if (isInit)
		{
			return;
		}
		
		AddNature(Nature.Type.Hardy, Stat.Attack, Stat.Attack);
		AddNature(Nature.Type.Lonely, Stat.Attack, Stat.Defense);
		AddNature(Nature.Type.Adamant, Stat.Attack, Stat.SpecialAttack);
		AddNature(Nature.Type.Naughty, Stat.Attack, Stat.SpecialDefense);
		AddNature(Nature.Type.Brave, Stat.Attack, Stat.Speed);
		
		AddNature(Nature.Type.Bold, Stat.Defense, Stat.Attack);
		AddNature(Nature.Type.Docile, Stat.Defense, Stat.Defense);
		AddNature(Nature.Type.Impish, Stat.Defense, Stat.SpecialAttack);
		AddNature(Nature.Type.Lax, Stat.Defense, Stat.SpecialDefense);
		AddNature(Nature.Type.Relaxed, Stat.Defense, Stat.Speed);
		
		AddNature(Nature.Type.Timid, Stat.Speed, Stat.Attack);
		AddNature(Nature.Type.Hasty, Stat.Speed, Stat.Defense);
		AddNature(Nature.Type.Jolly, Stat.Speed, Stat.SpecialAttack);
		AddNature(Nature.Type.Naive, Stat.Speed, Stat.SpecialDefense);
		AddNature(Nature.Type.Serious, Stat.Speed, Stat.Speed);
		
		AddNature(Nature.Type.Modest, Stat.SpecialAttack, Stat.Attack);
		AddNature(Nature.Type.Mild, Stat.SpecialAttack, Stat.Defense);
		AddNature(Nature.Type.Bashful, Stat.SpecialAttack, Stat.SpecialAttack);
		AddNature(Nature.Type.Rash, Stat.SpecialAttack, Stat.SpecialDefense);
		AddNature(Nature.Type.Quiet, Stat.SpecialAttack, Stat.Speed);
		
		AddNature(Nature.Type.Calm, Stat.SpecialDefense, Stat.Attack);
		AddNature(Nature.Type.Gentle, Stat.SpecialDefense, Stat.Defense);
		AddNature(Nature.Type.Careful, Stat.SpecialDefense, Stat.SpecialAttack);
		AddNature(Nature.Type.Quirky, Stat.SpecialDefense, Stat.SpecialDefense);
		AddNature(Nature.Type.Sassy, Stat.SpecialDefense, Stat.Speed);
		
		isInit = true;
	}
}

}}