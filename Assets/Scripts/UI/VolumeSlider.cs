using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	public Slider slider;

	void Start()
	{
		slider.value = Orchestrator.GetVolume();
	}

	// Update is called once per frame
	void Update()
	{
		slider.onValueChanged.AddListener(SetVolume);
	}

	public void SetVolume(float volume)
	{
		Orchestrator.ChangeVolume(volume);
	}
}
