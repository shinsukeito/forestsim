using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// https://www.youtube.com/watch?v=HXFoUGw7eKk
public class TooltipSystem : MonoBehaviour
{
	private static TooltipSystem current;

	public GameObject tooltip;

	// Start is called before the first frame update
	void Start()
	{
		current = this;
	}

	// Update is called once per frame
	void Update()
	{
	}

	public static void ShowText(string text)
	{
		current.tooltip.SetActive(true);
		current.tooltip.GetComponentInChildren<TMP_Text>().SetText(text);
	}

	public static void Hide()
	{
		current.tooltip.SetActive(false);
	}
}
