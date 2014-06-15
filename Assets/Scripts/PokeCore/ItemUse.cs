using Items;
using System.Collections.Generic;

namespace PokeCore {

public class ItemUse : ITurnAction
{
	IItem m_item;
	ItemContext m_context;
	Inventory m_inventory;
	
	public ItemUse(IItem item, ItemContext context, Inventory inventory)
	{
		m_item = item;
		m_context = context;
		m_inventory = inventory;
	}
	
	public virtual ICharacter Subject { get { return m_context.Player.ActivePokemon; } }
	
	public virtual ActionStatus Execute()
	{
		return m_inventory.UseItem(m_item, m_context);
	}
	
	public int Priority { get { return 6; } }
	
	public bool Verify(out List<string> messages)
	{
		messages = new List<string>();
		return true;
	}
}

}