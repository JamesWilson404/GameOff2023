using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCardInteractor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Canvas cardCanvas;
    
    private void Awake()
    {
        cardCanvas = GetComponent<Canvas>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        HandManager.Instance.CardHovered(cardCanvas);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandManager.Instance.CardUnHovered(cardCanvas);
    }
}
