using System.Collections.Generic;
using PokeCore;
using PokeCore.Items;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {
namespace Controllers {

public interface IItemDescriptionController : IScreenController
{
	KeyValuePair<IItem, int> GetItem();
}

}}}}