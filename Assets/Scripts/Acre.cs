#nullable enable

using System.Collections.Generic;

public enum FieldType
{
	Barren,
	Field,
	Ocean,
	River
}

public class Acre
{
	private Terraformer terraformer;
	public int x;
	public int y;
	public FieldType fieldType = FieldType.Ocean;
	public Forest? forest = null;
	public Disaster? disaster = null;
	public DisasterType nextDisaster = DisasterType.None;

	public Acre(Terraformer terraformer, int x, int y)
	{
		this.terraformer = terraformer;
		this.x = x;
		this.y = y;
	}

	public void OnEachDay(int day)
	{
		if (forest != null) forest.OnEachDay(day, disaster);
		if (disaster != null) disaster.OnEachDay(day, forest);

		if (nextDisaster != DisasterType.None)
		{
			terraformer.Wreak(nextDisaster, x, y, true);
			nextDisaster = DisasterType.None;
		}
	}

	public void OnEachSeason(Season season)
	{
		if (forest != null) forest.OnEachSeason(season);
		if (disaster != null) disaster.OnEachSeason(season);
	}

	public void OnEachCycle(int cycle)
	{
		if (forest != null) forest.OnEachCycle(cycle);
		if (disaster != null) disaster.OnEachCycle(cycle);
	}

	public void RepaintForest()
	{
		terraformer.PaintForest(x, y);
	}

	public void RepaintDisaster()
	{
		terraformer.PaintDisaster(x, y);
	}

	public void RemoveForest()
	{
		forest = null;
		terraformer.EraseForest(x, y);
	}

	public void RemoveDisaster(DisasterType disasterType)
	{
		if (disaster == null || disaster.GetDisasterType() != disasterType) return;
		disaster = null;
		terraformer.EraseDisaster(x, y);
	}

	public bool TryRemoveDisaster(DisasterType disasterType)
	{
		if (disaster == null || disaster.GetDisasterType() != disasterType) return false;
		disaster = null;
		terraformer.EraseDisaster(x, y);

		// Remove surrounding pending bushfires:
		if (disasterType == DisasterType.Bushfire)
		{
			var neighbours = GetNeighbours(1, false);
			neighbours.ForEach((n) =>
			{
				n.nextDisaster = DisasterType.None;
			});
		}

		return true;
	}

	public List<Acre> GetNeighbours(int size, bool circular)
	{
		return terraformer.GetNeighbours(x, y, size, false, circular);
	}

	public void Wreak(DisasterType disasterType)
	{
		nextDisaster = disasterType;
	}

	public int GetBushfireExtinguishChance()
	{
		return terraformer.bushfireExtinguishChance;
	}

	public int GetBlizzardHinderChance(int level)
	{
		return terraformer.blizzardHinderChance[level];
	}

	public int GetBlizzardDestroyChance(int level)
	{
		return terraformer.blizzardDestroyChance[level];
	}

	public int GetBlizzardYggdrasilDamageChance()
	{
		return terraformer.blizzardYggdrasilDamageChance;
	}

	public int GetDroughtDamageChance()
	{
		return terraformer.droughtDamageChance;
	}

	public int GetBushfireDamage()
	{
		return terraformer.bushfireDamage;
	}

	public int GetDroughtDamage()
	{
		return terraformer.droughtDamage;
	}

	public int GetBlizzardDamage()
	{
		return terraformer.blizzardDamage;
	}



	public float GetDroughtHinderModifier()
	{
		return terraformer.droughtHinderModifier;
	}

	public float GetFloodDamage()
	{
		return terraformer.floodDamage;
	}
}
