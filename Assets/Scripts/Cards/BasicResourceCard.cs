using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Basic Resource Card")]
public class BasicResourceCard : Card
{
    [Header("Basic Resource Card")]
    public eCardPolarity AwardPolarity;
    public int Value;

    public override void OnDiscard()
    {
    }

    public override void OnPlace(UIGameCard gameCard)
    {
        Game.Instance.AwardResource(AwardPolarity, Value);
    }
}
