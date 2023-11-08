using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eCardPolarity
{
    Hope,
    Blood
}


public abstract class Card : ScriptableObject
{
    [Header("Card Info")]
    public string CardName;
    public string CardDescription;

    public int cost;
    public eCardPolarity polarity;

    public Sprite CardArt;


    public abstract void OnPlace();


}
