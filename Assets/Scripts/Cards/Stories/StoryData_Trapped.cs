using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data / Trapped")]
public class StoryData_Trapped : StoryData
{
    public Card Tombstone;

    public override void OnRoundStart()
    {
        BoardManager.Instance.PlayTrappedEffect();
    }

    public override void OnTurnStart()
    {

    }

    public override void OnTurnEnd()
    {
    }
}