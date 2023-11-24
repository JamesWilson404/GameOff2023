using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandCard : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text KeyWordText;

    public Image IconImage;
    public Color GoodColour;
    public Color BadColour;

    public Image image;

    public Card CurrentCard;

    internal void Init(Card newCard)
    {
        CurrentCard = newCard;
        image.sprite = newCard.CardArt;

        if (IconImage == null)
        {
            return;
        }

        NameText.text = newCard.CardName;
        DescriptionText.text = newCard.CardDescription;
        if (newCard.polarity == eCardPolarity.Hope)
        {
            IconImage.color = GoodColour;
        }
        else if (newCard.polarity == eCardPolarity.Blood)
        {
            IconImage.color = BadColour;
        }

        KeyWordText.text = GetKeywordText(newCard);
    }

    private string GetKeywordText(Card newCard)
    {
        var newString = "";

        if (newCard.Keywords.Count > 0)
        {
            for (int i = 0; i < newCard.Keywords.Count; i++)
            {
                switch (newCard.Keywords[i])
                {
                    case eCardKeyword.Painless:
                        newString += "Painless";
                        break;
                    case eCardKeyword.Forgetful:
                        newString += "Forgetful";
                        break;
                    case eCardKeyword.Cathartic:
                        newString += "Cathartic";
                        break;
                    case eCardKeyword.Power:
                        newString += "Power";
                        break;
                    case eCardKeyword.Solace:
                        break;
                    case eCardKeyword.Resilience:
                        break;
                    default:
                        break;
                }


                if (i < newCard.Keywords.Count-1)
                {
                    newString += "\n";
                }
            }
        }
        return newString;
    }
}
