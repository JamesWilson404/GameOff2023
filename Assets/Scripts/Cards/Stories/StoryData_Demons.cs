using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data / Demons")]
public class StoryData_Demons : StoryData
{

    public override void OnRoundStart()
    {
    }

    public override void OnTurnStart()
    {

    }

    public override void OnTurnEnd()
    {
        if (BoardManager.Instance.BloodTileGenerated == 0)
        {
            Game.Instance.HopeTokens = 0;
        }
    }

    public override void OnCardDiscarded(Card card)
    {
    }

    public override void OnCardPlayed(Card card)
    {
    }
}