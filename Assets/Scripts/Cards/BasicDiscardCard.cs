using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Basic Discard Card")]
public class BasicDiscardCard : Card
{

    public override void OnPlace(UIGameCard gameCard)
    {
        HandManager.Instance.DiscardRandomCard();
        Game.Instance.AwardResource(eCardPolarity.Hope, 3);
    }
}
