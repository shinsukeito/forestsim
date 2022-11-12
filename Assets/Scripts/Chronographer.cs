using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Season
{
	Fair,
	Cold,
	Dry,
	Hot,
	Wet
}

public class Chronographer : MonoBehaviour
{
	public bool paused = true;
	public Season[] seasonOrder;

	public float tickSpan = 0;
	public float tickLength = 1;
	public float ticksInSeason = 10;

	public float currentTick = 0;
	public int currentSeason;
	public int currentCycle;

	// Start is called before the first frame update
	void Start()
	{
		GenerateCycle();
	}

	// Update is called once per frame
	void Update()
	{
		if (paused) return;

		tickSpan += Time.deltaTime;

		if (tickSpan > tickLength)
		{
			tickSpan -= tickLength;
			TickTime();
		}
	}

	void GenerateCycle()
	{
		// Randomise seasons:
		Season[] shuffledSeasons = { Season.Cold, Season.Dry, Season.Hot, Season.Wet };
		int n = shuffledSeasons.Length;
		System.Random rng = new System.Random();
		while (n > 1)
		{
			int k = rng.Next(n--);
			Season temp = shuffledSeasons[n];
			shuffledSeasons[n] = shuffledSeasons[k];
			shuffledSeasons[k] = temp;
		}

		// Always start with fair season:
		seasonOrder = new Season[5];
		seasonOrder[0] = Season.Fair;
		System.Array.Copy(shuffledSeasons, 0, seasonOrder, 1, shuffledSeasons.Length);
		currentSeason = 0;
	}



	Season GetCurrentSeason()
	{
		return seasonOrder[currentSeason];
	}

	void TickTime()
	{
		currentTick++;
		if (currentTick >= ticksInSeason)
			TickSeason();

	}

	void TickSeason()
	{
		currentTick = 0;
		currentSeason++;
		if (currentSeason >= seasonOrder.Length) // 5
			TickCycle();

	}

	void TickCycle()
	{
		currentSeason = 0;
		currentCycle++;
	}
}
