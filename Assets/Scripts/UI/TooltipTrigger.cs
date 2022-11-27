using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public string title;
	public string cost;
	[TextArea]
	public string text;

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		TooltipSystem.ShowText(title, cost, text);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		TooltipSystem.Hide();
	}
}
