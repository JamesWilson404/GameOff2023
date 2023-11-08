using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventCard : UICard, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer CardShadow;
    public SpriteRenderer CardBacking;
    public SpriteRenderer CardSprite;

    public Image CostIcon;
    public TMP_Text CostText;

    public CanvasGroup canvasGroup;

    public Sprite HopeIcon;
    public Sprite BloodIcon;

    public override void Init(Card newCard)
    {
        CurrentCard = newCard;
        spriteRenderer.sprite = newCard.CardArt;

        if (newCard.polarity == eCardPolarity.Hope)
        {
            CostIcon.sprite = HopeIcon;
        }
        else if (newCard.polarity == eCardPolarity.Blood)
        {
            CostIcon.sprite = BloodIcon;
        }

        CostText.text = newCard.cost.ToString();

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Instance.CardHovered(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Instance.CardUnHovered(this);
    }

    internal void StopDragging()
    {
        CardShadow.sortingLayerName = "Default";
        CardBacking.sortingLayerName = "Default";
        CardSprite.sortingLayerName = "Default";
        canvasGroup.gameObject.SetActive(true);
    }

    internal void StartDragging()
    {
        CardShadow.sortingLayerName = "GameUI";
        CardBacking.sortingLayerName = "GameUI";
        CardSprite.sortingLayerName = "GameUI";
        canvasGroup.gameObject.SetActive(false);
    }
}
