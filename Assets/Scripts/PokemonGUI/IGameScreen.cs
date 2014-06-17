namespace PokemonGUI {

public interface IGameScreen
{
	void Invalidate();
	bool enabled { get; set; }
}

}
