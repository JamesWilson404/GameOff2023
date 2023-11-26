using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound(SoundFX.UIHOVER);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
