using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
	[Header("References")]
	public Terraformer terraformer;
	public Yggdrasil yggdrasil;
	public TMP_Text textCycle;
	public TMP_Text textSeason;
	public TMP_Text textDay;
	public TMP_Text textGameOver;
	public Button exitButton;

	[Header("Configurations")]
	public bool paused = true;
	public bool finished = false;
	public Season[] seasonOrder;
	public SeasonIcon[] seasonIcons;

	public float tickSpan = 0;
	public float dayLength = 1;
	public float daysInSeason = 10;

	public int currentDay = 0;
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

		if (tickSpan > dayLength)
		{
			tickSpan -= dayLength;
			TickDay();
		}
	}

	public void Commence()
	{
		TickSeason();
		paused = false;
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

	void TickDay()
	{
		currentDay++;

		// Update map:
		bool won = terraformer.OnEachDay(currentDay);

		if (won)
			GameOver(true);

		if (yggdrasil.sunlight < yggdrasil.forestCost && terraformer.GetForestCount() == 0)
			GameOver(false);

		if (yggdrasil.health <= 0)
			GameOver(false);

		if (currentDay % daysInSeason == 0)
			TickSeason();


		// Update UI:
		textDay.text = $"Day: {currentDay + 1}";
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

		// Update map:
		terraformer.OnEachSeason(GetCurrentSeason());

		// Update UI:
		textSeason.text = $"Season: {GetCurrentSeason()}";
		seasonIcons[currentSeason].Highlight();
	}

	void TickCycle()
	{
		currentCycle++;

		// Update map:
		terraformer.OnEachCycle(currentCycle);

		// Update UI:
		textCycle.text = $"Cycle: {currentCycle + 1}";
	}

	public void GameOver(bool win)
	{
		if (win)
		{
			textGameOver.text = "You won!";
			paused = true;
		}
		else
		{
			textGameOver.text = "Game over!";
			paused = true;
		}

		finished = true;
		exitButton.gameObject.SetActive(true);
	}

	public void Exit()
	{
		SceneManager.LoadScene("TitleScene");
	}
}
