﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Story Data / Addicted")]
public class StoryData_Addicted : StoryData
{
    public Card Bottle;

    public override void OnRoundStart()
    {
    }

    public override void OnTurnEnd()
    {
        if (Game.Instance.hopeThisTurn <= 10)
        {
            DeckManager.Instance.AddToDiscard(Bottle);
            AudioManager.Instance.PlaySound(SoundFX.GULP);
        }

    }

    public override void OnTurnStart()
    {

    }
}