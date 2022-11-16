using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisasterType
{
	None,
	Blizzard,
	Bushfire,
	Drought,
	Flood,
}

public class Disaster
{
	private Acre acre;
	private DisasterType type;
	private int age = 0;

	public Disaster(Acre acre, DisasterType type)
	{
		this.acre = acre;
		this.type = type;
	}

	public void OnEachDay(int day)
	{
		age++;

		switch (type)
		{
			case DisasterType.Bushfire:
				// Chance to extinguish:
				if (acre.forest == null &&
					age > 1 &&
					Random.Range(0, 100) <= acre.GetBushfireExtinguishChance())
				{
					acre.RemoveDisaster();
					return;
				}

				// If not extinguished, spread:
				List<Acre> neighbours = acre.GetNeighbours(1, false);
				List<Acre> matched = neighbours.FindAll((n) =>
				{
					if (n.fieldType != FieldType.Barren && n.fieldType != FieldType.Field) return false;
					if (n.disaster != null && n.disaster.GetDisasterType() == DisasterType.Bushfire) return false;
					return true;
				});

				if (matched.Count > 0)
					matched[Random.Range(0, matched.Count)].Wreak(DisasterType.Bushfire);

				break;
		}
	}

	public void OnEachSeason(Season season)
	{
	}

	public void OnEachCycle(int cycle)
	{

	}

	public DisasterType GetDisasterType()
	{
		return type;
	}
}
