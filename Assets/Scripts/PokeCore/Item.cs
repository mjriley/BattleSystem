using System.Collections.Generic;

namespace PokeCore {

public class Item
{
	public Item()
	{
		HP = 0;
		PP = 0;
		GlobalPP = 0;
		Status = 0;
	}
	
	public int HP { get; set; }
	public int PercentHP { get; set; }
	public int PP { get; set; }
	public int GlobalPP { get; set; }
	public byte Status { get; set; }
	public string Desc { get; set; }
	public int Atk { get; set; }
	public int Def { get; set; }
	public int Spd { get; set; }
	public int SpAtk { get; set; }
	public int SpDef { get; set; }
	public int Acc { get; set; }
	
	public static Dictionary<string, Item> m_items;
	
	public static void Load()
	{
		m_items = new Dictionary<string, Item>();
		
		m_items["Ether"] = new Item() { PP = 10, Desc = "This medicine can restore 10 PP to a single selected move that has been learned by a Pokemon" };
		m_items["Berry Juice"] = new Item() { HP = 20, Desc = "A 100 percent pure juice made of Berries. When consumed, it restores 20 HP to an injured Pokemon" };
		m_items["Sweet Heart"] = new Item() { HP = 20, Desc = "A piece of cloyingly sweet chocolate. When consumed, it restores 20 HP to an injured Pokemon" };
		m_items["Max Ether"] = new Item() { PP = 1000, Desc = "This medicine can fully restore the PP of a single selected move that has been learned by a Pokemon" };
		m_items["Hyper Potion"] = new Item() { HP = 200, Desc = "A spray-type medicine for treating wounds. It can be used to restore 200 HP to an injured Pokemon" };
		m_items["Full Restore"] = new Item() { PercentHP = 100, Status = 0xFF, Desc = "A medicine that can be used to fully restore the HP of a single Pokemon and heal any status conditions it has." };
		m_items["Antidote"] = new Item() { Status = 0x1, Desc = "A spray-type medicine for poisoning. It can be used once to lift the effects of being poisoned from a Pokemon" };
		m_items["Revive"] = new Item() { PercentHP = 50, Status = 0x10, Desc = "A medicine that can revive fainted Pokemon. It also restores half of a fainted Pokemon's maximum HP" };
		m_items["Max Revive"] = new Item() { PercentHP = 100, Status = 0x10, Desc = "A medicine that can revive fainted Pokemon. It also fully restores a fainted Pokemon's maximum HP" };
		m_items["Lumiose Galette"] = new Item() { Status = 0xFF, Desc = "A popular treat in Lumiose City. It can be used once to heal all the status conditions of a Pokemon" };
		m_items["X Attack"] = new Item() { Atk = 10, Desc = "An item that boosts the Attack stat of a Pokemon during battle. It wears off once the Pokemon is withdrawn" };
	}
}

}