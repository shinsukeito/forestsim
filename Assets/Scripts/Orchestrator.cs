using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public enum BGM
{
	None,
	Death,
	Gameplay,
	Menu,
	Victory
}

public enum SFX
{
	UISelect,
	UIHover,
	TreeSwap,
	TreePlant,
	TreeSpell,
	TreeSpellHit,
	TreeGrow,
	FireSpawn,
	FireSpread,
	FireDamage,
	BlizzardSpawn,
	BlizzardDamage,
	FloodSpawn,
	FloodDamage,
	DroughtSpawn,
	DroughtDamage
}


public class Orchestrator : MonoBehaviour
{
	public static Orchestrator current;

	[Header("Music Events")]
	public EventReference deathEvent;
	public EventReference gameplayEvent;
	public EventReference menuEvent;
	public EventReference victoryEvent;

	[Header("Sound Effect Events")]
	public EventReference sfx_ui_select;
	public EventReference sfx_ui_hover;
	public EventReference sfx_tree_swap;
	public EventReference sfx_tree_plant;
	public EventReference sfx_tree_spell;
	public EventReference sfx_tree_spell_hit;
	public EventReference sfx_tree_grow;
	public EventReference sfx_fire_spawn;
	public EventReference sfx_fire_spread;
	public EventReference sfx_fire_damage;
	public EventReference sfx_blizzard_spawn;
	public EventReference sfx_blizzard_damage;
	public EventReference sfx_flood_spawn;
	public EventReference sfx_flood_damage;
	public EventReference sfx_drought_spawn;
	public EventReference sfx_drought_damage;

	private EventInstance gameplayEventInstance;
	private EventInstance menuEventInstance;

	private float bgmVolume = 1;

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
		gameplayEventInstance = RuntimeManager.CreateInstance(gameplayEvent);
		menuEventInstance = RuntimeManager.CreateInstance(menuEvent);
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

			case BGM.Death:
				PlayOnce(current.deathEvent, current.bgmVolume);

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
				PlayOnce(current.victoryEvent, current.bgmVolume);

				current.gameplayEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				current.menuEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				break;
		}
	}

	public static void PlaySFX(SFX sfx)
	{
		switch (sfx)
		{
			case SFX.UISelect:
				PlayOnce(current.sfx_ui_select, current.bgmVolume);
				break;
			case SFX.UIHover:
				PlayOnce(current.sfx_ui_hover, current.bgmVolume);
				break;
			case SFX.TreeSwap:
				PlayOnce(current.sfx_tree_swap, current.bgmVolume);
				break;
			case SFX.TreePlant:
				PlayOnce(current.sfx_tree_plant, current.bgmVolume);
				break;
			case SFX.TreeSpell:
				PlayOnce(current.sfx_tree_spell, current.bgmVolume);
				break;
			case SFX.TreeSpellHit:
				PlayOnce(current.sfx_tree_spell_hit, current.bgmVolume);
				break;
			case SFX.TreeGrow:
				PlayOnce(current.sfx_tree_grow, current.bgmVolume);
				break;
			case SFX.FireSpawn:
				PlayOnce(current.sfx_fire_spawn, current.bgmVolume);
				break;
			case SFX.FireSpread:
				PlayOnce(current.sfx_fire_spread, current.bgmVolume);
				break;
			case SFX.FireDamage:
				PlayOnce(current.sfx_fire_damage, current.bgmVolume);
				break;
			case SFX.BlizzardSpawn:
				PlayOnce(current.sfx_blizzard_spawn, current.bgmVolume);
				break;
			case SFX.BlizzardDamage:
				PlayOnce(current.sfx_blizzard_damage, current.bgmVolume);
				break;
			case SFX.FloodSpawn:
				PlayOnce(current.sfx_flood_spawn, current.bgmVolume);
				break;
			case SFX.FloodDamage:
				PlayOnce(current.sfx_flood_damage, current.bgmVolume);
				break;
			case SFX.DroughtSpawn:
				PlayOnce(current.sfx_drought_spawn, current.bgmVolume);
				break;
			case SFX.DroughtDamage:
				PlayOnce(current.sfx_drought_damage, current.bgmVolume);
				break;
		}
	}

	public static void SetSeason(Season season)
	{
		switch (season)
		{
			case Season.Fair:
				RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Fair");
				break;
			case Season.Cold:
				RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Frosts");
				break;
			case Season.Dry:
				RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Famine");
				break;
			case Season.Hot:
				RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Burns");
				break;
			case Season.Wet:
				RuntimeManager.StudioSystem.setParameterByNameWithLabel("Seasons", "Rains");
				break;
		}
	}

	public static void SetIntensity(int intensity)
	{
		RuntimeManager.StudioSystem.setParameterByName("Intensity", intensity);
	}

	public static void SetYggdrasil(float yggdrasil)
	{
		RuntimeManager.StudioSystem.setParameterByName("Yggdrasil", yggdrasil);
	}

	public static void Mute(bool mute)
	{
		RuntimeManager.MuteAllEvents(mute);
	}

	public static void ChangeVolume(float volume)
	{
		float logVolume = Mathf.Log(volume + 1, 2);
		current.bgmVolume = logVolume;
		current.gameplayEventInstance.setVolume(logVolume);
		current.menuEventInstance.setVolume(logVolume);
	}

	public static float GetVolume()
	{
		return current.bgmVolume;
	}

	public static void PlayOnce(EventReference eventRef, float volume, Vector3 position = new Vector3())
	{
		var instance = RuntimeManager.CreateInstance(eventRef);
		instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
		instance.setVolume(volume);
		instance.start();
		instance.release();
	}
}
