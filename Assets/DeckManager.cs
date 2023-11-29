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

    [SerializeField] Sprite[] DeckSizeLevel;
    [SerializeField] Vector3[] DeckTopPositions;

    [SerializeField] Card FallbackCard;

    public GameObject StoryCard;
    public SpriteRenderer DeckSprite;

    Animator animator;

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
        animator = GetComponent<Animator>();

        foreach (var item in StartingDeck.Cards)
        {
            Deck.Add(item);
        }
    }

    public void LandDeck()
    {
        animator.SetTrigger("Start");
        DeckSprite.gameObject.SetActive(true);
    }

    private void Start()
    {
        UpdateDeckUI();
    }

    public void DeckLandedAudio()
    {
        AudioManager.Instance.PlaySound(SoundFX.THUD);
    }

    public void UpdateDeckUI()
    {
        InDeckText.text = Deck.Count.ToString();
        InDiscardText.text = DiscardPile.Count.ToString();
        if (Deck.Count == 0)
        {
            DeckSprite.sprite = (DeckSizeLevel[3]);
            StoryCard.transform.position = DeckTopPositions[3];
        }
        else if (Deck.Count < 5)
        {
            DeckSprite.sprite = (DeckSizeLevel[2]);
            StoryCard.transform.position = DeckTopPositions[2];
        }
        else if (Deck.Count < 10)
        {
            DeckSprite.sprite = (DeckSizeLevel[1]);
            StoryCard.transform.position = DeckTopPositions[1];
        }
        else
        {
            DeckSprite.sprite = (DeckSizeLevel[0]);
            StoryCard.transform.position = DeckTopPositions[0];
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
        else
        {
            return FallbackCard;
        }
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

    internal int GetDeckCount()
    {
        return Deck.Count;
    }
}
