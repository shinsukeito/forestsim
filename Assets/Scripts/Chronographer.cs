using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public SeasonIcon[] seasonIcons;

	public Text tickText;
	public Text seasonText;
	public Text cycleText;

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

		// Set icons:
		for (int i = 0; i < seasonOrder.Length; i++)
			seasonIcons[i].SetSeason(seasonOrder[i]);

		seasonIcons[0].Highlight();
	}



	Season GetCurrentSeason()
	{
		return seasonOrder[currentSeason];
	}

	void TickTime()
	{
		currentTick++;
		if (currentTick >= ticksInSeason)
		{
			currentTick = 0;
			TickSeason();
		}

		// Update UI:
		tickText.text = $"Day: {currentSeason * ticksInSeason + currentTick + 1}";
	}

	void TickSeason()
	{
		// Unhighlight season first:
		seasonIcons[currentSeason].Unhighlight();

		currentSeason++;
		if (currentSeason >= seasonOrder.Length)// 5
		{
			currentSeason = 0;
			TickCycle();
		}

		// Update UI:
		seasonText.text = $"Season: {GetCurrentSeason()}";
		seasonIcons[currentSeason].Highlight();
	}

	void TickCycle()
	{
		currentCycle++;

		// Update UI:
		cycleText.text = $"Cycle: {currentCycle + 1}";
	}
}
