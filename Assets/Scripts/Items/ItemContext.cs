using PokeCore;

namespace Items
{

public class ItemContext
{
	public ItemContext(Player player, Character target)
	{
		this.Player = player;
		Target = target;
		MoveIndex = 0;
	}
	
	public Player Player { get; set; }
	public Character Target { get; set; }
	public int MoveIndex { get; set; }
}

}

