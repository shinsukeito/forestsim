using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonIcon : MonoBehaviour
{

	public Image seasonImage;
	public Image backgroundImage;
	public Sprite fairIcon;

	public Sprite coldIcon;

	public Sprite dryIcon;

	public Sprite hotIcon;
	public Sprite wetIcon;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetSeason(Season season)
	{
		switch (season)
		{
			case Season.Fair:
				seasonImage.sprite = fairIcon;
				break;
			case Season.Cold:
				seasonImage.sprite = coldIcon;
				break;
			case Season.Dry:
				seasonImage.sprite = dryIcon;
				break;
			case Season.Hot:
				seasonImage.sprite = hotIcon;
				break;
			case Season.Wet:
				seasonImage.sprite = wetIcon;
				break;
		}
	}

	public void Highlight()
	{
		backgroundImage.color = new Color(255, 255, 255, 0.9f);
	}

	public void Unhighlight()
	{
		backgroundImage.color = new Color(0, 0, 0, 0.5f);
	}
}
