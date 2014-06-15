using PokeCore;

namespace Items
{
	public interface IItem
	{
		ItemCategory Category { get; } 
		string Key { get; }
		string Name { get; }
		string Desc { get; }
		ActionStatus Use(ItemContext context);
	}
}

