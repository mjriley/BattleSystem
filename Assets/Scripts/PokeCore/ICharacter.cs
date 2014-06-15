namespace PokeCore {

// Pokemon Character interface to allow...sane testing
public interface ICharacter
{
	Player Owner { get; }
	int Spd { get; }
}

}