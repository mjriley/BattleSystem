namespace PokemonGUI {

public interface IScreenController
{
	void ProcessInput(int selection);
	
	void Unload();
	
	void Enable();
	void Disable();
}

}