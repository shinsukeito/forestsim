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

		// if (age >= 40)
		// {
		// 	acre.RemoveDisaster();
		// 	return;
		// }

		switch (type)
		{
			case DisasterType.Bushfire:
				List<Acre> neighbours = acre.GetNeighbours();
				List<Acre> matched = neighbours.FindAll((n) =>
				{
					return n.fieldType == FieldType.Field || n.fieldType == FieldType.Barren;
				});

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
