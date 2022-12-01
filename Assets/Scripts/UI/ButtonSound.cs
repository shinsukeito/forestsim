using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		Orchestrator.PlaySFX(SFX.UISelect);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Orchestrator.PlaySFX(SFX.UIHover);
	}
}
