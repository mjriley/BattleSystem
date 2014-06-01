using System;

namespace PokeCore {

// Wrapper around System.Random
public class CRandom : IRandom
{
	Random m_generator;
	
	public CRandom(Random random=null)
	{
		if (random == null)
		{
			random = new Random();
		}
		
		m_generator = random;
	}
	
	public int Next()
	{
		return m_generator.Next();
	}
	
	public int Next(int maxValue)
	{
		return m_generator.Next(maxValue);
	}
	
	public int Next(int minValue, int maxValue)
	{
		return m_generator.Next(minValue, maxValue);
	}
}

}