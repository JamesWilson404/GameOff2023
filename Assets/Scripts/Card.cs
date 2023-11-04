using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string CardName;
    public string CardDescription;

    public Sprite CardArt;


    public abstract void OnPlace();


}
