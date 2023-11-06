using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeckManager : MonoBehaviour
{
    List<Card> Deck;
    List<Card> DiscardPile;

    public static DeckManager Instance;
    public CardCollection StartingDeck;

    [SerializeField] TMP_Text InDeckText;
    [SerializeField] TMP_Text InDiscardText;

    [SerializeField] Tile[] DeckSizeLevel;

    private void Awake()
    {
        if (DeckManager.Instance == null)
        {
            DeckManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        Deck = new List<Card>();
        DiscardPile = new List<Card>();

        foreach (var item in StartingDeck.Cards)
        {
            Deck.Add(item);
        }
    }

    private void Start()
    {
        UpdateDeckUI();
    }


    public void UpdateDeckUI()
    {
        InDeckText.text = Deck.Count.ToString();
        InDiscardText.text = DiscardPile.Count.ToString();
        if (Deck.Count < 5)
        {
            BoardManager.Instance.SetDeckImage(DeckSizeLevel[0]);
        }
        else if (Deck.Count < 10)
        {
            BoardManager.Instance.SetDeckImage(DeckSizeLevel[1]);
        }
        else
        {
            BoardManager.Instance.SetDeckImage(DeckSizeLevel[2]);
        }
    }

    public Card DrawCard()
    {
        if (Deck.Count > 0)
        {
            int CardToPick = Game.Instance.rand.Next(Deck.Count);
            var DrawCard = Deck[CardToPick];
            Deck.RemoveAt(CardToPick);
            UpdateDeckUI();
            return DrawCard;
        }
        return null;
    }

    internal void AddToDiscard(Card currentCard)
    {
        DiscardPile.Add(currentCard);
        UpdateDeckUI();
    }

    public void ReshuffleDeck()
    {
        foreach (var item in DiscardPile)
        {
            Deck.Add(item);
        }
        DiscardPile.Clear();
        UpdateDeckUI();
    }

    internal void AddCardToDeck(Card currentCard)
    {
        Deck.Add(currentCard);
        UpdateDeckUI();
    }
}
