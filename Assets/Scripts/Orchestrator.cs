using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public enum BGM
{
	None,
	Menu,
	Gameplay,
	Victory
}

public class Orchestrator : MonoBehaviour
{
	public static Orchestrator current;

	public FMODUnity.EventReference gameplayEvent;
	public FMODUnity.EventReference menuEvent;
	public FMODUnity.EventReference victoryEvent;

	private EventInstance gameplayEventInstance;
	private EventInstance menuEventInstance;

	void Awake()
	{
		if (current == null)
		{
			DontDestroyOnLoad(gameObject);
			current = this;
		}
		else if (current != this)
		{
			Destroy(gameObject);
		}

		SceneManager.activeSceneChanged += OnSceneChange;
	}

	void Start()
	{
		// Load event instances:
		gameplayEventInstance = FMODUnity.RuntimeManager.CreateInstance(gameplayEvent);
		menuEventInstance = FMODUnity.RuntimeManager.CreateInstance(menuEvent);
		PlayBGM(BGM.Menu);
	}

	private void OnSceneChange(Scene prev, Scene next)
	{
		switch (next.name)
		{
			case "TitleScene":
				PlayBGM(BGM.Menu);
				break;
			case "TutorialScene":
				PlayBGM(BGM.Menu);
				break;
			case "GameScene":
				PlayBGM(BGM.Gameplay);
				break;
		}
	}

	public static void PlayBGM(BGM bgm)
	{
		PLAYBACK_STATE state;

		switch (bgm)
		{
			case BGM.None:
				current.gameplayEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				current.menuEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				break;

			case BGM.Gameplay:
				current.gameplayEventInstance.getPlaybackState(out state);
				if (state != PLAYBACK_STATE.PLAYING) current.gameplayEventInstance.start();

				current.menuEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				break;
			case BGM.Menu:
				current.menuEventInstance.getPlaybackState(out state);
				if (state != PLAYBACK_STATE.PLAYING) current.menuEventInstance.start();

				current.gameplayEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				break;
			case BGM.Victory:
				RuntimeManager.PlayOneShot(current.victoryEvent);

				current.gameplayEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				current.menuEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				break;
		}
	}

	public static void SetSeason(Season season)
	{
		switch (season)
		{
			case Season.Fair:
				FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Fair");
				break;
			case Season.Cold:
				FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Frosts");
				break;
			case Season.Dry:
				FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Famine");
				break;
			case Season.Hot:
				FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Burns");
				break;
			case Season.Wet:
				FMODUnity.RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Rains");
				break;
		}
	}

	public static void SetIntensity(int intensity)
	{
		FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", intensity);
	}

	public static void SetYggdrasil(float yggdrasil)
	{
		FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Yggdrasil", yggdrasil);
	}

	public static void Mute(bool mute)
	{
		RuntimeManager.MuteAllEvents(mute);
	}

}
