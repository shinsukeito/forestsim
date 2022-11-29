using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeasonIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public Image seasonImage;
	public Image backgroundImage;
	public Sprite fairIcon;

	public Sprite coldIcon;

	public Sprite dryIcon;

	public Sprite hotIcon;
	public Sprite wetIcon;

	private string tooltipTitle = "";
	private string tooltipText = "";

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
				tooltipTitle = "Fair Season";
				tooltipText = "No disaters this season";
				break;
			case Season.Cold:
				seasonImage.sprite = coldIcon;
				tooltipTitle = "Cold Season";
				tooltipText = "Spawns <color=#74F5FD>the Frosts</color> in a random area";
				break;
			case Season.Dry:
				seasonImage.sprite = dryIcon;
				tooltipTitle = "Dry Season";
				tooltipText = "Spawns <color=#FBEAA3>the Famine</color> in a random area";
				break;
			case Season.Hot:
				seasonImage.sprite = hotIcon;
				tooltipTitle = "Hot Season";
				tooltipText = "Spawns <color=#E37840>the Burns</color> in a random location that spreads over time";
				break;
			case Season.Wet:
				seasonImage.sprite = wetIcon;
				tooltipTitle = "Wet Season";
				tooltipText = "Spawns <color=#1475C0>the Rains</color> near rivers";
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

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		TooltipSystem.ShowText(tooltipTitle, 0, tooltipText);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		TooltipSystem.Hide();
	}
}
