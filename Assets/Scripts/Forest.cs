#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private int expPerLevel = 40;

	// Forest stats:
	ForestStats borealStats = new ForestStats(
		3,
		new int[3] { 25, 50, 50 },
		new int[3] { 9, 14, 14 },
		new int[3] { 0, 1, 2 },
		new int[3] { 25, 50, 50 }
	);

	ForestStats bushlandStats = new ForestStats(
		3,
		new int[3] { 35, 60, 60 },
		new int[3] { 4, 5, 5 },
		new int[3] { 0, 1, 2 },
		new int[3] { 25, 50, 50 }
	);

	ForestStats mangroveStats = new ForestStats(
		3,
		new int[3] { 25, 40, 40 },
		new int[3] { 5, 7, 7 },
		new int[3] { 0, 1, 2 },
		new int[3] { 25, 50, 50 }
	);

	ForestStats rainforestStats = new ForestStats(
		3,
		new int[3] { 20, 35, 35 },
		new int[3] { 6, 9, 9 },
		new int[3] { 0, 1, 2 },
		new int[3] { 25, 50, 50 }
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
		return level;
	}

	public bool FullyGrown()
	{
		return level == stats.maxLevel - 1;
	}

	public string GetExperience()
	{
		if (level == stats.maxLevel - 1) return "MAX";
		return $"{experience} / {expPerLevel}";
	}

	public string GetMaturity()
	{
		switch (level)
		{
			case 0:
				return "Sapling";
			case 1:
				return "Young";
			case 2:
				return "Stout";
		}
		return "";
	}

	public string GetHealth()
	{
		return $"{health} / {stats.bark[level]}";
	}

	public int GetSunlightGeneration()
	{
		float sunlightModifier = 1;
		return Mathf.RoundToInt(stats.leaves[level] * sunlightModifier);
	}

	public void ChangeHealth(int amount)
	{
		health += amount;

		if (health <= 0)
		{
			health = 0;
			acre.RemoveForest();
		}

		hb.GetComponent<Healthbar>().SetFill(health * 1f / stats.bark[level]);
	}

	public void Photosynthesise()
	{
		yggdrasil.SetSunlight(yggdrasil.sunlight + GetSunlightGeneration());
	}

	public void Grow()
	{
		float expModifier = 1;

		if (level >= stats.maxLevel - 1) return;
		if (acre.disaster != null)
		{
			switch (acre.disaster.GetDisasterType())
			{
				case DisasterType.Blizzard:
					if (Random.Range(0, 100) <= acre.GetBlizzardHinderChance(level))
						return;
					break;
				case DisasterType.Drought:
					expModifier = acre.GetDroughtHinderModifier();
					break;
			}
		}

		experience += Mathf.RoundToInt(expGeneration * expModifier);

		if (experience >= expPerLevel)
		{
			experience -= expPerLevel;
			level++;

			acre.RepaintForest();

			health = stats.bark[level];
			hb.GetComponent<Healthbar>().SetFill(health * 1f / stats.bark[level]);
		}
	}
	public void OnEachDay(int day, Disaster? disaster)
	{
		if (type == ForestType.WorldTree)
		{
			if (disaster != null)
			{
				switch (disaster.GetDisasterType())
				{
					case DisasterType.Blizzard:
						if (Random.Range(0, 100) <= acre.GetBlizzardYggdrasilDamageChance())
						{
							yggdrasil.ChangeHealth(-acre.GetBlizzardDamage());
							return;
						}
						break;
					case DisasterType.Bushfire:
						yggdrasil.ChangeHealth(-acre.GetBushfireDamage());
						break;
					case DisasterType.Drought:
						if (Random.Range(0, 100) <= acre.GetDroughtDamageChance())
						{
							yggdrasil.ChangeHealth(-acre.GetDroughtDamage());
							return;
						}
						break;
					case DisasterType.Flood:
						yggdrasil.ChangeHealth(Mathf.RoundToInt(-acre.GetFloodDamage() * disaster.GetAge()));
						break;
				}
			}

			return;
		}

		if (disaster != null)
		{
			switch (disaster.GetDisasterType())
			{
				case DisasterType.Blizzard:
					if (Random.Range(0, 100) <= acre.GetBlizzardDestroyChance(level))
					{
						ChangeHealth(-acre.GetBlizzardDamage());
						return;
					}
					break;
				case DisasterType.Bushfire:
					ChangeHealth(-acre.GetBushfireDamage());
					break;
				case DisasterType.Drought:
					if (Random.Range(0, 100) <= acre.GetDroughtDamageChance())
					{
						ChangeHealth(-acre.GetDroughtDamage());
						return;
					}
					break;
				case DisasterType.Flood:
					if (disaster.GetAge() > 1)
						ChangeHealth(Mathf.RoundToInt(-acre.GetFloodDamage() * disaster.GetAge()));
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

	public int GetSpellCost()
	{
		return stats.costs[level];
	}
}
