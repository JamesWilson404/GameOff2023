using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data / Innocent")]
public class StoryData_Innocent : StoryData
{

    public override void OnRoundStart()
    {
        
    }

    public override void OnTurnStart()
    {
        HandManager.Instance.CardDrawLimit = 7;
    }

    public override void OnTurnEnd()
    {
        HandManager.Instance.CardDrawLimit = 100;
    }

    public override void OnCardDiscarded(Card card)
    {
    }

    public override void OnCardPlayed(Card card)
    {
    }
}