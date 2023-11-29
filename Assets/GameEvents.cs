using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameEvents
{

    public void OnCardPlayed(UIGameCard gameCard, Card card)
    {
        card.OnPlace(gameCard);
        Game.Instance.CurrentStory.OnCardPlayed(card);
    }


    internal void OnResourceGained(eCardPolarity polarity, int value)
    {
        if (Game.Instance.inBoss)
        {
            switch (polarity)
            {
                case eCardPolarity.Hope:
                    Game.Instance.DamageBoss(value);
                    break;
                case eCardPolarity.Blood:
                    Game.Instance.HealBoss(value);
                    break;
                default:
                    break;
            }
        }
    }

    internal void OnCardDiscarded(Card card)
    {
        card.OnDiscard();
        Game.Instance.CurrentStory.OnCardDiscarded(card);
    }
}