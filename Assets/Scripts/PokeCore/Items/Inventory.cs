using System.Linq;
using System.Collections.Generic;
using PokeCore;
using System;

namespace PokeCore {
namespace Items {

public class Inventory
{
	Dictionary<string, KeyValuePair<IItem, int>> m_items;
	
	public Inventory()
	{
		m_items = new Dictionary<string, KeyValuePair<IItem, int>>();
	}
	
	public List<KeyValuePair<IItem, int>> GetCategory(ItemCategory category)
	{
		var filtered = m_items.Where(kvp => kvp.Value.Key.Category == category);
		List<KeyValuePair<IItem, int>> list = filtered.Select(kvp => kvp.Value).ToList();
		
		return list;
	}
	
	public void AddItem(IItem item, int count=1)
	{
		int existingCount = 0;
		if (m_items.ContainsKey(item.Name))
		{
			KeyValuePair<IItem, int> existing = m_items[item.Name];
			existingCount = existing.Value;
		}
		
		m_items[item.Name] = new KeyValuePair<IItem, int>(item, existingCount + count);
	}
	
	public ActionStatus UseItem(IItem item, ItemContext context)
	{
		ActionStatus status = item.Use(context);
		DecreaseItemCount(item);
		
		return status;
	}
	
	void DecreaseItemCount(IItem item, int amount = 1)
	{
		if (!m_items.ContainsKey(item.Name))
		{
			throw new Exception("Item does not exist");
		}
		
		KeyValuePair<IItem, int> existing = m_items[item.Name];
		
		if (existing.Value - 1 == 0)
		{
			m_items.Remove(item.Name);
		}
		else
		{
			m_items[item.Name] = new KeyValuePair<IItem, int>(item, existing.Value - 1);
		}
	}
	
	public ItemUse GetItemUse(IItem item, ItemContext context)
	{
		if (!m_items.ContainsKey(item.Name))
		{
			throw new Exception("Item does not exist");
		}
		
		ItemUse use = new ItemUse(item, context, this);
		
		return use;
	}
}

}}