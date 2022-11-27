using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// https://www.youtube.com/watch?v=HXFoUGw7eKk
public class TooltipSystem : MonoBehaviour
{
	private static TooltipSystem current;

	public GameObject tooltip;
	public TMP_Text textTitle;
	public TMP_Text textCost;
	public TMP_Text textTooltip;

	// Start is called before the first frame update
	void Start()
	{
		current = this;
	}

	// Update is called once per frame
	void Update()
	{
	}

	public static void ShowText(string title, string cost, string text)
	{
		current.tooltip.SetActive(true);
		current.textTitle.SetText(title);
		current.textCost.SetText(cost);
		current.textTooltip.SetText(text);
	}

	public static void Hide()
	{
		current.tooltip.SetActive(false);
	}
}
