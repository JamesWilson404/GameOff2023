using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data / Deceased")]
public class StoryData_Deceased : StoryData
{
    public Card Tombstone;

    public override void OnRoundStart()
    {
    }

    public override void OnTurnStart()
    {
        var randLocation = BoardManager.Instance.GetRandomEmptyLocations(true);
        if (randLocation != BoardManager.Instance.DeckPosition)
        {
            BoardManager.Instance.PlayCard(Tombstone, randLocation);
        }
    }
}