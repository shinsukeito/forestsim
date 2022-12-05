using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// https://www.youtube.com/watch?v=HXFoUGw7eKk
public class TooltipSystem : MonoBehaviour
{
	private static TooltipSystem current;

	public GameObject tooltip;
	public TMP_Text textTitle;
	public TMP_Text textCost;
	public TMP_Text textTooltip;
	public Image sunlightIcon;

	public RectTransform rect;

	private string title;
	private int cost;
	private string text;

	// Start is called before the first frame update
	void Start()
	{
		rect = tooltip.GetComponent<RectTransform>();
		current = this;
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 position = Input.mousePosition;
		float px = 0;
		float py = -0.1f;

		if (position.x < Screen.width / 2)
		{
			px = 0;
		}
		else
		{
			px = 1f;
		}

		if (position.y < Screen.height / 2)
		{
			py = -0.1f;
		}
		else
		{
			py = 1.1f;
		}

		rect.pivot = new Vector2(px, py);
	}

	public static void ShowText(string title, int cost, string text)
	{
		current.title = title;
		current.cost = cost;
		current.text = text;
		current.Invoke("Appear", 0.75f);
	}

	public static void Hide()
	{
		current.tooltip.SetActive(false);
		current.CancelInvoke("Appear");
	}

	void Appear()
	{
		current.tooltip.SetActive(true);
		current.textTitle.SetText(current.title);
		current.textTooltip.SetText(current.text);

		if (current.cost != 0)
		{
			current.textCost.gameObject.SetActive(true);
			current.sunlightIcon.gameObject.SetActive(true);
			current.textCost.SetText(current.cost.ToString());
		}
		else
		{
			current.textCost.gameObject.SetActive(false);
			current.sunlightIcon.gameObject.SetActive(false);
			current.textCost.SetText(current.cost.ToString());
		}
	}
}
