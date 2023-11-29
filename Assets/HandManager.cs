using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] Camera gameCamera;

    [SerializeField] GameObject HandCardPrefab;

    public static HandManager Instance;

    GameObject CurrentCard;

    HandRotator handRotator;
    CardLayerSorter layerSorter;

    public GameObject PlacementCard;
    public GameObject CardToPlace;
    public bool Placing = false;

    [SerializeField] CanvasGroup HandGroup;
    [SerializeField] CanvasGroup ReturnGroup;

    public GameObject DiscardPrefab;
    public GameObject DiscardParent;

    public int HandCount = 0;
    public float TimeWithout = 0;

    public int CardDrawnThisTurn = 0;
    public int CardDrawLimit = 100;


    private void Awake()
    {
        if (Instance == null)
        {
            HandManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        handRotator = GetComponent<HandRotator>();
        layerSorter = GetComponent<CardLayerSorter>();

    }

    internal void CardUnHovered(Canvas cardCanvas)
    {
        if (cardCanvas.gameObject == CurrentCard)
        {
            UnHoverCard(cardCanvas);
            CurrentCard = null;
            handRotator.SetHandRotation();
        }
    }

    internal void DrawHand()
    {
        StartCoroutine(DrawHandCoroutine());
    }

    public IEnumerator DrawHandCoroutine()
    {
        int cardsToDraw = 5;
        while (cardsToDraw > 0)
        {
            yield return new WaitForSeconds(0.2f);
            DrawCard();
            cardsToDraw--;
        }

    }

    public void DrawCard(int numberToDraw)
    {
        StartCoroutine(DrawWithWait(numberToDraw));
    }

    public IEnumerator DrawWithWait(int numberToDraw)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < numberToDraw; i++)
        {
            yield return new WaitForSeconds(0.1f);
            DrawCard();
        }
        
    }

    public void DrawCard()
    {
        if (CardDrawnThisTurn >= CardDrawLimit)
        {
            return;
        }

        var newCard = DeckManager.Instance.DrawCard();
        if (DeckManager.Instance.GetDeckCount() == 0)
        {
            Game.Instance.EndOfRound = true;
        }
        AddCardToHand(newCard);
        CardDrawnThisTurn++;
    }

    public void AddCardToHand(Card nextCard)
    {
        var newCard = Instantiate(HandCardPrefab, this.transform);
        newCard.GetComponent<UIHandCard>().Init(nextCard);
        AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
        layerSorter.ResolveCardOrdering();
        handRotator.SetHandRotation();
    }

    internal void CardHovered(Canvas cardCanvas)
    {
        if (cardCanvas.gameObject == CurrentCard)
        {
            return;
        }

        // New Card Hovered
        if (CurrentCard == null)
        {
            HoverCard(cardCanvas);
            CurrentCard = cardCanvas.gameObject;
            cardCanvas.transform.transform.rotation = Quaternion.identity;
        }
        // Unhover old card, hover on new one
        else
        {
            UnHoverCard(CurrentCard.GetComponent<Canvas>());
            handRotator.SetHandRotation();
            HoverCard(cardCanvas);
            cardCanvas.transform.transform.rotation = Quaternion.identity;
            CurrentCard = cardCanvas.gameObject;
        }
    }

    private void UnHoverCard(Canvas cardCanvas)
    {
        cardCanvas.sortingOrder = cardCanvas.transform.GetSiblingIndex();
        cardCanvas.transform.localScale = Vector3.one * 1f;
        var anim = cardCanvas.transform.GetComponent<Animator>();
        anim.SetBool("Selected", false);
        anim.Play("Default");
        Game.Instance.GameTooltip.HandCardUnhovered();
    }

    private void HoverCard(Canvas cardCanvas)
    {
        cardCanvas.sortingOrder = 99;
        cardCanvas.transform.localScale = Vector3.one * 1.2f;
        cardCanvas.transform.GetComponent<Animator>().SetBool("Selected", true);
        Game.Instance.GameTooltip.HandCardHovered(cardCanvas.GetComponent<UIHandCard>().CurrentCard.Keywords, gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        var mousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;


        Card card = null;
        if (CardToPlace != null)
        {
            card = CardToPlace.GetComponent<UIHandCard>().CurrentCard;
        }


        if (Game.Instance.TurnState == Game.eTurnState.InTurn)
        {
            UpdateHandCount();
        }

        if (Placing && card != null)
        {
            var nearestTilePos = BoardManager.Instance.GetNearestTile(mousePosition);
            if (BoardManager.Instance.IsPlacementValid(nearestTilePos, card.Keywords.Contains(eCardKeyword.Cathartic)))
            {
                mousePosition = BoardManager.Instance.GetCellCenter(nearestTilePos);
            }


            UpdatePlacementCard(mousePosition);
            if (Input.GetMouseButtonUp(0))
            {
                Placing = false;
                PlacementCard.SetActive(false);
                HandGroup.alpha = 1;
                ReturnGroup.alpha = 0;

                if (BoardManager.Instance.TryPlaceCard(mousePosition, card))
                {
                    Destroy(CardToPlace.gameObject);
                    //DiscardCard(CardToPlace.gameObject);
                    
                    layerSorter.ResolveCardOrdering();
                    handRotator.SetHandRotation();

                    if (Game.Instance.TurnState == Game.eTurnState.PostJudgement)
                    {
                        if ( card.Keywords.Contains(eCardKeyword.Power))
                        {
                            Game.Instance.RewardPlayed();
                        }
                    }


                }

                CardToPlace = null;
            }
        }
        else
        {
            if (CurrentCard != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Placing = true;
                    UpdatePlacementCard(mousePosition);
                    HandGroup.alpha = 0;
                    ReturnGroup.alpha = 1;
                    CardToPlace = CurrentCard;
                }
            }
        }
    }

    private void UpdateHandCount()
    {
        HandCount = transform.childCount;
        if (HandCount == 0 && Game.Instance.TurnState == Game.eTurnState.InTurn)
        {
            TimeWithout += Time.deltaTime;
            if (TimeWithout > 1f)
            {
                Game.Instance.UIEndTurnPressed();
            }
        }
        else
        {
            TimeWithout = 0;
        }
    }

    private void UpdatePlacementCard(Vector3 mousePos)
    {
        PlacementCard.SetActive(true);

        PlacementCard.transform.position = mousePos;
    }

    internal void DestroyHand()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }

    internal void DiscardRandomCard()
    {
        if (transform.childCount > 0)
        {
            int child = Game.Instance.rand.Next(transform.childCount);
            DiscardCard(transform.GetChild(child).gameObject);
        }
    }

    private void DiscardCard(GameObject gameObject)
    {

        if (CurrentCard == gameObject)
        {
            CurrentCard = null;
        }

        foreach (Transform item in transform)
        {
            if (item.gameObject == gameObject)
            {
                var uiHand = gameObject.GetComponent<UIHandCard>();
                var gCard = uiHand.CurrentCard;
                Game.Instance.EventManager.OnCardDiscarded(gCard);
                DeckManager.Instance.AddToDiscard(gCard);
                SpawnDiscard(gameObject, gCard);
                Destroy(gameObject);
                return;
            }
        }

    }

    private void SpawnDiscard(GameObject g, Card gCard)
    {
        var newDiscard = Instantiate(DiscardPrefab, gameObject.transform.position, Quaternion.identity, DiscardParent.transform);
        var card = newDiscard.GetComponent<UIHandCard>();
        card.Init(gCard);
        card.transform.localPosition = g.transform.localPosition;
    }
}
