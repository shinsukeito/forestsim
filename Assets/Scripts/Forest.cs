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
	public Yggdrasil yggdrasil;
	private ForestType type;
	private int level = 1;
	private int experience = 0;
	private int health = 20;
	private int sunlightGeneration = 2;
	private int expGeneration = 3;

	private int expPerLevel = 20;
	private int maxLevel = 5;

	public Forest(Yggdrasil yggdrasil, ForestType type)
	{
		this.yggdrasil = yggdrasil;
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

	public void ChangeHealth(int amount)
	{
		health += amount;

		if (health <= 0)
		{
			health = 0;
		}
	}

	public void Photosynthesise()
	{
		yggdrasil.SetSunlight(yggdrasil.sunlight + sunlightGeneration);
	}

	public void Grow()
	{
		if (level >= maxLevel) return;

		experience += expGeneration;

		if (experience >= expPerLevel)
		{
			experience -= expPerLevel;
			level++;
		}
	}
	public void OnEachDay(int day)
	{
		Photosynthesise();
	}

	public void OnEachSeason(Season season)
	{
		Grow();
	}

	public void OnEachCycle(int cycle)
	{

	}
}
