using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Cards/Blood Draw Card")]
public class BloodDrawCard : Card
{

    public override void OnPlace(UIGameCard gameCard)
    {
        if (gameCard.RequiresBloodTrail)
        {
            HandManager.Instance.DrawCard(3);
        }
        else
        {
            HandManager.Instance.DrawCard(1);
        }
    }

    public override void OnDiscard()
    {
    }
}
