using PokeCore;

namespace PokeCore {
namespace Items {

public abstract class BaseItem : IItem
{
	public BaseItem(string name, ItemCategory category)
	{
		Key = name;
		Name = L18N.Get("ITEM_" + name);
		Desc = L18N.Get("ITEM_DESC_" + name);
		Category = category;
	}
	
	public string Key { get; protected set; }
	public string Name { get; protected set; }
	public string Desc { get; protected set; }
	public ItemCategory Category { get; protected set; }
	
	public abstract ActionStatus Use(ItemContext context);
}

}}