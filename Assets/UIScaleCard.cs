using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScaleCard : UICard, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer CardShadow;
    public SpriteRenderer CardBacking;
    public Image CardSprite;

    public TMP_Text CostText;

    public CanvasGroup canvasGroup;

    public Sprite HopeIcon;
    public Sprite BloodIcon;

    public GameObject Scalers;
    public bool aloud = false;

    public bool inside = false;

    public void Init(eCardPolarity polarity)
    {
        if (polarity == eCardPolarity.Hope)
        {
            CardSprite.sprite = HopeIcon;
            CostText.text = "0";
            aloud = true;
        }
        else if (polarity == eCardPolarity.Blood)
        {
            CardSprite.sprite = BloodIcon;
            CostText.text = Game.Instance.BloodTokens.ToString();
        }

        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    public void Update()
    {
        if (aloud && inside)
        {
            var scrollDelta = Input.mouseScrollDelta;
            if (scrollDelta.y > 0)
            {
                ChangeValue(1);
            }
            else if (scrollDelta.y < 0)
            {
                ChangeValue(-1);
            }
        }
    }

    public void ChangeValue(int v)
    {
        int current = int.Parse(CostText.text);
        if (current == 0 && v < 0)
        {
            AudioManager.Instance.PlaySound(SoundFX.CANT_AFFORD);
            return;
        }
        else if (current == Game.Instance.HopeTokens && v > 0)
        {
            AudioManager.Instance.PlaySound(SoundFX.CANT_AFFORD);
            return;
        }

        current = Mathf.Clamp(current + v, 0, Game.Instance.HopeTokens);
        CostText.text = current.ToString();
        AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inside = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
    }
}
