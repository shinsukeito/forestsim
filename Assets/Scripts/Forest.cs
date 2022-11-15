public enum ForestType
{
	None,
	WorldTree,
	Boreal,
	Bushland,
	Mangrove,
	Rainforest
}

public class Forest
{
	private ForestType type;
	private int level = 1;
	private int experience = 0;
	private int health = 20;
	private int sunlightGeneration = 2;

	private int expPerLevel = 20;
	private int maxLevel = 5;

	public Forest(ForestType type)
	{
		this.type = type;
	}

	public ForestType GetForestType()
	{
		return type;
	}

	public int GetLevel()
	{
		return level;
	}

	public int GetExperience()
	{
		return experience;
	}

	public int GetHealth()
	{
		return health;
	}

	public int GetSunlightGeneration()
	{
		return sunlightGeneration;
	}

	public void AddExperience(int amount)
	{
		if (level >= maxLevel) return;

		experience += amount;

		if (experience >= expPerLevel)
		{
			experience -= expPerLevel;
			level++;
		}
	}

	public void ChangeHealth(int amount)
	{
		health += amount;

		if (health <= 0)
		{
			health = 0;
		}
	}
}
