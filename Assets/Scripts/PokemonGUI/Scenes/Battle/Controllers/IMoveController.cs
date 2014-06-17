using PokeCore;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
namespace Controllers {

public interface IMoveController : IScreenController
{
	Character GetPokemon();
}

}}}}