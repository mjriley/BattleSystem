using NUnit.Framework;
using Moves;
using Moves.Power;
using PokeCore;

namespace Tests
{

class DummyRepeatMove : Moves.IRepeatMove
{
	public void Reset() { }
	public ActionStatus Execute(Character actor, Player targetPlayer, IMoveImpl parentMove) { return new ActionStatus(); }
	public int CurrentTurn { get; set; }
	public int MaxTurns { get { return 0; } }
	public DummyRepeatMove(int currentTurn) { CurrentTurn = currentTurn; }
}

[TestFixture]
public class PowerPerTurnTests
{
	RepeatMove m_repeatMove;
	PowerPerTurn m_powerObject;
	
	[SetUp]
	public void Init()
	{
		m_powerObject = new PowerPerTurn(30);
	}
	
	[Test]
	public void BasePowerOnFirstTurn()
	{
		DummyRepeatMove parent = new DummyRepeatMove(0);
		uint power = m_powerObject.GetPower(null, null, null, parent);
		Assert.AreEqual(30, power);
	}
	
	[Test]
	public void PowerOnTurnFour()
	{
		DummyRepeatMove parent = new DummyRepeatMove(3);
		uint power = m_powerObject.GetPower(null, null, null, parent);
		Assert.AreEqual(240, power);
	}
}

}

