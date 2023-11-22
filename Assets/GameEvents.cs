using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameEvents
{

    public void OnCardPlayed(Card card)
    {
        if (Game.Instance.TurnState == Game.eTurnState.PostJudgement)
        {
            if (card.Keywords.Contains(eCardKeyword.Power))
            {
                Game.Instance.RewardPlayed();
            }
        }

        card.OnPlace();

    }
}