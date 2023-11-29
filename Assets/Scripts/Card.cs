using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eCardPolarity
{
    Hope,
    Blood
}

public enum eCardKeyword
{
    Painless,
    Forgetful,
    Cathartic,
    Power,
    Solace,
    Resilience,
    StoryKeep,
    StoryPainless,
    Story,
}

public abstract class Card : ScriptableObject
{
    [Header("Card Info")]
    public string CardName;
    [TextArea()]
    public string CardDescription;

    public int cost;
    public eCardPolarity polarity;

    public Sprite CardArt;
    public List<eCardKeyword> Keywords;




    public abstract void OnPlace(UIGameCard gameCard);
    public abstract void OnDiscard();

}
