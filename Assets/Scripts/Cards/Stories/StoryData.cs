using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryData : ScriptableObject
{
    public string StoryTitle;
    public Sprite StorySprite;
    public VoiceFX CardVO;

    public Card[] GoodRewards;
    public Card[] BadRewards;

}
