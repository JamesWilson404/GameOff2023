using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Cards/Draw Card")]
public class DrawCard : Card
{
    public int cardToDraw;

    public override void OnPlace(UIGameCard gameCard)
    {
        HandManager.Instance.DrawCard(cardToDraw);
    }

    public override void OnDiscard()
    {
    }
}
