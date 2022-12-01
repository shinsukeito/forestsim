using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeasonIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public Image seasonImage;
	public Sprite fairIcon;
	public Sprite coldIcon;
	public Sprite dryIcon;
	public Sprite hotIcon;
	public Sprite wetIcon;

	public Image backgroundImage;
	public Sprite clock0;
	public Sprite clock12;
	public Sprite clock25;
	public Sprite clock37;
	public Sprite clock50;
	public Sprite clock62;
	public Sprite clock75;
	public Sprite clock87;
	public Sprite clock100;

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
				tooltipText = "No disasters this season!";
				break;
			case Season.Cold:
				seasonImage.sprite = coldIcon;
				tooltipTitle = "The Frosts";
				tooltipText = "Spawns a <color=#74F5FD>blizzard</color> in a random area";
				tooltipText += "\n\n<color=#74F5FD>Blizzard</color>";
				tooltipText += "\n - Chance to highly damage forests";
				tooltipText += "\n - Forests have a chance not to grow";
				break;
			case Season.Dry:
				seasonImage.sprite = dryIcon;
				tooltipTitle = "The Famine";
				tooltipText = "Spawns a <color=#FBEAA3>drought</color> in a random area";
				tooltipText += "\n\n<color=#FBEAA3>Drought</color>";
				tooltipText += "\n - Hinders forests from growing";
				tooltipText += "\n - Low but steady damage to forests";
				break;
			case Season.Hot:
				seasonImage.sprite = hotIcon;
				tooltipTitle = "The Burns";
				tooltipText = "Spawns a <color=#E37840>fire</color> in a random location that spreads over time";
				tooltipText += "\n\n<color=#E37840>Fire</color>";
				tooltipText += "\n - Damages forests";
				tooltipText += "\n - Has a chance to spread each day";
				break;
			case Season.Wet:
				seasonImage.sprite = wetIcon;
				tooltipTitle = "The Rains";
				tooltipText = "<color=#1475C0>Floods</color> land near rivers";
				tooltipText += "\n\n<color=#1475C0>Flood</color>";
				tooltipText += "\n - Damages forests";
				tooltipText += "\n - Deals more damage the longer it remains";
				break;
		}
	}

	public void Highlight()
	{
		backgroundImage.sprite = clock100;
	}

	public void Unhighlight()
	{
		backgroundImage.sprite = clock0;
	}

	public void SetClock(float percent)
	{
		if (percent < 0.12f) backgroundImage.sprite = clock100;
		else if (percent < 0.25f) backgroundImage.sprite = clock87;
		else if (percent < 0.37f) backgroundImage.sprite = clock75;
		else if (percent < 0.50f) backgroundImage.sprite = clock62;
		else if (percent < 0.62f) backgroundImage.sprite = clock50;
		else if (percent < 0.75f) backgroundImage.sprite = clock37;
		else if (percent < 0.87f) backgroundImage.sprite = clock25;
		else if (percent < 1f) backgroundImage.sprite = clock12;
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
