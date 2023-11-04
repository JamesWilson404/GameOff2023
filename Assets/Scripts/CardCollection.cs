using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Deck/CardCollection")]
public class CardCollection : ScriptableObject
{
    public string DeckName;
    public List<Card> Cards;
}
