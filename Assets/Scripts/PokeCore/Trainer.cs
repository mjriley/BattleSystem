using System;
using System.Linq;
using System.Resources;
using System.Reflection;

namespace PokeCore {

public class Trainer
{
	TrainerDefinition.TrainerType m_class;
	string m_name;
	string m_title;
	string m_texturePath;
	
	public string TexturePath { get { return m_texturePath; } }
	
	public string Name { get { return m_name; } }
	public string Title { get { return m_title; } }
	
	string GetTitle(Pokemon.Gender gender)
	{
		ResourceManager rm = new ResourceManager("Assembly-CSharp", Assembly.GetExecutingAssembly());
		string localizationKey = m_class.ToString() + "_" + gender.ToString();
		
		return rm.GetString(localizationKey);
	}
	
	public Trainer(TrainerDefinition.TrainerType trainerType, Pokemon.Gender gender, int textureIndex=-1)
	{
		Random generator = new Random();
		
		if (trainerType == TrainerDefinition.TrainerType.RandomClass)
		{
			TrainerDefinition.TrainerType[] possibleClasses = (TrainerDefinition.TrainerType[])Enum.GetValues(typeof(TrainerDefinition.TrainerType));
			possibleClasses = possibleClasses.Where(x => x != TrainerDefinition.TrainerType.RandomClass).ToArray();
			
			int index = generator.Next(possibleClasses.Length);
			trainerType = possibleClasses[index];
		}
		
		m_class = trainerType;
		
		TrainerDefinition definition = TrainerDefinition.GetDefinition(m_class);
		
		if (gender == Pokemon.Gender.Random)
		{
			if (!definition.IsMale)
			{
				gender = Pokemon.Gender.Female;
			}
			else if (!definition.IsFemale)
			{
				gender = Pokemon.Gender.Male;
			}
			else
			{
				bool isMale = Convert.ToBoolean(generator.Next(2));
				gender = (isMale) ? Pokemon.Gender.Male : Pokemon.Gender.Female;
			}
		}
		
		string[] possibleTextures = (gender == Pokemon.Gender.Male) ? definition.MaleTextures : definition.FemaleTextures;
		
		if (textureIndex == -1)
		{
			textureIndex = generator.Next(possibleTextures.Length);
		}
		
		m_texturePath = "Textures/Trainers/VS" + possibleTextures[textureIndex];
		
		string[] possibleNames = (gender == Pokemon.Gender.Male) ? TrainerNames.Male : TrainerNames.Female;
		m_name = possibleNames[generator.Next(possibleNames.Length)];
		
		m_title = GetTitle(gender);
		
	}
	
}

}