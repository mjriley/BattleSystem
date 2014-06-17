namespace PokeCore {
namespace Moves {

public interface IRepeatMove : IMoveImpl
{
	int CurrentTurn { get; }
	int MaxTurns { get; }
}

}}