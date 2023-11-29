using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data / Rejected")]
public class StoryData_Rejected : StoryData
{

    public override void OnRoundStart()
    {
    }

    public override void OnTurnStart()
    {

    }

    public override void OnTurnEnd()
    {
    }

    public override void OnCardDiscarded(Card card)
    {
        Game.Instance.AwardResource(eCardPolarity.Blood, 2);
    }

    public override void OnCardPlayed(Card card)
    {
    }
}