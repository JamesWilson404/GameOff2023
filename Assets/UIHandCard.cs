using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandCard : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text DescriptionText;

    public Image image;

    public Card CurrentCard;

    internal void Init(Card newCard)
    {
        CurrentCard = newCard;
        NameText.text = newCard.CardName;
        DescriptionText.text = newCard.CardDescription;
        image.sprite = newCard.CardArt;
    }
}
