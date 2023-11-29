using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryData : ScriptableObject
{
    public string StoryTitle;
    public string StoryDescription;
    public Sprite StorySprite;
    public VoiceFX CardVO;

    public Card[] GoodRewards;
    public Card[] BadRewards;

    public abstract void OnCardPlayed(Card card);

    public abstract void OnTurnStart();
    public abstract void OnRoundStart();

    public abstract void OnTurnEnd();

    public abstract void OnCardDiscarded(Card card);
}
