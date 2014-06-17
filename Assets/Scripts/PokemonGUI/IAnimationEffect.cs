namespace PokemonGUI {

public interface IAnimationEffect
{
	bool Done { get; }
	
	void Start();
	void Update();
}

}