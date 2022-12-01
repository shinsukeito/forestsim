using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public void ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	public void SetDifficulty(int difficulty)
	{
		if (difficulty == 0) Omniscience.Difficulty = GameDifficulty.Easy;
		else if (difficulty == 1) Omniscience.Difficulty = GameDifficulty.Normal;
		else Omniscience.Difficulty = GameDifficulty.Hard;
	}

	public void PlayHoverSound()
	{
		Orchestrator.PlaySFX(SFX.UIHover);
	}

	public void PlaySelectSound()
	{
		Orchestrator.PlaySFX(SFX.UISelect);
	}

	public void ResetOrchestrator()
	{
		Orchestrator.SetSeason(Season.Fair);
		Orchestrator.SetIntensity(0);
		Orchestrator.SetYggdrasil(1f);
	}
}
