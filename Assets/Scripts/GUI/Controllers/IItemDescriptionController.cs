using System.Collections.Generic;
using PokeCore;
using Items;

public interface IItemDescriptionController : IScreenController
{
	KeyValuePair<IItem, int> GetItem();
}

