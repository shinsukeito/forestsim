#nullable enable

using System;

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
	public Acre acre;
	private ForestType type;
	private ForestStats stats;
	private Healthbar hb;
	private int level = 0;
	private int experience = 0;
	private int health;
	private int expGeneration = 10;

	private float sunlightModifier = 1;
	private float expModifier = 1;

	private int expPerLevel = 20;

	// Forest stats:
	ForestStats borealStats = new ForestStats(
		3,
		new int[3] { 25, 45, 90 },
		new int[3] { 1, 2, 4 },
		new int[3] { 0, 1, 2 }
	);

	ForestStats bushlandStats = new ForestStats(
		3,
		new int[3] { 35, 55, 100 },
		new int[3] { 0.3, 0.6, 1 },
		new int[3] { 0, 1, 2 }
	);

	ForestStats mangroveStats = new ForestStats(
		3,
		new int[3] { 20, 35, 50 },
		new int[3] { 0.5, 0.8, 1.2 },
		new int[3] { 0, 1, 2 }
	);

	ForestStats rainforestStats = new ForestStats(
		3,
		new int[3] { 15, 25, 35 },
		new int[3] { 0.5, 1, 1.5 },
		new int[3] { 0, 1, 2 }
	);

	public Forest(Yggdrasil yggdrasil, Acre acre, ForestType type, Healthbar hb)
	{
		this.yggdrasil = yggdrasil;
		this.hb = hb;
		this.acre = acre;
		this.type = type;
		switch (type)
		{
			case ForestType.Boreal:
				this.stats = borealStats;
				this.health = stats.bark[0];
				break;
			case ForestType.Bushland:
				this.stats = bushlandStats;
				this.health = stats.bark[0];
				break;
			case ForestType.Mangrove:
				this.stats = mangroveStats;
				this.health = stats.bark[0];
				break;
			case ForestType.Rainforest:
				this.stats = rainforestStats;
				this.health = stats.bark[0];
				break;
			case ForestType.WorldTree:
				this.stats = new ForestStats(
					0,
					null,
					null,
					null
				);
				break;
		}
	}

	public ForestType GetForestType()
	{
		return type;
	}

	public int GetLevel()
	{
		return level + 1;
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
		return (int)Math.Round(stats.leaves[level] * sunlightModifier);
	}

	public void ChangeHealth(int amount)
	{
		health += amount;

		hb.GetComponent<Healthbar>().SetFill(health * 1f / stats.bark[level]);

		if (health <= 0)
		{
			health = 0;
			acre.RemoveForest();
		}
	}

	public void Photosynthesise()
	{
		yggdrasil.SetSunlight(yggdrasil.sunlight + GetSunlightGeneration());
	}

	public void Grow()
	{
		if (level >= stats.maxLevel - 1) return;

		experience += (int)Math.Round(expGeneration * expModifier);

		if (experience >= expPerLevel)
		{
			experience -= expPerLevel;
			level++;

			health = stats.bark[level];
		}
	}
	public void OnEachDay(int day, Disaster? disaster)
	{
		if (type == ForestType.WorldTree) return;

		if (disaster != null)
		{
			switch (disaster.GetDisasterType())
			{
				case DisasterType.Blizzard:
					break;
				case DisasterType.Bushfire:
					ChangeHealth(-5);
					break;
				case DisasterType.Drought:
					break;
				case DisasterType.Flood:
					break;
			}
		}

		Photosynthesise();
	}

	public void OnEachSeason(Season season)
	{
		if (type == ForestType.WorldTree) return;

		Grow();
	}

	public void OnEachCycle(int cycle)
	{

	}
}
