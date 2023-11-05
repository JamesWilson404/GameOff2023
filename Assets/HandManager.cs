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
    bool Placing = false;

    [SerializeField] CanvasGroup HandGroup;
    [SerializeField] CanvasGroup ReturnGroup;

    public int HandCount = 0;

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
            Card nextCard = DeckManager.Instance.DrawCard();
            if (nextCard != null)
            {
                var newCard = Instantiate(HandCardPrefab, this.transform);
                newCard.GetComponent<UIHandCard>().Init(nextCard);
                AudioManager.Instance.PlaySound(SoundFX.CARD_DRAWN);
                layerSorter.ResolveCardOrdering();
                handRotator.SetHandRotation();
                cardsToDraw--;
            }
            else
            {
                // Shuffle Required
                DeckManager.Instance.ReshuffleDeck();
            }
        }
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
    }

    private void HoverCard(Canvas cardCanvas)
    {
        cardCanvas.sortingOrder = 99;
        cardCanvas.transform.localScale = Vector3.one * 1.2f;
        cardCanvas.transform.GetComponent<Animator>().SetBool("Selected", true);
    }


    // Update is called once per frame
    void Update()
    {
        var mousePosition = gameCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        if (Game.Instance.TurnState == Game.eTurnState.InTurn)
        {
            UpdateHandCount();
        }

        if (Placing)
        {
            var nearestTilePos = BoardManager.Instance.GetNearestTile(mousePosition);
            if (BoardManager.Instance.IsPlacementValid(nearestTilePos))
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
                if (BoardManager.Instance.TryPlaceCard(mousePosition, CardToPlace))
                {
                    Destroy(CardToPlace.gameObject);
                    layerSorter.ResolveCardOrdering();
                    handRotator.SetHandRotation();                    
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
        if (HandCount == 0)
        {
            Game.Instance.UIEndTurnPressed();
        }
    }

    private void UpdatePlacementCard(Vector3 mousePos)
    {
        PlacementCard.SetActive(true);

        PlacementCard.transform.position = mousePos;
    }
}
