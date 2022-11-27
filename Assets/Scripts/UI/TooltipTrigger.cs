using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	[TextArea]
	public string tooltipText;

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		TooltipSystem.ShowText(tooltipText);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		TooltipSystem.Hide();
	}
}
