using System.Collections.Generic;
using PokeCore.Pokemon;

namespace PokeCore {
namespace Items {

public static class ItemFactory
{
	static Dictionary<string, IItem> m_items;
	
	static ItemFactory()
	{
		m_items = new Dictionary<string, IItem>();
		
		AddItem(new RestoreItem("Potion", 20));
		AddItem(new RestoreItem("Super Potion", 50));
		AddItem(new RestoreItem("Hyper Potion", 200));
		AddItem(new EtherItem("Ether", 10));
		AddItem(new EtherItem("Max Ether", 255));
		AddItem(new EtherItem("Elixir", 10, allMoves: true));
		AddItem(new EtherItem("Max Elixir", 255, allMoves: true));
		
		AddItem(new StatusItem("Antidote", StatusType.Poison));
		AddItem(new StatusItem("Paralyze Heal", StatusType.Paralysis));
		AddItem(new StatusItem("Awakening", StatusType.Sleep));
		AddItem(new StatusItem("Burn Heal", StatusType.Burn));
		AddItem(new StatusItem("Ice Heal", StatusType.Freeze));
		
		AddItem(new BallItem("Poke Ball", 1.0));
		AddItem(new BallItem("Great Ball", 1.5));
		AddItem(new BallItem("Ultra Ball", 2.0));
		AddItem(new BallItem("Master Ball", 255.0));
		
		AddItem(new StatItem("X Attack", Stat.Attack, 1));
		AddItem(new StatItem("X Defense", Stat.Defense, 1));
		AddItem(new StatItem("X Sp. Atk", Stat.SpecialAttack, 1));
		AddItem(new StatItem("X Sp. Def", Stat.SpecialDefense, 1));
		AddItem(new StatItem("X Speed", Stat.Speed, 1));
		AddItem(new StatItem("X Accuracy", Stat.Accuracy, 1));
	}
	
	public static IItem GetItem(string name)
	{
		return m_items[name];
	}
	
	static void AddItem(IItem item)
	{
		m_items[item.Key] = item;
	}
}
	
}}

