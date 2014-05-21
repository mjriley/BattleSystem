using System;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;

public class TrainerDefinition
{
	public enum TrainerType
	{
		Nobility1, // Viscount / Viscountess
		Nobility2, // Marquis / Marchioness
		Nobility3, // Earl / Countess
		Nobility4, // Baron / Baroness
		Nobility5, // Duke / Duchess
		Nobility6, // Female only (Grand Duchess)
		Chef, // Male only
		Driver, // Male only
		DreamGirl, // Female only
		Waiter, // Garcon / Waitress
		Gardener, // Male only
		Owner, // Male only
		Punk, // Punk Girl / Punk Guy
		Ranger, // Male & Female
		RisingStar, // Male & Female
		Skater, // Male & Female
		Student, // Schoolboy / Schoolgirl
		SkyTrainer, // Male & Female
		Tourist, // Male & Female
		AceTrainer, // Male & Female
		Painter, // Male & Female
		Backpacker,
		Hiker, // Male only
		Karate, // Male & Female
		Beauty, // Female only
		Fisherman, // Male only
		HexManiac, // Female only
		PokeFan, // Male & Female
		Breeder, // Male & Female
		Preschooler, // Male & Female
		Psychic, // Male only
		Scientist, // Male & Female
		Swimmer, // Male & Female
		Veteran, // Male & Female
		Worker, // Male only
		Child, // Male & Female
		RandomClass
	};
	
	
	static bool m_isInit = false;
	static Dictionary<TrainerType, TrainerDefinition> m_trainers = new Dictionary<TrainerType, TrainerDefinition>();
	//static Dictionary<Class, string> m_maleClassNames = new Dictionary<Class, string>();
	//static Dictionary<Class, string> m_femaleClassNames = new Dictionary<Class, string>();
	
	static void AddTrainer(TrainerType trainerClass, string[] male=null, string[] female=null)
	{
		m_trainers[trainerClass] = new TrainerDefinition(trainerClass, male, female);
	}
	
	static void Init()
	{
		if (m_isInit)
		{
			return;
		}
		
		//UnityEngine.Debug.Log("Current directory is: " + Mono.Unix.UnixDirectoryInfo.GetCurrentDirectory());
		//Mono.Unix.Catalog.Init("i8n1", "locale");
		
		AddTrainer(TrainerType.Nobility1, male: new string[] {"Rich_Boy", "Monsieur", "Butler"}, female: new string[] {"Lady", "Madame", "Maid"});
		AddTrainer(TrainerType.Nobility2, male: new string[] {"Rich_Boy", "Monsieur", "Butler"}, female: new string[] {"Lady", "Madame", "Maid"});
		AddTrainer(TrainerType.Nobility3, male: new string[] {"Rich_Boy", "Monsieur", "Butler"}, female: new string[] {"Lady", "Madame", "Maid"});
		AddTrainer(TrainerType.Nobility4, male: new string[] {"Rich_Boy", "Monsieur", "Butler", "Grant", "Ramos", "Clemont", "Wulfric"}, 
			female: new string[] {"Lady", "Madame", "Maid", "Furisode_Girl_1", "Furisode_Girl_2", "Furisode_Girl_3", "Furisode_Girl_4", "Viola", "Korrina", "Valerie", "Olympia"});
		AddTrainer(TrainerType.Nobility5, male: new string[] {"Rich_Boy", "Monsieur", "Butler", "Wikstrom", "Siebold"}, 
			female: new string[] {"Lady", "Madame", "Maid", "Furisode_Girl_1", "Furisode_Girl_2", "Furisode_Girl_3", "Furisode_Girl_4", "Malva", "Drasna"});
		AddTrainer(TrainerType.Nobility6, female: new string[] {"Diantha"});
		AddTrainer(TrainerType.Chef, male: new string[] {"Chef"});
		AddTrainer(TrainerType.Driver, male: new string[] {"Veteran_M", "Punk_Guy", "Monsieur"});
		AddTrainer(TrainerType.DreamGirl, female: new string[] {"Fairy_Tale_Girl"});
		AddTrainer(TrainerType.Waiter, male: new string[] {"Garcon"}, female: new string[] {"Waitress"});
		AddTrainer(TrainerType.Gardener, male: new string[] {"Gardener"});
		AddTrainer(TrainerType.Owner, male: new string[] {"Owner"});
		AddTrainer(TrainerType.Punk, male: new string[] {"Punk_Guy"}, female: new string[] {"Punk_Girl"});
		AddTrainer(TrainerType.Ranger, male: new string[] {"Ranger_M"}, female: new string[] {"Ranger_F"});
		AddTrainer(TrainerType.RisingStar, male: new string[] {"Rising_Star_M"}, female: new string[] {"Rising_Star_F"});
		AddTrainer(TrainerType.Skater, male: new string[] {"Roller_Skater_M"}, female: new string[] {"Roller_Skater_F"});
		AddTrainer(TrainerType.Student, male: new string[] {"Schoolboy"}, female: new string[] {"Schoolgirl"});
		AddTrainer(TrainerType.SkyTrainer, male: new string[] {"Sky_Trainer_M"}, female: new string[] {"Sky_Trainer_F"});
		AddTrainer(TrainerType.Tourist, male: new string[] {"Tourist_M"}, female: new string[] {"Tourist_F_A", "Tourist_F_B"});
		AddTrainer(TrainerType.AceTrainer, male: new string[] {"Ace_Trainer_M"}, female: new string[] {"Ace_Trainer_F"});
		AddTrainer(TrainerType.Painter, male: new string[] {"Artist_M"}, female: new string[] {"Artist_F"});
		AddTrainer(TrainerType.Backpacker, male: new string[] {"Backpacker"});
		AddTrainer(TrainerType.Hiker, male: new string[] {"Hiker"});
		AddTrainer(TrainerType.Karate, male: new string[] {"Black_Belt"}, female: new string[] {"Battle_Girl"});
		AddTrainer(TrainerType.Beauty, female: new string[] {"Beauty"});
		AddTrainer(TrainerType.Fisherman, male: new string[] {"Fisherman"});
		AddTrainer(TrainerType.HexManiac, female: new string[] {"Hex_Maniac"});
		AddTrainer(TrainerType.PokeFan, male: new string[] {"Poke_Fan_M"}, female: new string[] {"Poke_Fan_F"});
		AddTrainer(TrainerType.Breeder, male: new string[] {"Pokemon_Breeder_M"}, female: new string[] {"Pokemon_Breeder_F"});
		AddTrainer(TrainerType.Preschooler, male: new string[] {"Preschooler_M"}, female: new string[] {"Preschooler_F"});
		AddTrainer(TrainerType.Psychic, male: new string[] {"Psychic"});
		AddTrainer(TrainerType.Scientist, male: new string[] {"Scientist_M"}, female: new string[] {"Scientist_F"});
		AddTrainer(TrainerType.Swimmer, male: new string[] {"Swimmer_M"}, female: new string[] {"Swimmer_F"});
		AddTrainer(TrainerType.Veteran, male: new string[] {"Veteran_M"}, female: new string[] {"Veteran_F"});
		AddTrainer(TrainerType.Worker, male: new string[] {"Worker_A", "Worker_B"});
		AddTrainer(TrainerType.Child, male: new string[] {"Youngster"}, female: new string[] {"Lass"});
		
		m_isInit = true;
	}
	
	public static TrainerDefinition GetDefinition(TrainerDefinition.TrainerType trainerType)
	{
		Init();
		return m_trainers[trainerType];
	}
	
	public string[] MaleTextures;
	public string[] FemaleTextures;
	public bool IsMale { get { return MaleTextures != null; } }
	public bool IsFemale { get { return FemaleTextures != null; } }
	
	public TrainerDefinition.TrainerType TrainerClass { get; private set; }
	
	public TrainerDefinition(TrainerDefinition.TrainerType trainerType, string[] male=null, string[] female=null)
	{
		TrainerClass = trainerType;
		MaleTextures = male;
		FemaleTextures = female;
	}
	
	public string GetMaleString(TrainerType trainerType)
	{
		Init();
		
		ResourceManager rm = new ResourceManager("Assembly-CSharp", Assembly.GetExecutingAssembly());
		return rm.GetString(trainerType.ToString());
	}
	
}
