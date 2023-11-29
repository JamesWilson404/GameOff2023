using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventCard : UICard, IPointerEnterHandler, IPointerExitHandler
{

    public Image CostIcon;
    public TMP_Text CostText;

    public CanvasGroup canvasGroup;

    public Sprite HopeIcon;
    public Sprite BloodIcon;

    public override void Init(Card newCard, Vector2Int position)
    {
        CurrentCard = newCard;
        CardSprite.sprite = newCard.CardArt;
        this.position = position;

        if (newCard.polarity == eCardPolarity.Hope)
        {
            CostIcon.sprite = HopeIcon;
        }
        else if (newCard.polarity == eCardPolarity.Blood)
        {
            CostIcon.sprite = BloodIcon;
        }

        CostText.text = newCard.cost.ToString();
        if (BoardManager.Instance.CurrentEvent == eEventType.BadCardPick || BoardManager.Instance.CurrentEvent == eEventType.GoodCardPick)
        {
            canvasGroup.gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.gameObject.SetActive(true);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Instance.CardHovered(this);
        Game.Instance.ShopTooltip.CardHovered(CurrentCard, gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Instance.CardUnHovered(this);
        Game.Instance.ShopTooltip.CardUnHovered();
    }

    internal void StopDragging()
    {
        CardShadow.sortingLayerName = "Default";
        CardBacking.sortingLayerName = "Default";
        CardSprite.sortingLayerName = "Default";
        if (BoardManager.Instance.CurrentEvent == eEventType.BadCardPick || BoardManager.Instance.CurrentEvent == eEventType.GoodCardPick)
        {
            canvasGroup.gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.gameObject.SetActive(true);
        }
    }

    internal void StartDragging()
    {
        CardShadow.sortingLayerName = "GameUI";
        CardBacking.sortingLayerName = "GameUI";
        CardSprite.sortingLayerName = "GameUI";
        canvasGroup.gameObject.SetActive(false);
    }
}
